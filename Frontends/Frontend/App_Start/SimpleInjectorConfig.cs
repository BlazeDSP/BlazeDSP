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
    using System.Web.Http;

    using Blaze.DSP.Library.Constants;
    using Blaze.DSP.Library.Interfaces.SimpleInjector;
    using Blaze.DSP.Library.IoC.Database;
    using Blaze.DSP.Library.IoC.MessageBus;

    using Microsoft.Azure;

    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;

    public static class SimpleInjectorConfig
    {
        public static void Init(HttpConfiguration config)
        {
            var container = new Container();

            container.Register<IMessageBus, ServiceBus>(Lifestyle.Singleton);

            // TODO: Check for nulls and log errors
            var conn = CloudConfigurationManager.GetSetting(DatabaseStrings.DatabaseConnectionStringName);
            container.RegisterWebApiRequest<IDatabase>(() => new AzureSqlDatabase(conn));

            container.RegisterWebApiControllers(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}