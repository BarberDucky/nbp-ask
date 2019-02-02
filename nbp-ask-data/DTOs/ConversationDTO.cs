using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_ask_data.Model;

namespace nbp_ask_data.DTOs
{
    public class CreateConversationDTO
    {
        public String UserId1 { get; set; }
        public String UserId2 { get; set; }

        public static Conversation FromDTO(CreateConversationDTO dto)
        {
            return new Conversation()
            {
                UserId1 = dto.UserId1,
                UserId2 = dto.UserId2
            };
        }
    }

    public class ReadConversationDTO
    {
        public String ConversationId { get; set; }
        public String ConversationWithUsername { get; set; }
        public String ConversationWithUserId { get; set; }

        public static ReadConversationDTO FromEntity(Conversation c, string userId)
        {
            ReadConversationDTO dto = new ReadConversationDTO()
            {
                ConversationId = c.Id
            };

            if (c.UserId1 == userId)
            {
                dto.ConversationWithUsername = c.User2Username;
                dto.ConversationWithUserId = c.UserId2;
            }
            else
            {
                dto.ConversationWithUsername = c.User1Username;
                dto.ConversationWithUserId = c.UserId1;
            }

            return dto;
        }

        public static List<ReadConversationDTO> FromEntityList(List<Conversation> conv, string userId)
        {
            List<ReadConversationDTO> list = new List<ReadConversationDTO>();
            if (conv != null)
            {
                foreach (var c in conv)
                {
                    list.Add(ReadConversationDTO.FromEntity(c,userId));
                }
            }

            return list;
        }
    }

    public class ConversationWithMessagesDTO
    {
        public String ConversationId { get; set; }
        public String ConversationWithUsername { get; set; }
        public String ConversationWithUserId { get; set; }

        public List<ReadMessageDTO> Messages { get; set; }

        public static ConversationWithMessagesDTO FromEntity(Conversation c, string userId)
        {
            ConversationWithMessagesDTO dto = new ConversationWithMessagesDTO()
            {
                ConversationId = c.Id,
                Messages = ReadMessageDTO.FromEntityList(c.Messages)
            };

            if (c.UserId1 == userId)
            {
                dto.ConversationWithUsername = c.User2Username;
                dto.ConversationWithUserId = c.UserId2;
            }
            else
            {
                dto.ConversationWithUsername = c.User1Username;
                dto.ConversationWithUserId = c.UserId1;
            }
            
            return dto;
        }
    }
}
