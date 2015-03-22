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

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

using AutoMapper;

using Library.Interfaces.SimpleInjector;

namespace Frontend.Controllers.Api
{
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
            var events = await _sql.QueryAsync<Library.Models.Database.Event>("SELECT * FROM Events");

            return Ok(Mapper.Map<IEnumerable<Library.Models.Database.Event>, IEnumerable<Library.Models.Ember.Event>>(events));
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            var evnt = await _sql.ReadAsync<Library.Models.Database.Event>(id);

            return Ok(Mapper.Map<Library.Models.Database.Event, Library.Models.Ember.Event>(evnt));
        }

        public async Task<IHttpActionResult> Get(string flight)
        {
            var events = await _sql.QueryAsync<Library.Models.Database.Event>("SELECT TOP(@Limit) * FROM Events WHERE Flight = @Flight ORDER BY Time DESC", new { Flight = flight, Limit = 10 });

            return Ok(Mapper.Map<IEnumerable<Library.Models.Database.Event>, IEnumerable<Library.Models.Ember.Event>>(events));
        }
    }
}