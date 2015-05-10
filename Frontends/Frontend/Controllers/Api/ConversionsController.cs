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
    using System.Threading.Tasks;
    using System.Web.Http;

    using Blaze.DSP.Library.Interfaces.SimpleInjector;
    using Blaze.DSP.Library.Models.Database;

    /// <summary>
    ///     Controller is read only
    /// </summary>
    public class ConversionsController : ApiController
    {
        private readonly IDatabase _sql;

        public ConversionsController(IDatabase sql)
        {
            _sql = sql;
        }

        public async Task<IHttpActionResult> Get()
        {
            // TODO: This should only return data the user is authorized to access.
            return Ok(await _sql.QueryAsync<Conversion>("SELECT * FROM Conversions"));
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            // TODO: This should only return data the user is authorized to access.
            return Ok(await _sql.ReadAsync<Conversion>(id));
        }
    }
}