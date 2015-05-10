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

namespace Frontend.Controllers.Api.Management
{
    using System.Web.Http;

    using Blaze.DSP.Library.Constants;
    using Blaze.DSP.Library.Helpers.Assembly;
    using Blaze.DSP.Library.Models.Ember;

    using Microsoft.Azure;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class SystemsController : ApiController
    {
        public SystemsController()
        {
            ApplicationInformation.ExecutingAssembly = typeof(SystemsController).Assembly;
        }

        public IHttpActionResult Get()
        {
            return Ok(new[]
            {
                new Systems
                {
                    Id = 1,
                    BuildVersion = ApplicationInformation.ExecutingAssemblyVersion.ToString(),
                    BuildDate = ApplicationInformation.CompileDate,
                    BuildBeta = true,
                    FrontendInstanceId = RoleEnvironment.CurrentRoleInstance.Id,
                    NodeTrackingDomain = CloudConfigurationManager.GetSetting(SettingsFrontend.NodeTrafficManagerDomain),
#if RELEASE_DEMO
                    ServiceBusConnectionString = "HIDDEN"
#else
                    ServiceBusConnectionString = CloudConfigurationManager.GetSetting(ServiceBusConnectionStrings.WorkerServiceBusConnection)
#endif
                }
            });
        }

        public IHttpActionResult Get(int id)
        {
            return Ok(new Systems
            {
                Id = id,
                BuildVersion = ApplicationInformation.ExecutingAssemblyVersion.ToString(),
                BuildDate = ApplicationInformation.CompileDate,
                BuildBeta = true,
                FrontendInstanceId = RoleEnvironment.CurrentRoleInstance.Id,
                NodeTrackingDomain = CloudConfigurationManager.GetSetting(SettingsFrontend.NodeTrafficManagerDomain),
#if RELEASE_DEMO
                ServiceBusConnectionString = "HIDDEN"
#else
                ServiceBusConnectionString = CloudConfigurationManager.GetSetting(ServiceBusConnectionStrings.WorkerServiceBusConnection)
#endif
            });
        }
    }
}