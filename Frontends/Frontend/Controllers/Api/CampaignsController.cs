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
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using Blaze.DSP.Library.Interfaces.SimpleInjector;
    using Blaze.DSP.Library.Models.Database;

    using Microsoft.AspNet.Identity;

    // TODO: Refactor UserID
    public class CampaignsController : ApiController
    {
        private readonly IDatabase _sql;

        public CampaignsController(IDatabase sql)
        {
            _sql = sql;
        }

        public async Task<IHttpActionResult> Get()
        {
            var result = await _sql.QueryAsync<Campaign>("SELECT * FROM Campaigns WHERE UserId = @User", new
            {
                User = HttpContext.Current.User.Identity.GetUserId()
            });

            return Ok(result);
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            var result = await _sql.QueryAsync<Campaign>("SELECT TOP 1 * FROM Campaigns WHERE UserId = @User AND Id = @Id", new
            {
                User = HttpContext.Current.User.Identity.GetUserId(),
                Id = id
            });

            return Ok(result.SingleOrDefault());
        }

        public async Task<IHttpActionResult> Post([FromBody] Campaign model)
        {
            if (!ModelState.IsValid)
            {
                Validate(model);
                return BadRequest(ModelState);
            }

            // Ignore model values
            model.UserId = HttpContext.Current.User.Identity.GetUserId();
            model.AddedDate = DateTimeOffset.UtcNow;
            model.ModifiedDate = null;

            var result = await _sql.CreateAsync(model);
            if (result < 1)
            {
                return BadRequest();
            }

            return Ok(model);
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody] Campaign model)
        {
            if (!ModelState.IsValid)
            {
                Validate(model);
                return BadRequest(ModelState);
            }

            // TODO: Fix permissions problems (anyone can modify or delete)

            // Set ID because Ember doesn't send it
            model.Id = id;
            // Ignore model values
            model.UserId = HttpContext.Current.User.Identity.GetUserId();
            model.ModifiedDate = DateTimeOffset.UtcNow;

            var result = await _sql.UpdateAsync(model);
            if (!result)
            {
                return BadRequest();
            }

            return Ok(model);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            // TODO: Fix permissions problems (anyone can modify or delete)
            var result = await _sql.DeleteAsync(new Campaign { Id = id });
            if (!result)
            {
                return BadRequest();
            }

            return Ok(id);
        }
    }
}