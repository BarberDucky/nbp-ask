using MongoDB.Driver;
using nbp_ask_data.DTOs;
using nbp_ask_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data.DataProvider
{
    public class ConversationDataProvider
    {
        #region Private

        private static bool AddConversationToUser(Conversation conv)
        {
            try
            {
                var filter1 = Builders<User>.Filter.Eq("Id", conv.UserId1);
                var filter2 = Builders<User>.Filter.Eq("Id", conv.UserId2);
                var collection = DataLayer.Database.GetCollection<User>("users");
                User user1 = UserDataProvider.GetUserById(conv.UserId1);
                User user2 = UserDataProvider.GetUserById(conv.UserId2);

                if (user1 == null || user2 == null)
                    return false;

                UserConversation userConv1 = new UserConversation()
                {
                    Username = user2.Username,
                    ConversationId = conv.Id
                };
                user1.UserConversaions.Add(userConv1);
                UserDataProvider.UpdateUser(user1);

                UserConversation userConv2 = new UserConversation()
                {
                    Username = user1.Username,
                    ConversationId = conv.Id
                };
                user2.UserConversaions.Add(userConv2);
                UserDataProvider.UpdateUser(user2);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        private static bool RemoveConversationFromUser(Conversation conv)
        {
            try
            {
                var filter1 = Builders<User>.Filter.Eq("Id", conv.UserId1);
                var filter2 = Builders<User>.Filter.Eq("Id", conv.UserId2);
                var collection = DataLayer.Database.GetCollection<User>("users");
                User user1 = collection.FindSync<User>(filter1).First<User>();
                User user2 = collection.FindSync<User>(filter2).First<User>();

                if (user1 == null || user2 == null)
                    return false;

                UserConversation uc = user1.UserConversaions.Find(x => x.ConversationId == conv.Id);
                user1.UserConversaions.Remove(uc);
                collection.ReplaceOne<User>((x => x.Id == conv.UserId1), user1);

                uc = user2.UserConversaions.Find(x => x.ConversationId == conv.Id);
                user2.UserConversaions.Remove(uc);
                collection.ReplaceOne<User>((x => x.Id == conv.UserId2), user2);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static bool CheckConversation(User user1, User user2)
        {
            return user1.Id != user2.Id;
        }
        #endregion

        public static Conversation GetConversation(string convId)
        {
            try
            {
                var builder = Builders<Conversation>.Filter;
                var filter = builder.Eq("Id", convId);
                var collection = DataLayer.Database.GetCollection<Conversation>("conversations");
                Conversation conv = collection.FindSync<Conversation>(filter).First<Conversation>();

                return conv;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static Conversation GetConversation(string userId1, string user2username)
        {
            try
            {
                var builder = Builders<Conversation>.Filter;
                var filter = (builder.Eq("UserId1", userId1) & builder.Eq("User2Username", user2username))
                    | (builder.Eq("User1Username", user2username) & builder.Eq("UserId2", userId1));
                var collection = DataLayer.Database.GetCollection<Conversation>("conversations");
                Conversation conv = collection.FindSync<Conversation>(filter).First<Conversation>();

                return conv;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<ReadConversationDTO> GetConversationsWithUser(string userId)
        {
            try
            {
                var builder = Builders<Conversation>.Filter;
                var filter = builder.Eq("UserId1", userId) | builder.Eq("UserId2", userId);
                var collection = DataLayer.Database.GetCollection<Conversation>("conversations");
                List<Conversation> conv = collection.Find<Conversation>(filter).SortByDescending(x => x.Timestamp).ToList();

                return ReadConversationDTO.FromEntityList(conv, userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<ReadConversationDTO>();
            }
        }

        public static ConversationWithMessagesDTO ReadConversation(string convId, string userId)
        {
            try
            {
                Conversation conv = GetConversation(convId);
                if (conv == null)
                    return null;
                return ConversationWithMessagesDTO.FromEntity(conv, userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static Conversation CreateConversationFromDTO(CreateConversationDTO dto)
        {
            try
            {
                User fetchedUser = UserDataProvider.GetUserById(dto.UserId1);

                if (fetchedUser == null)
                    return null;

                fetchedUser = UserDataProvider.GetUserById(dto.UserId2); ;
                if (fetchedUser == null)
                    return null;

                Conversation c = CreateConversationDTO.FromDTO(dto);
                c.Id = Guid.NewGuid().ToString();

                return c;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static Conversation CreateConversation(string senderId, string receiverUsername)
        {
            try
            {
                User fetchedUser = UserDataProvider.GetUserById(senderId);

                if (fetchedUser == null)
                    return null;

                User fetchedUser2 = UserDataProvider.GetUserByUserName(receiverUsername); ;
                if (fetchedUser2 == null)
                    return null;

                if (!CheckConversation(fetchedUser, fetchedUser2))
                    return null;

                Conversation c = new Conversation()
                {
                    Id = Guid.NewGuid().ToString(),
                    User1Username = fetchedUser.Username,
                    User2Username = fetchedUser2.Username,
                    UserId1 = fetchedUser.Id,
                    UserId2 = fetchedUser2.Id
                };

                return c;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static String InsertConversation(Conversation conv)
        {
            try
            {
                if (AddConversationToUser(conv) == false)
                    return null;

                var collection = DataLayer.Database.GetCollection<Conversation>("conversations");
                collection.InsertOne(conv);
                return conv.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static bool UpdateConversation(Conversation conv)
        {
            try
            {
                var collection = DataLayer.Database.GetCollection<Conversation>("conversations");
                return collection.ReplaceOne<Conversation>((x => x.Id == conv.Id), conv).IsAcknowledged;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool DeleteConversation(string id)
        {
            try
            {
                var collection = DataLayer.Database.GetCollection<Conversation>("conversations");
                Conversation conv = collection.FindSync<Conversation>(c => c.Id == id).First<Conversation>();
                RemoveConversationFromUser(conv);
                return collection.DeleteOne<Conversation>(x => x.Id == id).IsAcknowledged;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
