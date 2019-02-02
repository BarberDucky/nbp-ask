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

        public static ConversationWithMessagesDTO AddMessageToConversation(MessageWithConversationDTO dto)
        {
            try
            {
                if (!CheckMessageWithConversation(dto))
                    return null;
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

        public static ConversationWithMessagesDTO CreateMessage(CreateMessageDTO dto)
        {
            try
            {

                if (!CheckMessage(dto))
                    return null;
                
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

                return ConversationWithMessagesDTO.FromEntity(conv, message.SenderId);
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
