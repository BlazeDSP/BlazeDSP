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

namespace Blaze.DSP.Library.IoC.MessageBus
{
    using System;
    using System.Threading.Tasks;

    using Constants;

    using Interfaces.SimpleInjector;

    using Microsoft.Azure;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    using ProtoBuf.Meta;
    using ProtoBuf.ServiceModel;

    public class ServiceBus : IMessageBus
    {
        private readonly QueueClient _queueClient;

        public ServiceBus()
        {
            // TODO: Harden connection
            var connectionString = CloudConfigurationManager.GetSetting(ServiceBusConnectionStrings.WorkerServiceBusConnection);
            
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new NotImplementedException("'connectionString' is NULL");
            }

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists(ServiceBusPathNames.ServiceBusUpdateQueue))
            {
                namespaceManager.CreateQueue(ServiceBusPathNames.ServiceBusUpdateQueue);
            }

            _queueClient = QueueClient.CreateFromConnectionString(connectionString, ServiceBusPathNames.ServiceBusUpdateQueue);
        }

        public void QueueSend<T>(T message)
        {
            QueueSendAsync<T>(message).Wait();
        }

        public Task QueueSendAsync<T>(T message)
        {
            // TODO: Serializer shouldn't be in here! (bad daniel, no scooby snacks for you).
            // TODO: ISerializer
            var xps = new XmlProtoSerializer(RuntimeTypeModel.Default, typeof(T));

            var bm = new BrokeredMessage(message, xps)
            {
                ContentType = typeof(T).FullName
            };

            return _queueClient.SendAsync(bm);
        }

        public void Close()
        {
            _queueClient.Close();
        }
    }
}