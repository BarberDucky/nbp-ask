using nbp_ask_data.DTOs;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.PusherHelper
{
    public class PusherProvider
    {
        private static String APP_ID = "705621";
        private static String APP_KEY = "416ab596eeb00b3e96e8";
        private static String APP_SECRET = "ff018934b5b89bdbd2b8";
        private static String APP_CLUSTER = "eu";

        private static Pusher pusher = null;

        public static Pusher GetPusher()
        {
            if (pusher == null)
            {
                var options = new PusherOptions
                {
                    Cluster = APP_CLUSTER
                };

                pusher = new Pusher(APP_ID, APP_KEY, APP_SECRET, options);
            }
            return pusher;
        }

        public static async Task PublishMessage(ConversationWithMessagesDTO message)
        {
            try
            {
                var events = new Event[2]{
                new Event(){ EventName = "message_event", Channel = message.ConversationId, Data = message },
                new Event(){ EventName = "notif_event", Channel = message.ConversationWithUserId, Data = message }
                };
                var result = await GetPusher().TriggerAsync(events);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



        }
    }
}
