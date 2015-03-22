// The MIT License (MIT)
// 
// Copyright (c) 2015 Daniel Franklin. http://blazedsp.com/
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using Library.Interfaces.SimpleInjector;
using Library.Models.Database;
using Library.Models.ProtoBuf;

using Microsoft.AspNet.Identity;

namespace Frontend.Controllers.Api
{
    public class DestinationsController : ApiController
    {
        private readonly IDatabase _sql;
        private readonly IMessageBus _messageBus;

        public DestinationsController(IDatabase sql, IMessageBus messageBus)
        {
            _sql = sql;
            _messageBus = messageBus;
        }

        public async Task<IHttpActionResult> Get()
        {
            var result = await _sql.QueryAsync<Destination>("SELECT * FROM Destinations WHERE UserId = @User", new
            {
                User = User.Identity.GetUserId()
            });

            return Ok(result);
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            var result = await _sql.QueryAsync<Destination>("SELECT TOP 1 * FROM Destinations WHERE UserId = @User AND Id = @Id", new
            {
                User = User.Identity.GetUserId(),
                Id = id
            });

            return Ok(result.SingleOrDefault());
        }

        public async Task<IHttpActionResult> Post([FromBody] Destination model)
        {
            if (!ModelState.IsValid)
            {
                Validate(model);
                return BadRequest(ModelState);
            }

            // Ignore model values
            model.UserId = User.Identity.GetUserId();
            model.AddedDate = DateTimeOffset.UtcNow;
            model.ModifiedDate = null;

            var result = await _sql.CreateAsync(model);
            if (result < 1)
            {
                return BadRequest();
            }

            // TODO: Link Destinations to Flights
            //var resultBridge = await _sql.CreateAsync(new BridgeFlightsDestinations
            //{
            //    Flight = model.Flights.,
            //    Destination = model.Id
            //});
            //if (resultBridge < 1)
            //{
            //    return BadRequest();
            //}

            // Inform Worker there has been an update to a Destination
            await _messageBus.QueueSendAsync(new Update { Destination = model.Id, Url = model.Url});

            return Ok(model);
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody] Destination model)
        {
            if (!ModelState.IsValid)
            {
                Validate(model);
                return BadRequest(ModelState);
            }

            // Set ID because Ember doesn't send it
            model.Id = id;
            // Ignore model values
            model.UserId = User.Identity.GetUserId();
            model.ModifiedDate = DateTimeOffset.UtcNow;

            var result = await _sql.UpdateAsync(model);
            if (!result)
            {
                return BadRequest();
            }

            // Inform Worker there has been an update to a Destination
            await _messageBus.QueueSendAsync(new Update { Destination = model.Id, Url = model.Url });

            return Ok(model);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            // TODO: Notify Nodes that the destination doesn't exist anymore

#if !RELEASE_DEMO
            var result = await _sql.DeleteAsync(new Destination { Id = id });
            if (!result)
            {
                return BadRequest();
            }
#endif

            return Ok(id);
        }
    }
}