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

namespace Frontend.Controllers.Api
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    using AutoMapper;

    using Blaze.DSP.Library.Interfaces.SimpleInjector;

    using DatabaseEvent = Blaze.DSP.Library.Models.Database.Event;
    using EmberEvent = Blaze.DSP.Library.Models.Ember.Event;

    /// <summary>
    ///     Controller is read only
    /// </summary>
    public class EventsController : ApiController
    {
        private readonly IDatabase _sql;

        public EventsController(IDatabase sql)
        {
            _sql = sql;
        }

        public async Task<IHttpActionResult> Get()
        {
            // TODO: This should only return data the user is authorized to access.
            var events = await _sql.QueryAsync<DatabaseEvent>("SELECT * FROM Events");

            var map = Mapper.Map<IEnumerable<DatabaseEvent>, IEnumerable<EmberEvent>>(events);

            return Ok(map);
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            // TODO: This should only return data the user is authorized to access.
            var evnt = await _sql.ReadAsync<DatabaseEvent>(id);

            if (evnt == null)
            {
                return BadRequest("Flight ID Empty");
            }

            var map = Mapper.Map<DatabaseEvent, EmberEvent>(evnt);

            return Ok(map);
        }

        public async Task<IHttpActionResult> Get(string flight)
        {
            if (string.IsNullOrWhiteSpace(flight))
            {
                return BadRequest("Flight ID Empty");
            }

            // TODO: This should only return data the user is authorized to access.
            //var events = await _sql.QueryAsync<Library.Models.Database.Event>("SELECT TOP(@Limit) * FROM Events WHERE Flight = @Flight ORDER BY Time DESC", new { Flight = flight, Limit = 10 });
            var events = await _sql.QueryAsync<DatabaseEvent>("SELECT * FROM Events WHERE Flight = @Flight ORDER BY Time DESC", new { Flight = flight });

            var map = Mapper.Map<IEnumerable<DatabaseEvent>, IEnumerable<EmberEvent>>(events);

            return Ok(map);
        }
    }
}