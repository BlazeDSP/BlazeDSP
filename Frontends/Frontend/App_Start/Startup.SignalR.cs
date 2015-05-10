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

namespace Frontend
{
    using Blaze.DSP.Library.Constants;
    // ReSharper disable once RedundantUsingDirective
    using Blaze.DSP.Library.SignalR;

    using Microsoft.AspNet.SignalR;
    // ReSharper disable once RedundantUsingDirective
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.Azure;

    using Owin;

    public partial class Startup
    {
        public static void ConfigureSignalR(IAppBuilder app)
        {
            // SignalR + Service Bus
            var connectionString = CloudConfigurationManager.GetSetting(ServiceBusConnectionStrings.SignalRServiceBusConnection);

            GlobalHost.DependencyResolver.UseServiceBus(connectionString, ServiceBusPathNames.SignalRServiceBus);

            // Custom UserId provider
            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());

#if !DEBUG
            // Minify hub javascript
            GlobalHost.DependencyResolver.Register(typeof(IJavaScriptMinifier), () => new HubMinify());
#endif

            // SignalR
#if DEBUG
            app.MapSignalR(new HubConfiguration
            {
                EnableDetailedErrors = true
            });
#else
            app.MapSignalR(new HubConfiguration());
#endif

            // Force Auth in SignalR
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}