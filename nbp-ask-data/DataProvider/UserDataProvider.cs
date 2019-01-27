using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_ask_data.DTOs;
using nbp_ask_data.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace nbp_ask_data.DataProvider
{
    public class UserDataProvider
    {
        public static String CreateUser(UserDTO userDto)
        {
            try
            { 
                var collection = DataLayer.Database.GetCollection<User>("users");
                User newUser = UserDTO.FromDTO(userDto);
                String newId = Guid.NewGuid().ToString();
                newUser.Id = newId;
                newUser.Questions = new List<string>();
                newUser.Answers = new List<string>();
                collection.InsertOne(newUser);
                return newId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static UserDTO ReadUser(String userId)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq("Id", userId);
                var collection = DataLayer.Database.GetCollection<User>("users");
                User fetchedUser = collection.FindSync<User>(filter).First<User>();
                return UserDTO.FromEntity(fetchedUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static UserDTO UpdateUser(UserDTO userDTO, String userId)
        {
            var filter = Builders<User>.Filter.Eq("Id", userId);

            var collection = DataLayer.Database.GetCollection<User>("users");

            User updatedUser = UserDTO.FromDTO(userDTO);
            updatedUser.Id = userId;
            collection.FindOneAndReplace<User>(filter, updatedUser);

            return UserDTO.FromEntity(updatedUser);
        }
    }
}
