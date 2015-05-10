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

namespace NodeMVC
{
    // ReSharper disable RedundantUsingDirective
    using System;
    // ReSharper restore RedundantUsingDirective
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    // ReSharper disable once RedundantUsingDirective
    using System.Net;
    using System.Web;

    // ReSharper disable once RedundantUsingDirective
    using Blaze.DSP.Library.Constants;
    using Blaze.DSP.Library.Models.Update;

    // ReSharper disable once RedundantUsingDirective
    using Microsoft.Azure;

    using Newtonsoft.Json;

    using Utils;

    // TODO: Use LocalSQL/RavenDB (in memory) instead of memory cache. Will make lookups easier.
    public static class SetupConfig
    {
#if DEBUG
        public static void Init(HttpContext httpContext)
        {
            //throw new NotImplementedException();

            Trace.TraceInformation("-----------> REMOVE THIS !!!! DEBUG ONLY");

            //var data = File.ReadAllText(httpContext.Server.MapPath("~/App_Data/DestinationsData.json"));
            var data = "[{\"Flight\": \"1\",\"Destination\": \"1\",\"Url\": \"http://blazedsp.com\"},{\"Flight\": \"1\",\"Destination\": \"3\",\"Url\": \"http://bing.com\"},{\"Flight\": \"2\",\"Destination\": \"1\",\"Url\": \"http://blazedsp.com\"}]";

            var destinations = JsonConvert.DeserializeObject<IEnumerable<DestinationsUpdate>>(data).ToArray();

            foreach (var dest in destinations)
            {
                DestinationsCache.Set(dest.Flight, dest);
            }
        }
#else
        public static void Init()
        {
            var host = CloudConfigurationManager.GetSetting(Endpoint.AddressStringName);
            var auth = CloudConfigurationManager.GetSetting(Endpoint.AuthenticationStringName);

            using (var wc = new WebClient())
            {
                wc.Headers.Add(Endpoint.AuthenticationHeader, auth);

                var endpoint = string.Format("http://{0}/api/node", host);

                var data = wc.DownloadString(endpoint);

                var destinations = JsonConvert.DeserializeObject<IEnumerable<DestinationsUpdate>>(data).ToArray();

                if (!destinations.Any())
                {
                    throw new NotImplementedException("No destinations received from worker");
                }

                foreach (var dest in destinations)
                {
                    DestinationsCache.Set(dest.Flight, dest);
                }
            }
        }
#endif
    }
}