using nbp_ask_data.DTOs;
using nbp_ask_data.Model;
using nbp_ask_data.PusherHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.DataProvider
{
    public class MessageDataProvider
    {
        #region Private
        private static ConversationWithMessagesDTO AddToNewConversation(CreateMessageDTO dto, Message message)
        {
            //Create new conversation
            Conversation conv = ConversationDataProvider.CreateConversation(dto.SenderId, dto.ReceiverUsername);
            if (conv == null)
                return null;

            //Add message to conversation
            message.SenderUsername = GetSenderUserName(conv, message.SenderId);
            conv.Messages.Add(message);

            //Insert conversation
            if (ConversationDataProvider.InsertConversation(conv) == null)
                return null;

            var retM = ConversationWithMessagesDTO.FromEntity(conv, message.SenderId);
            return retM;
        }
        private static ConversationWithMessagesDTO AddToExistingConversation(CreateMessageDTO dto, Message message)
        {
            Conversation existConv = ConversationDataProvider.GetConversation(dto.SenderId, dto.ReceiverUsername);

            if (existConv != null)
            {
                ConversationWithMessagesDTO ret = AddMessageToConversation(message, existConv);
                return ret;
            }

            return null;
        }

        private static string GetSenderUserName(Conversation c, string senderId)
        {
            if (c.UserId1 == senderId)
                return c.User1Username;
            else
                return c.User2Username;
        }
        private static ConversationWithMessagesDTO AddMessageToConversation(Message message, Conversation c)
        {
            message.SenderUsername = GetSenderUserName(c, message.SenderId);
            c.Messages.Add(message);
            c.Timestamp = message.Timestamp;

            if (ConversationDataProvider.UpdateConversation(c))
            {
                Conversation retConv = new Conversation(c);
                retConv.Messages.Add(message);
                return ConversationWithMessagesDTO.FromEntity(retConv, message.SenderId);
            }

            return null;
        }
        private static Message FindMessage(string convId, string messageId)
        {
            Conversation c = ConversationDataProvider.GetConversation(convId);
            if (c == null)
                return null;

            Message msg = c.Messages.Find(x => x.Id == messageId);
            return msg;
        }

        private static bool CheckMessage(CreateMessageDTO dto)
        {
            if (
                dto.Content == null ||
                dto.Content == String.Empty ||
                dto.ReceiverUsername == null ||
                dto.ReceiverUsername == String.Empty ||
                dto.SenderId == null ||
                dto.SenderId == String.Empty
                )
                return false;
            return true;
        }
        private static bool CheckMessageWithConversation(MessageWithConversationDTO dto)
        {
            if (
                dto.Content == null ||
                dto.Content == String.Empty ||
                dto.ConversationId == null ||
                dto.ConversationId == String.Empty ||
                dto.SenderId == null ||
                dto.SenderId == String.Empty
                )
                return false;
            return true;
        }

        #endregion

        public static async Task<ConversationWithMessagesDTO> AddMessageToConversation(MessageWithConversationDTO dto)
        {
            try
            {
                if (!CheckMessageWithConversation(dto))
                    return null;

                // Get existing conversation
                Conversation existConv = ConversationDataProvider.GetConversation(dto.ConversationId);
                if (existConv == null)
                    return null;

                //Add the message to the existing conversation
                Message message = MessageWithConversationDTO.FromDTO(dto);
                var res = AddMessageToConversation(message, existConv);
                if (res == null)
                    return null;

                //Publish the message
                await PusherProvider.PublishMessage(res);

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<ConversationWithMessagesDTO> CreateMessage(CreateMessageDTO dto)
        {
            try
            {
                if (!CheckMessage(dto))
                    return null;
                
                Message message = CreateMessageDTO.FromDTO(dto);
                
                //Ako konverzacija vec postoji
                ConversationWithMessagesDTO result = AddToExistingConversation(dto, message);

                if (result == null)
                {
                    //Ako konverzacija ne postoji
                    result = AddToNewConversation(dto, message);
                }

                if (result == null)
                    return null;

                await PusherProvider.PublishMessage(result);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static ReadMessageDTO ReadMessage(string convId, string messageId)
        {
            try
            {
                Message msg = FindMessage(convId, messageId);
                if (msg == null)
                    return null;
                return ReadMessageDTO.FromEntity(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public static bool DeleteMessage(string MessageId, string ConversationId)
        {
            try
            {
                Conversation c = ConversationDataProvider.GetConversation(ConversationId);
                if (c == null)
                    return false;

                Message msg = c.Messages.Find(x => x.Id == MessageId);
                c.Messages.Remove(msg);

                return ConversationDataProvider.UpdateConversation(c);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
