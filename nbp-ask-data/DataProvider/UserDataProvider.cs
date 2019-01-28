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
        private static bool CheckDTO(UserDTO userDTO)
        {
            if (userDTO.Password == null ||
                userDTO.Username == null ||
                userDTO.Password == "" ||
                userDTO.Username == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static String CreateUser(UserDTO userDTO)
        {
            try
            {
                if (!CheckDTO(userDTO))
                {
                    return null;
                }

                var collection = DataLayer.Database.GetCollection<User>("users");

                //check if user exists
                var filter = Builders<User>.Filter.Eq("Username", userDTO.Username);
                User fetchedUser = collection.Find<User>(filter).FirstOrDefault<User>();
                if (fetchedUser != null)
                {
                    return null;
                }

                User newUser = UserDTO.FromDTO(userDTO);
                String newId = Guid.NewGuid().ToString();
                newUser.Id = newId;
                newUser.Questions = new List<string>();
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

        public static List<UserDTO> ReadAllUsers()
        {
            try
            {
                var collection = DataLayer.Database.GetCollection<User>("users");
                List<User> fetchedUsers = collection.Find<User>(_ => true).ToList<User>();
                return UserDTO.FromEntityList(fetchedUsers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static UserDTO LoginUser(UserDTO userDTO)
        {
            try
            {
                if (!CheckDTO(userDTO))
                {
                    return null;
                }

                var filter = Builders<User>.Filter.Eq("Username", userDTO.Username);
                var collection = DataLayer.Database.GetCollection<User>("users");
                User fetchedUser = collection.FindSync<User>(filter).First<User>();
                if (fetchedUser.Password == userDTO.Password)
                {
                    return UserDTO.FromEntity(fetchedUser);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static UserDTO UpdateUser(UserDTO userDTO, String userId)
        {

            try
            {
                if (!CheckDTO(userDTO))
                {
                    return null;
                }

                var idFilter = Builders<User>.Filter.Eq("Id", userId);
                var existsFilter = Builders<User>.Filter.Eq("Username", userDTO.Username);

                var collection = DataLayer.Database.GetCollection<User>("users");

                //check if user exists
                User fetchedUser = collection.Find<User>(existsFilter).FirstOrDefault<User>();
                if (fetchedUser != null && fetchedUser.Id != userId)
                {
                    return null;
                }

                User updatedUser = UserDTO.FromDTO(userDTO);
                updatedUser.Id = userId;
                collection.FindOneAndReplace<User>(idFilter, updatedUser);

                return UserDTO.FromEntity(updatedUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
