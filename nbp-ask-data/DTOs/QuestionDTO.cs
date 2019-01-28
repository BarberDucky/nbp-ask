using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_ask_data.Model;

namespace nbp_ask_data.DTOs
{
    public class QuestionDTO
    {
        public String Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public int Points { get; set; }
        public bool IsAnswered { get; set; }
        public List<String> Tags { get; set; }
        public String PosterId { get; set; }
        public List<Answer> Answers { get; set; }

        public static Question FromDTO(QuestionDTO dto)
        {
            return new Question()
            {
                Id = dto.Id,
                TimeStamp = dto.TimeStamp,
                Title = dto.Title,
                Content = dto.Title,
                Points = dto.Points,
                IsAnswered = dto.IsAnswered,
                Tags = dto.Tags,
                PosterId = dto.PosterId,
                Answers = dto.Answers != null ? dto.Answers : new List<Answer>()
            };
        }

        public static QuestionDTO FromEntity(Question question)
        {
            return new QuestionDTO()
            {
                Id = question.Id,
                TimeStamp = question.TimeStamp,
                Title = question.Title,
                Content = question.Title,
                Points = question.Points,
                IsAnswered = question.IsAnswered,
                Tags = question.Tags,
                PosterId = question.PosterId,
                Answers = question.Answers,
            };
        }

        public static List<QuestionDTO> FromEntityList(List<Question> entityList)
        {
            List<QuestionDTO> dtoList = new List<QuestionDTO>();
            foreach (Question entity in entityList)
            {
                dtoList.Add(QuestionDTO.FromEntity(entity));
            }
            return dtoList;
        }
    }
}
