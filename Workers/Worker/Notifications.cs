//namespace Worker
//{
//    using System.Threading.Tasks;

//    using Microsoft.ServiceBus.Messaging;

//    using Models.ProtoBuf;

//    public class Notifications
//    {
//        public async Task NotificationReceived(BrokeredMessage m)
//        {
//            var update = m.GetBody<Update>(_xpsUpdate);

//            await _nodesUpdateTopic.SendAsync(new BrokeredMessage(new Update { Destination = update.Destination, Flight = update.Flight, Url = update.Url }, _xpsUpdate) { ContentType = typeof(Update).FullName });//.ConfigureAwait(false);

//            await m.CompleteAsync();//.ConfigureAwait(false);
//        }
//    }
//}