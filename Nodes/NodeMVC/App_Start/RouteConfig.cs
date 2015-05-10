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
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Tracker", "T/{action}/{id}", new
            {
                controller = "Tracker",
                action = "Index",
                id = UrlParameter.Optional
            });

            // Enabled in Web.config
            routes.MapRoute("LanderGif", "L/t.gif", new
            {
                controller = "Lander",
                action = "Image"
            });

            routes.MapRoute("Lander", "L/{id}", new
            {
                controller = "Lander",
                action = "Index",
                id = UrlParameter.Optional
            });

#if DEBUG || RELEASE_DEMO
            routes.MapRoute("Information", "Information/{action}/{id}", new
            {
                controller = "Information",
                action = "Index",
                id = UrlParameter.Optional
            });

            routes.MapRoute("UnloadAppDomain", "Unload", new
            {
                controller = "UnloadAppDomain",
                action = "Index"
            });
#endif

            // Catch All Route
            routes.MapRoute("Default", "{*url}", new
            {
                controller = "Home",
                action = "Index"
            });
        }
    }
}