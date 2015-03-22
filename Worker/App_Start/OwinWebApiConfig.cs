using System;
using System.Diagnostics;

using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Worker
{
    public static class OwinWebApiConfig
    {
        public static void Init(out IDisposable api)
        {
            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Endpoint"];
            var baseUri = String.Format("{0}://{1}", endpoint.Protocol, endpoint.IPEndpoint);
            Trace.TraceInformation(String.Format("Starting OWIN at {0}", baseUri), "Information");
            api = WebApp.Start<Startup>(new StartOptions(baseUri));
        }
    }
}