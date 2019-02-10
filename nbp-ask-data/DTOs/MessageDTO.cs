using nbp_ask_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.DTOs
{
    public class CreateMessageDTO
    {
        public String Content { get; set; }
        public String SenderId { get; set; }
        public String ReceiverUsername { get; set; }

        public static Message FromDTO(CreateMessageDTO dto)
        {
            return new Message()
            {
                Content = dto.Content,
                SenderId = dto.SenderId
            };
        }
    }

    public class MessageWithConversationDTO
    {
        public String Content { get; set; }
        public String SenderId { get; set; }
        //public String ReceiverId { get; set; }
        public String ConversationId { get; set; }

        public static Message FromDTO(MessageWithConversationDTO dto)
        {
            return new Message()
            {
                Content = dto.Content,
                SenderId = dto.SenderId
            };
        }
    }

    public class ReadMessageDTO
    {
        public String Id { get; set; }
        public String Content { get; set; }
        public DateTime Timestamp { get; set; }
        public String SenderId { get; set; }
        public String SenderUsername { get; set; }

        public static ReadMessageDTO FromEntity(Message m)
        {
            return new ReadMessageDTO()
            {
                Id = m.Id,
                Content = m.Content,
                Timestamp = m.Timestamp.ToLocalTime(),
                SenderId = m.SenderId,
                SenderUsername = m.SenderUsername
            };
        }

        public static List<ReadMessageDTO> FromEntityList(List<Message> messages)
        {
            List<ReadMessageDTO> list = new List<ReadMessageDTO>();
            if (messages != null)
            {
                foreach (var c in messages)
                {
                    list.Add(ReadMessageDTO.FromEntity(c));
                }
            }

            return list;
        }
    }
}
