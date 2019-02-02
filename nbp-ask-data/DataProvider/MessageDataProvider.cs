using nbp_ask_data.DTOs;
using nbp_ask_data.Model;
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
        private static string GetSenderUserName(Conversation c, string senderId)
        {
            if (c.UserId1 == senderId)
                return c.User1Username;
            else
                return c.User2Username;
        }

        private static string AddMessageToConversation(Message message, Conversation c)
        {
            message.SenderUsername = GetSenderUserName(c, message.SenderId);
            c.Messages.Add(message);
            c.Timestamp = message.Timestamp;

            if (ConversationDataProvider.UpdateConversation(c))
                return message.Id;

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

        #endregion

        public static string AddMessageToConversation(MessageWithConversationDTO dto)
        {
            try
            {
                Conversation existConv = ConversationDataProvider.GetConversation(dto.ConversationId);
                if (existConv == null)
                    return null;

                Message message = MessageWithConversationDTO.FromDTO(dto);

                return AddMessageToConversation(message, existConv);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string CreateMessage(CreateMessageDTO dto)
        {
            try
            {
                // ako koverzacija vec postoji
                Message message = CreateMessageDTO.FromDTO(dto);

                Conversation existConv = ConversationDataProvider.GetConversation(dto.SenderId, dto.ReceiverUsername);

                if (existConv != null)
                {
                    return AddMessageToConversation(message, existConv);
                }

                //Ako konverzacija vec ne postoji

                Conversation conv = ConversationDataProvider.CreateConversation(dto.SenderId, dto.ReceiverUsername);

                if (conv == null)
                    return null;

                message.SenderUsername = GetSenderUserName(conv, message.SenderId);
                conv.Messages.Add(message);

                if (ConversationDataProvider.InsertConversation(conv) == null)
                    return null;

                return message.Id;
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
