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
    using System.Linq;
    using System.Web.Http;

    using FiftyOne.Foundation.Mobile.Detection;
    using FiftyOne.Foundation.Mobile.Detection.Entities;

    public class FiftyOneDegreesController : ApiController
    {
        public IHttpActionResult Get()
        {
            //var prov = new Provider();

            //return Ok(prov.DataSet.Hardware.Profiles.Select(profile => profile.ProfileId));

            return Ok(WebProvider.ActiveProvider.DataSet.Hardware.Profiles.Select(p => new
            {
                p.ProfileId,
                Properties = p.Properties.Select(pp => new
                {
                    pp.Name,
                    pp.Description,
                    Values = pp.Values.ToString(),
                    pp.Category,
                    pp.Url
                }),
                Values = p.Values.Select(pp => new
                {
                    pp.Name,
                    pp.Description,
                    pp.Url
                }),
            }));

            //return Ok(WebProvider.ActiveProvider.DataSet.Properties.Select(p => new 
            //{
            //    p.Name,
            //    p.Description,
            //    Values = p.Values.ToString()
            //}));
        }
    }
}