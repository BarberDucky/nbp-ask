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
        public List<String> Questions { get; set; }

        public static User FromDTO(UserDTO dto)
        {
            return new User()
            {
                Id = dto.Id,
                Username = dto.Username,
                Password = dto.Password,
                Questions = dto.Questions != null ? dto.Questions : new List<string>()
            };
        }

        public static UserDTO FromEntity(User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Questions = user.Questions
            };
        }

        public static List<UserDTO> FromEntityList(List<User> entityList)
        {
            List<UserDTO> dtoList = new List<UserDTO>();
            foreach (User entity in entityList)
            {
                dtoList.Add(UserDTO.FromEntity(entity));
            }
            return dtoList;
        }
    }
}
