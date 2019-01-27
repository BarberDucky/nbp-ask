using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_ask_data.Model;

namespace nbp_ask_data.DTOs
{
    public class UserDTO
    {
        public String Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        //public List<Question> Questions { get; set; }
        //public List<Answer> Answers { get; set; }

        public static User FromDTO(UserDTO dto)
        {
            return new User()
            {
                Id = dto.Id,
                Username = dto.Username,
                Password = dto.Password,
                Questions = new List<Question>(),
                Answers = new List<Answer>()
            };
        }
    }
}
