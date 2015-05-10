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

namespace Worker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Blaze.DSP.Library.Constants;

    using Microsoft.Azure;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    public static class SerialBusConfig
    {
        public static void Init(IEnumerable<string> nodes, ref List<QueueClient> queueClients, out TopicClient topicClient, out QueueClient frontendClient)
        {
            // Node Events Queues
            foreach (var connectionString in nodes.Select(CloudConfigurationManager.GetSetting))
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new NotImplementedException("Connection String can not be blank");
                }

                var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
                if (!namespaceManager.QueueExists(ServiceBusPathNames.ServiceBusEventQueue))
                {
                    namespaceManager.CreateQueue(ServiceBusPathNames.ServiceBusEventQueue);
                }

                queueClients.Add(QueueClient.CreateFromConnectionString(connectionString, ServiceBusPathNames.ServiceBusEventQueue));
            }

            var workerConn = CloudConfigurationManager.GetSetting(ServiceBusConnectionStrings.WorkerServiceBusConnection);
            var workerNamespace = NamespaceManager.CreateFromConnectionString(workerConn);

            if (string.IsNullOrWhiteSpace(workerConn))
            {
                throw new NotImplementedException("Connection String can not be blank");
            }

            // Update Node Topic
            if (!workerNamespace.TopicExists(ServiceBusPathNames.ServiceBusUpdateTopic))
            {
                var td = new TopicDescription(ServiceBusPathNames.ServiceBusUpdateTopic)
                {
                    DefaultMessageTimeToLive = new TimeSpan(0, 1, 0) // TTL 1 minute
                };

                workerNamespace.CreateTopic(td);
            }

            topicClient = TopicClient.CreateFromConnectionString(workerConn, ServiceBusPathNames.ServiceBusUpdateTopic);

            // Frontend Notification Queue
            var nm = NamespaceManager.CreateFromConnectionString(workerConn);
            if (!nm.QueueExists(ServiceBusPathNames.ServiceBusUpdateQueue))
            {
                nm.CreateQueue(ServiceBusPathNames.ServiceBusUpdateQueue);
            }

            frontendClient = QueueClient.CreateFromConnectionString(workerConn, ServiceBusPathNames.ServiceBusUpdateQueue);
        }
    }
}