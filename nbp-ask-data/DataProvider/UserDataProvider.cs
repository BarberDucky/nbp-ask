using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_ask_data.DTOs;
using nbp_ask_data.Model;

namespace nbp_ask_data.DataProvider
{
    public class UserDataProvider
    {
        public static String CreateUser(UserDTO userDto)
        {
            try
            {
                var database = DataLayer.Client.GetDatabase("ask-db");
                var collection = database.GetCollection<User>("users");
                User newUser = UserDTO.FromDTO(userDto);
                String newId = Guid.NewGuid().ToString();
                newUser.Id = newId;
                collection.InsertOne(newUser);
                return newId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
