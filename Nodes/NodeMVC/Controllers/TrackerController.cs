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
    using System.Web;
    using System.Web.Mvc;

    using Blaze.DSP.Library.Constants;
    using Blaze.DSP.Library.Enumerators;
    using Blaze.DSP.Library.Helpers.Converters;
    using Blaze.DSP.Library.Models.ProtoBuf;
    using Blaze.DSP.Library.Singletons;

    using Utils;

    public class TrackerController : Controller
    {
        public async Task<ActionResult> Index()
        {
            // Flight
            var flight = Request.QueryString.Get(QueryStrings.FlightId);
            if (string.IsNullOrWhiteSpace(flight))
            {
                // TODO: Log the error and redirect to a default catch all address (value should be setup in Frontend config)
                throw new NotImplementedException("'flight' is NULL");
            }

            // User ID
            var userId = Request.Cookies.Get(Cookies.UserId);
            var uid = userId != null ? userId.Value : Guid.NewGuid().ToString();

            // Destination List
            var destinations = DestinationsCache.Get(flight);
            if (destinations == null)
            {
                // TODO: Log the error and request update for Flight ID
                throw new NotImplementedException("'destinations' is NULL");
            }
            // Select Destination (Weighted)
            var destination = destinations.Select();

            // NOTE: .NET v4.5.2 (currently not supported on Azure)
            // TODO: There needs to be a fallback if the node can't connect to the Service Bus (eg. MSMQ)
            //HostingEnvironment.QueueBackgroundWorkItem(ct => ServiceBusSingleton.Instance.SendEventAsync(new Event
            //{
            //    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            //    UserId = uid,
            //    UserAgent = Request.UserAgent,
            //    UserHostAddress = Request.UserHostAddress,
            //    UserProxyAddress = Request.Headers.Get("X-FORWARDED-FOR"),
            //    Flight = flight,
            //    Destination = destination.Destination
            //}));
            // TODO: There needs to be a fallback if the node can't connect to the Service Bus (eg. MSMQ)
            await ServiceBusSingleton.Instance.SendEventAsync(new Event
            {
                Type = EventType.AdvertClick,

                Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                
                UserId = uid,
                
                UserAgent = Request.UserAgent,
                UserLanguage = Request.Headers.Get("Accept-Language"),
                UserHostAddress = Request.UserHostAddress,
                UserProxyAddress = Request.Headers.Get("X-FORWARDED-FOR"),

                Referer = Request.UrlReferrer == null ? null : Request.UrlReferrer.AbsoluteUri,
                
                Flight = flight,
                Destination = destination.Destination
            }).ConfigureAwait(true);

            // TODO: Fix this for cross domain issues
            Response.AppendCookie(new HttpCookie(Cookies.UserId, uid)
            {
                //Domain = "",
                Expires = DateTime.UtcNow.AddYears(2)
            });

#if DEBUG || RELEASE_DEMO
            var debugMode = Request.QueryString.Get(QueryStrings.DebugMode);
            if (!string.IsNullOrWhiteSpace(debugMode))
            {
                return View(destination);
            }
#endif

            // Redirect to URL
            return Redirect(destination.Url);
        }
    }
}