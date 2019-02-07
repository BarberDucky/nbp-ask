using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_ask_data.Model;

namespace nbp_ask_data.DTOs
{
    public class AnswerDTO
    {
        public String Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public String Content { get; set; }
        public int Points { get; set; }
        public bool IsTrue { get; set; }
        public String PosterId { get; set; }
        public String PosterName { get; set; }
        public String QuestionId { get; set; }

        public static Answer FromDTO(AnswerDTO dto)
        {
            return new Answer()
            {
                Id = dto.Id,
                TimeStamp = dto.TimeStamp,
                Content = dto.Content,
                Points = dto.Points,
                PosterId = dto.PosterId,
                PosterName = dto.PosterName,
                IsTrue = dto.IsTrue,
                QuestionId = dto.QuestionId
            };
        }

        public static AnswerDTO FromEntity(Answer question)
        {
            return new AnswerDTO()
            {
                Id = question.Id,
                TimeStamp = question.TimeStamp,
                Content = question.Content,
                Points = question.Points,
                IsTrue = question.IsTrue,
                PosterId = question.PosterId,
                PosterName = question.PosterName,
                QuestionId = question.QuestionId
            };
        }

        public static List<AnswerDTO> FromEntityList(List<Answer> entityList)
        {
            List<AnswerDTO> dtoList = new List<AnswerDTO>();
            foreach (Answer entity in entityList)
            {
                dtoList.Add(AnswerDTO.FromEntity(entity));
            }
            return dtoList;
        }

        public static List<Answer> FromDTOList(List<AnswerDTO> dtoList)
        {
            List<Answer> entityList = new List<Answer>();
            foreach (AnswerDTO dto in dtoList)
            {
                entityList.Add(AnswerDTO.FromDTO(dto));
            }
            return entityList;
        }
    }
}
