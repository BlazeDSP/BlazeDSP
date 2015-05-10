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
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Runtime.Caching;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Blaze.DSP.Library.Enumerators;
    using Blaze.DSP.Library.Helpers.Http;
    using Blaze.DSP.Library.Models.ProtoBuf;
    using Blaze.DSP.Library.Models.Update;
    using Blaze.DSP.Library.Singletons;

    using Ether.WeightedSelector;
    using Ether.WeightedSelector.Extensions;

    using ProtoBuf.Meta;
    using ProtoBuf.ServiceModel;

    using Utils;

    public class MvcApplication : HttpApplication
    {
        private readonly XmlProtoSerializer _xpsUpdate = new XmlProtoSerializer(RuntimeTypeModel.Default, typeof(Update));

        //private static readonly string FilePath = string.Format("{0}{1}", RoleEnvironment.GetLocalResource(LocalStorage.StorageName).RootPath, LocalStorage.LockName);

        //private readonly CloudBlockBlob _blockBlob;

        //public MvcApplication()
        //{
        //    var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Node.Storage.ConnectionString"));
        //    var blobClient = storageAccount.CreateCloudBlobClient();
        //    var container = blobClient.GetContainerReference("node");
        //    container.CreateIfNotExists();
        //    _blockBlob = container.GetBlockBlobReference(string.Format("offline.{0}.lock", RoleEnvironment.CurrentRoleInstance.Id.GetHash()));
        //}

        protected void Application_Start()
        {
            Trace.TraceInformation("Application_Start");

            // TODO: [Optimization]
            ServicePointManager.DefaultConnectionLimit = 12;

            // Initalize ProtoBuf
            ProtoBufConfig.Init();

            // Register Filters
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Register Routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Get Destination Data & Populate Cache
            try
            {
#if DEBUG
                SetupConfig.Init(HttpContext.Current);
#else
                SetupConfig.Init();
#endif
            }
            catch (Exception ex)
            {
                Trace.TraceError("SetupConfig Exception");

                // This will catch a misconfigured Worker Endpoint and inform of Faulted
                ServiceBusSingleton.Instance.SendCommandAsync(new Command { Type = Commands.Faulted }).Wait();
                return;
            }

            #region ServiceBusSingleton
            // Init Updates callback
            ServiceBusSingleton.Instance.OnUpdateMessage(message =>
            {
                try
                {
                    // Process message from subscription
                    var update = message.GetBody<Update>(_xpsUpdate);

                    if (update.Destination != null)
                    {
                        // TODO: Clean this
                        foreach (var flight in MemoryCache.Default)
                        {
                            var item = flight.Value as WeightedSelector<DestinationsUpdate>;
                            if (item == null)
                            {
                                continue;
                            }

                            foreach (var dest in item.ListByWeightAscending().Where(a => a.Value.Destination == update.Destination.ToString()))
                            {
                                item.Remove(dest);
                                item.Add(new DestinationsUpdate
                                {
                                    Flight = update.Flight != null ? update.Flight.ToString() : dest.Value.Flight,
                                    Destination = update.Destination.ToString(),
                                    Url = update.Url
                                }, 1);
                            }
                        }
                    }
                    else if (update.Flight != null)
                    {
                        var flights = DestinationsCache.Get(update.Flight);

                        // TODO: There currently is no way to select and update a single entry in WeightedSelector
                        foreach (var flight in flights.ListByWeightAscending().Where(flight => flight.Value.Destination == update.Destination.ToString()))
                        {
                            flights.Remove(flight);
                            flights.Add(new DestinationsUpdate
                            {
                                Flight = update.Flight.ToString(),
                                Destination = update.Destination != null ? update.Destination.ToString() : flight.Value.Destination,
                                Url = update.Url
                            }, 1);
                        }
                    }
                    // ReSharper disable RedundantIfElseBlock
                    else
                    {
                        // TODO: If the entry isn't in the cache then what? Request fresh data from Worker?
                    }
                    // ReSharper restore RedundantIfElseBlock

                    // Remove message from subscription
                    message.Complete();
                }
                catch (Exception)
                {
                    // TODO: Log error & dead letter message
                    // Indicates a problem, unlock message in subscription
                    message.Abandon();
                }
            });

            // Inform Worker its online (used later for monitoring Nodes)
            // TODO: Add Instance ID
            ServiceBusSingleton.Instance.SendCommandAsync(new Command { Type = Commands.Online }).Wait();
            #endregion


            
            // Set Node online
            //if (!_blockBlob.DeleteIfExists())
            //{
            //    throw new NotImplementedException();
            //}
            //File.Delete(FilePath);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Trace.TraceInformation("Application_End");

            ServiceBusSingleton.Instance.Close();
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            // Remove unnecessary HTTP headers
            ((HttpApplication)sender).Clean();
        }
    }
}