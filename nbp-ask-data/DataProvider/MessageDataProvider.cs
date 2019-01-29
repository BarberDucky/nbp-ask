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
        public static string AddMessageToConversation(MessageWithConversationDTO dto)
        {
            try
            {
                Conversation existConv = ConversationDataProvider.GetConversation(dto.ConversationId);
                if (existConv == null)
                    return null;

                Message message = MessageWithConversationDTO.FromDTO(dto);
                message.Id = Guid.NewGuid().ToString();

                existConv.Messages.Add(message);
                existConv.Timestamp = message.Timestamp;

                if (ConversationDataProvider.UpdateConversation(existConv))
                    return message.Id;

                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string CreateMessage(MessageDTO dto)
        {
            try
            {
                Message message = MessageDTO.FromDTO(dto);
                message.Id = Guid.NewGuid().ToString();

                Conversation existConv = ConversationDataProvider.GetConversation(dto.SenderId, dto.ReceiverId);
                if (existConv != null)
                {
                    existConv.Messages.Add(message);
                    existConv.Timestamp = message.Timestamp;
                    if (ConversationDataProvider.UpdateConversation(existConv))
                        return message.Id;
                    return null;
                }

                //////////////////////////////////////////////////
                CreateConversationDTO cDTO = new CreateConversationDTO()
                {
                    UserId1 = dto.SenderId,
                    UserId2 = dto.ReceiverId
                };

                Conversation conv = ConversationDataProvider.CreateConversationFromDTO(cDTO);

                if (conv == null)
                    return null;


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
