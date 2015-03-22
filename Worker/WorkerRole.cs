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

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Dapper.Contrib.Extensions;

using Library.Const;
using Library.Converters;
using Library.Models.ProtoBuf;

using Microsoft.AspNet.SignalR;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

using ProtoBuf.ServiceModel;

namespace Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private IHubContext _eventHubContext;

        private IDisposable _api;

        private TopicClient _nodesUpdateTopic;

        private QueueClient _frontendNotificationsQueue;

        private XmlProtoSerializer _xpsEvent;
        private XmlProtoSerializer _xpsUpdate;
        private XmlProtoSerializer _xpsCommand;

        // TODO: Clean this up
        private List<QueueClient> _nodeEventsQueues = new List<QueueClient>();

        private readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);

        // DB Conection String
        private readonly string _conn = CloudConfigurationManager.GetSetting(DatabaseStrings.DatabaseConnectionStringName);

        public override void Run()
        {
            Trace.WriteLine("Running Worker Instance");

            // Process Events
            foreach (var client in _nodeEventsQueues)
            {
                client.OnMessageAsync(EventReceived, new OnMessageOptions { MaxConcurrentCalls = 5, AutoComplete = false });
            }

            _frontendNotificationsQueue.OnMessageAsync(NotificationReceived, new OnMessageOptions { MaxConcurrentCalls = 5, AutoComplete = false });

            // Block & Wait
            _completedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            Trace.Listeners.Add(new DiagnosticMonitorTraceListener());

            Trace.WriteLine("Starting Worker Instance");

            // TODO: [Optimization]
            ServicePointManager.DefaultConnectionLimit = 12;

            // Role Configuration Change Events
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            RoleEnvironment.Changed += RoleEnvironmentChanged;

            // ProtoBuf
            ProtoBufConfig.Init(out _xpsEvent, out _xpsUpdate, out _xpsCommand);

            // OWIN WebAPI
            OwinWebApiConfig.Init(out _api);
            
            // Service Bus
            // TODO: Refactor to pull from DB
            var sbconnect = new[]
            {
                ServiceBusConnectionStrings.NodeServiceBusConnection1,
                ServiceBusConnectionStrings.NodeServiceBusConnection2,
                ServiceBusConnectionStrings.NodeServiceBusConnection3
            };
            SerialBusConfig.Init(sbconnect, ref _nodeEventsQueues, out _nodesUpdateTopic, out _frontendNotificationsQueue);

            // SignalR
            SignalRServiceBusConfig.Init(out _eventHubContext);

            // GoGoGadget
            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.WriteLine("Stopping Worker Instance");

            if (_api != null)
            {
                _api.Dispose();
            }

            // Close the connections to Service Bus
            foreach (var client in _nodeEventsQueues)
            {
                client.Close();
            }
            if (_nodesUpdateTopic != null)
            {
                _nodesUpdateTopic.Close();
            }
            if (_frontendNotificationsQueue != null)
            {
                _frontendNotificationsQueue.Close();
            }

            // Release blocked thread
            _completedEvent.Set();

            base.OnStop();
        }

        private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            Trace.TraceInformation("RoleEnvironmentChanging Event Fired");

            var changes = e.Changes
                           .OfType<RoleEnvironmentConfigurationSettingChange>()
                           .ToList();

            if (!changes.Any())
            {
                return;
            }

            e.Cancel = true; // Instruct server to reboot on change (if not canceled below)

            // TODO: Implement (for now just repboot server)
            //// Service Bus Connection Strings Updated
            //if (changes.Any(c => c.ConfigurationSettingName == ServiceBus.FrontendServiceBusConnectionStringName))
            //{
            //    e.Cancel = false; // Cancel reboot of server
            //}
            //if (changes.Any(c => c.ConfigurationSettingName == ServiceBus.WorkerServiceBusConnectionStringName))
            //{
            //    e.Cancel = false; // Cancel reboot of server
            //}
        }

        private static void RoleEnvironmentChanged(object sender, RoleEnvironmentChangedEventArgs e)
        {
            Trace.TraceInformation("RoleEnvironmentChanged Event Fired");

            // TODO: Implement
            //var changes = e.Changes
            //               .OfType<RoleEnvironmentConfigurationSettingChange>()
            //               .ToList();

            //// Service Bus Connection Strings Updated
            //if (changes.Any(c => c.ConfigurationSettingName == ServiceBus.FrontendServiceBusConnectionStringName))
            //{
            //    // TODO: Close Service Bus connection and reopen with new connection string
            //}
            //if (changes.Any(c => c.ConfigurationSettingName == ServiceBus.WorkerServiceBusConnectionStringName))
            //{
            //    // TODO: Close Service Bus connection and reopen with new connection string
            //}
        }

        private async Task NotificationReceived(BrokeredMessage m)
        {
            var update = m.GetBody<Update>(_xpsUpdate);

            await _nodesUpdateTopic.SendAsync(new BrokeredMessage(new Update { Destination = update.Destination, Flight = update.Flight, Url = update.Url }, _xpsUpdate)
            {
                ContentType = typeof(Update).FullName
            });

            await m.CompleteAsync();
        }

        private async Task EventReceived(BrokeredMessage m)
        {
            Trace.WriteLine("Message Event Received");

            var shouldAbandon = false;

            try
            {
                // TODO: Use reflection instead
                switch (m.ContentType)
                {
                    case "Library.Models.ProtoBuf.Event":
                    {
                        // Deserialize Message
                        var msg = m.GetBody<Event>(_xpsEvent);
                        await ProcessEvent(msg);
                        break;
                    }
                    case "Library.Models.ProtoBuf.Command":
                    {
                        // Deserialize Message
                        var msg = m.GetBody<Command>(_xpsCommand);
                        await ProcessCommand(msg);
                        break;
                    }
                }

                await m.CompleteAsync();
            }
            catch
            {
                Trace.TraceError("Message Event Threw");

                // NOTE: Can't await inside a catch... yet
                shouldAbandon = true;

                // TODO: Handle message processing specific exceptions
            }

            if (shouldAbandon)
            {
                await m.AbandonAsync();
            }
        }

        // TODO: There is no sanitization on message data.
        private async Task ProcessEvent(Event evnt)
        {
            int result;

            // Database
            // TODO: Refactor to own method.
            // TODO: Automapper.
            // TODO: SQL connection shouldn't be initialized this way.
            using (var sql = new SqlConnection(_conn))
            {
                await sql.OpenAsync();

                // TODO: Sanitize this input (the default '0' for null isn't good)
                // TODO: Should handle bad campaign or flight ids
                // TODO: Parse methods will throw on error, convert to TryParse.
                // TODO: Events should be isolated into it's own DB
                result = await sql.InsertAsync(new Library.Models.Database.Event
                {
                    Time = evnt.Time.FromUnixTimeMilliseconds(),
                    UserId = Guid.Parse(evnt.UserId),
                    UserAgent = evnt.UserAgent,
                    UserHostAddress = (evnt.UserHostAddress == null ? null : IPAddress.Parse(evnt.UserHostAddress).GetAddressBytes()),
                    UserProxyAddress = (evnt.UserProxyAddress == null ? null : IPAddress.Parse(evnt.UserProxyAddress).GetAddressBytes()),
                    Flight = (string.IsNullOrWhiteSpace(evnt.Flight) ? 0 : int.Parse(evnt.Flight)),
                    Destination = (string.IsNullOrWhiteSpace(evnt.Destination) ? 0 : int.Parse(evnt.Destination))
                });

                sql.Close();
            }

            if (result < 1)
            {
                // TODO: Log error and raise hell
                throw new NotImplementedException();
            }

            // SignalR
            // TODO: [Optimization] If there are no clients connected to SignalR, don't push event message.
            // TODO: Refactor to own method.
            // TODO: Automapper.
            _eventHubContext.Clients
                            .All
                            .newEvent(new Library.Models.Ember.Event
                            {
                                Id = result,
                                Time = evnt.Time.FromUnixTimeMilliseconds(),
                                UserId = evnt.UserId,
                                UserAgent = evnt.UserAgent,
                                UserHostAddress = evnt.UserHostAddress,
                                UserProxyAddress = evnt.UserProxyAddress,
                                Flight = evnt.Flight,
                                Destination = evnt.Destination
                            });
        }

        private async Task ProcessCommand(Command command)
        {
            // TODO
            await Task.Delay(0);
        }
    }
}