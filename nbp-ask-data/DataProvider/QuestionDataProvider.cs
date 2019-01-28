using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_ask_data.DTOs;
using nbp_ask_data.Model;
using MongoDB.Driver;
using MongoDB.Bson;

namespace nbp_ask_data.DataProvider
{
    public class QuestionDataProvider
    {
        private static bool CheckDTO(QuestionDTO questionDTO)
        {
            if (questionDTO.PosterId == null ||
                questionDTO.PosterId == "" ||
                questionDTO.Content == null ||
                questionDTO.Title == null ||
                questionDTO.Content == "" ||
                questionDTO.Title == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static QuestionDTO CreateQuestion(QuestionDTO questionDTO)
        {
            try
            {
                if (!CheckDTO(questionDTO))
                {
                    return null;
                }
                
                var collection = DataLayer.Database.GetCollection<Question>("questions");

                // check if poster user exists
                var filter = Builders<User>.Filter.Eq("Id", questionDTO.PosterId);
                var userCollection = DataLayer.Database.GetCollection<User>("users");
                User fetchedUser = userCollection.Find<User>(filter).FirstOrDefault<User>();
                if (fetchedUser == null)
                {
                    return null;
                }

                Question newQuestion = QuestionDTO.FromDTO(questionDTO);
                String newId = Guid.NewGuid().ToString();
                newQuestion.Id = newId;
                newQuestion.Answers = new List<Answer>();
                collection.InsertOne(newQuestion);

                //add question to user
                fetchedUser.Questions.Add(newId);
                userCollection.FindOneAndReplace<User>(filter, fetchedUser);

                return QuestionDTO.FromEntity(newQuestion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static QuestionDTO ReadQuestion(String questionId)
        {
            try
            {
                var filter = Builders<Question>.Filter.Eq("Id", questionId);
                var collection = DataLayer.Database.GetCollection<Question>("questions");
                Question fetchedQuestion = collection.FindSync<Question>(filter).First<Question>();
                return QuestionDTO.FromEntity(fetchedQuestion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<QuestionDTO> ReadAllQuestions()
        {
            try
            {
                var collection = DataLayer.Database.GetCollection<Question>("questions");
                List<Question> fetchedQuestions = collection.Find<Question>(_ => true).ToList<Question>();
                return QuestionDTO.FromEntityList(fetchedQuestions);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static QuestionDTO UpdateQuestion(QuestionDTO questionDTO, String questionId)
        {

            try
            {
                if (!CheckDTO(questionDTO))
                {
                    return null;
                }

                var idFilter = Builders<Question>.Filter.Eq("Id", questionId);

                var collection = DataLayer.Database.GetCollection<Question>("questions");

                var filter = Builders<Question>.Filter.Eq("Id", questionId);
                Question fetchedQuestion = collection.Find<Question>(filter).FirstOrDefault<Question>();


                Question updatedQuestion = QuestionDTO.FromDTO(questionDTO);
                updatedQuestion.Id = questionId;
                updatedQuestion.PosterId = fetchedQuestion.PosterId;
                updatedQuestion.TimeStamp = fetchedQuestion.TimeStamp;
                collection.FindOneAndReplace<QuestionDTO>(idFilter, updatedQuestion);

                return QuestionDTO.FromEntity(updatedQuestion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static bool DeleteQuestion(String questionId)
        {
            try
            {
                var idFilter = Builders<Question>.Filter.Eq("Id", questionId);

                var collection = DataLayer.Database.GetCollection<Question>("questions");
                Question fetchedQuestion = collection.Find<Question>(idFilter).FirstOrDefault<Question>();


                //remove question from user
                var userFilter = Builders<User>.Filter.Eq("Id", fetchedQuestion.PosterId);
                var userCollection = DataLayer.Database.GetCollection<User>("users");
                User fetchedUser = userCollection.Find<User>(userFilter).FirstOrDefault<User>();
                fetchedUser.Questions.Remove(questionId);
                userCollection.FindOneAndReplace<User>(userFilter, fetchedUser);


                collection.FindOneAndDelete<QuestionDTO>(idFilter);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        } 
    }
}
