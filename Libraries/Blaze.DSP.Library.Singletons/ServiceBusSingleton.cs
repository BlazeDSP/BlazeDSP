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

namespace Blaze.DSP.Library.Singletons
{
    using System;
    using System.Threading.Tasks;

    using Constants;

    using Microsoft.Azure;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    using Models.ProtoBuf;

    using ProtoBuf.Meta;
    using ProtoBuf.ServiceModel;

    public sealed class ServiceBusSingleton
    {
        private static volatile ServiceBusSingleton _instance;

        private static readonly object Locker = new object();

        private readonly QueueClient _queueClient;

        private readonly Random _rnd = new Random();

        private readonly SubscriptionClient _subscriptionClient;

        private readonly XmlProtoSerializer _xpsCommand = new XmlProtoSerializer(RuntimeTypeModel.Default, typeof(Command));
        private readonly XmlProtoSerializer _xpsEvent = new XmlProtoSerializer(RuntimeTypeModel.Default, typeof(Event));

        private ServiceBusSingleton()
        {
            var connectionString = CloudConfigurationManager.GetSetting(ServiceBusConnectionStrings.NodeServiceBusConnection);
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(ServiceBusPathNames.ServiceBusEventQueue))
            {
                namespaceManager.CreateQueue(ServiceBusPathNames.ServiceBusEventQueue);
            }
            _queueClient = QueueClient.CreateFromConnectionString(connectionString, ServiceBusPathNames.ServiceBusEventQueue);

            var subName = _rnd.Next().ToString(); //Guid.NewGuid().ToString();

            var conn = CloudConfigurationManager.GetSetting(ServiceBusConnectionStrings.WorkerServiceBusConnection);
            var ns = NamespaceManager.CreateFromConnectionString(conn);
            if (!ns.SubscriptionExists(ServiceBusPathNames.ServiceBusUpdateTopic, subName))
            {
                var sd = new SubscriptionDescription(ServiceBusPathNames.ServiceBusUpdateTopic, subName)
                {
                    AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
                };
                ns.CreateSubscription(sd);
            }
            _subscriptionClient = SubscriptionClient.CreateFromConnectionString(conn, ServiceBusPathNames.ServiceBusUpdateTopic, subName);
        }

        public static ServiceBusSingleton Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (Locker)
                {
                    if (_instance == null)
                    {
                        _instance = new ServiceBusSingleton();
                    }
                }

                return _instance;
            }
        }

        public Task SendEventAsync(Event evnt)
        {
            var msg = new BrokeredMessage(evnt, _xpsEvent)
            {
                // TODO: This could be better implemented
                ContentType = typeof(Event).FullName,
                // NOTE: For debugging
                Label = typeof(Event).FullName
            };
            return _queueClient.SendAsync(msg);
        }

        public Task SendCommandAsync(Command evnt)
        {
            var msg = new BrokeredMessage(evnt, _xpsCommand)
            {
                // TODO: This could be better implemented
                ContentType = typeof(Command).FullName,
                // NOTE: For debugging
                Label = typeof(Command).FullName
            };
            return _queueClient.SendAsync(msg);
        }

        public void OnUpdateMessage(Action<BrokeredMessage> callback)
        {
            _subscriptionClient.OnMessage(callback, new OnMessageOptions
            {
                AutoComplete = false,
                //AutoRenewTimeout = TimeSpan.FromSeconds(10),
                MaxConcurrentCalls = 5
            });
        }

        public void OnUpdateMessageAsync(Func<BrokeredMessage, Task> callback)
        {
            _subscriptionClient.OnMessageAsync(callback, new OnMessageOptions
            {
                AutoComplete = false,
                //AutoRenewTimeout = TimeSpan.FromMinutes(1),
                MaxConcurrentCalls = 5
            });
        }

        public void Close()
        {
            _queueClient.Close();
        }
    }
}