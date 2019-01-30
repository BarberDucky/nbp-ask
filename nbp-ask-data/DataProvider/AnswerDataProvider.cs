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
    public class AnswerDataProvider
    {
        public static QuestionDTO CreateAnswer(AnswerDTO answerDTO)
        {
            try
            {
                // check if poster user exists
                var userFilter = Builders<User>.Filter.Eq("Id", answerDTO.PosterId);
                var userCollection = DataLayer.Database.GetCollection<User>("users");
                User fetchedUser = userCollection.Find<User>(userFilter).FirstOrDefault<User>();
                if (fetchedUser == null)
                {
                    return null;
                }

                // check if question exists
                var questionFilter = Builders<Question>.Filter.Eq("Id", answerDTO.QuestionId);
                var questionCollection = DataLayer.Database.GetCollection<Question>("questions");
                Question fetchedQuestion = questionCollection.Find<Question>(questionFilter).FirstOrDefault<Question>();
                if (fetchedQuestion == null)
                {
                    return null;
                }

                Answer newAnswer = AnswerDTO.FromDTO(answerDTO);
                newAnswer.Id = Guid.NewGuid().ToString();
                newAnswer.TimeStamp = DateTime.Now;

                if (newAnswer.IsTrue)
                {
                    fetchedQuestion.IsAnswered = true;
                    foreach (Answer answer in fetchedQuestion.Answers)
                    {
                        answer.IsTrue = false;
                    }
                }
                fetchedQuestion.Answers.Add(newAnswer);

                questionCollection.FindOneAndReplace<Question>(questionFilter, fetchedQuestion);

                return QuestionDTO.FromEntity(fetchedQuestion);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static List<AnswerDTO> ReadAllAnswers(String questionId)
        {
            try
            {
                // check if question exists
                var questionFilter = Builders<Question>.Filter.Eq("Id", questionId);
                var questionCollection = DataLayer.Database.GetCollection<Question>("questions");
                Question fetchedQuestion = questionCollection.Find<Question>(questionFilter).FirstOrDefault<Question>();
                if (fetchedQuestion == null)
                {
                    return null;
                }

                return AnswerDTO.FromEntityList(fetchedQuestion.Answers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static QuestionDTO UpdateAnswer(String questionId, String answerId, AnswerDTO answerDTO)
        {
            try
            {
                // check if question exists
                var questionFilter = Builders<Question>.Filter.Eq("Id", questionId);
                var questionCollection = DataLayer.Database.GetCollection<Question>("questions");
                Question fetchedQuestion = questionCollection.Find<Question>(questionFilter).FirstOrDefault<Question>();
                if (fetchedQuestion == null)
                {
                    return null;
                }

                Answer updatedAnswer = fetchedQuestion.Answers.Find(answer => answer.Id == answerId);
                if (updatedAnswer == null)
                {
                    return null;
                }

                updatedAnswer.IsTrue = answerDTO.IsTrue;
                updatedAnswer.Points = answerDTO.Points;
                updatedAnswer.Content = answerDTO.Content;

                if (updatedAnswer.IsTrue)
                {
                    fetchedQuestion.IsAnswered = true;
                    foreach (Answer answer in fetchedQuestion.Answers)
                    {
                        if (answer.Id != updatedAnswer.Id) answer.IsTrue = false;
                    }
                } else
                {
                    fetchedQuestion.IsAnswered = false;
                    foreach (Answer answer in fetchedQuestion.Answers)
                    {
                        if (answer.IsTrue) fetchedQuestion.IsAnswered = true;
                    }
                }

                questionCollection.FindOneAndReplace<Question>(questionFilter, fetchedQuestion);

                return QuestionDTO.FromEntity(fetchedQuestion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static QuestionDTO DeleteAnswer(String questionId, String answerId)
        {
            try
            {
                // check if question exists
                var questionFilter = Builders<Question>.Filter.Eq("Id", questionId);
                var questionCollection = DataLayer.Database.GetCollection<Question>("questions");
                Question fetchedQuestion = questionCollection.Find<Question>(questionFilter).FirstOrDefault<Question>();
                if (fetchedQuestion == null)
                {
                    return null;
                }

                Answer answerForDeletion = fetchedQuestion.Answers.Find(answer => answer.Id == answerId);
                if (answerForDeletion.IsTrue)
                {
                    fetchedQuestion.IsAnswered = false;
                }

                fetchedQuestion.Answers.Remove(answerForDeletion);
                questionCollection.FindOneAndReplace<Question>(questionFilter, fetchedQuestion);

                return QuestionDTO.FromEntity(fetchedQuestion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
