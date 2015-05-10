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

namespace NodeMVC.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Blaze.DSP.Library.Constants;

    public class LanderController : Controller
    {
        public async Task<ActionResult> Index(int id)
        {
            // User ID
            var userId = Request.Cookies.Get(Cookies.UserId);
            if (userId == null)
            {
                // TODO: Log error and redirect to default offer
                throw new NotImplementedException();
            }

            // TODO: Log lander click through (like voluum)
            // TODO: Cookie should contain lander and destination information

            //return Content("Index");

            return Redirect("http://localhost:21987/purchase");
        }

        public async Task<ActionResult> Image()
        {
            // User ID
            var userId = Request.Cookies.Get(Cookies.UserId);
            if (userId == null)
            {
                // TODO: Log Error
                return new FilePathResult("~/Images/__t.gif", "image/gif");
            }

            // TODO: Log lander hit

            // NOTE: .NET v4.5.2 (currently not supported on Azure)
            // TODO: There needs to be a fallback if the node can't connect to the Service Bus (eg. MSMQ)
            //HostingEnvironment.QueueBackgroundWorkItem(ct => ServiceBusSingleton.Instance.SendEventAsync(new Event
            //{
            //    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            //    UserId = userId.Value,
            //    UserAgent = Request.UserAgent,
            //    UserHostAddress = Request.UserHostAddress,
            //    UserProxyAddress = Request.Headers.Get("X-FORWARDED-FOR"),
            //    Flight = flight,
            //    Destination = destination.Destination
            //}));
            // TODO: There needs to be a fallback if the node can't connect to the Service Bus (eg. MSMQ)
            //await ServiceBusSingleton.Instance.SendEventAsync(new Event
            //{
            //    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            //    UserId = userId.Value,
            //    UserAgent = Request.UserAgent,
            //    UserHostAddress = Request.UserHostAddress,
            //    UserProxyAddress = Request.Headers.Get("X-FORWARDED-FOR"),
            //    Flight = flight,
            //    Destination = destination.Destination
            //});

            return new FilePathResult("~/Images/__t.gif", "image/gif");
        }
    }
}