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

using System.Web.Http;

using Library.Const;
using Library.Models.Ember;

using Microsoft.WindowsAzure;

namespace Frontend.Controllers.Api
{
    public class SettingsController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(new[]
            {
                new Settings
                {
                    Id = 1,
                    Version = typeof(HomeController).Assembly.GetName().Version.ToString(),

                    NodeTrackingDomain = CloudConfigurationManager.GetSetting(SettingsFrontend.NodeTrafficManagerDomain),
                    
                    // TODO: Fix this
                    ServiceBusConnectionString = "HIDDEN"//CloudConfigurationManager.GetSetting(SettingsFrontend.ServiceBusConnectionString)
                }
            });
        }

        public IHttpActionResult Get(int id)
        {
            return Ok(new Settings
            {
                Id = id,
                Version = typeof(HomeController).Assembly.GetName().Version.ToString(),

                NodeTrackingDomain = CloudConfigurationManager.GetSetting(SettingsFrontend.NodeTrafficManagerDomain),
                
                // TODO: Fix this
                ServiceBusConnectionString = "HIDDEN"//CloudConfigurationManager.GetSetting(SettingsFrontend.ServiceBusConnectionString)
            });
        }
    }
}