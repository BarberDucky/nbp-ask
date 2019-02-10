using nbp_ask_data.DataProvider;
using nbp_ask_data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nbp_ask_api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AnswersController : ApiController
    {
        

        [HttpGet]
        [Route("api/Questions/{questionId}/Answers")]
        public IEnumerable<AnswerDTO> GetAnswersOfQuestion(String questionId)
        {
            return AnswerDataProvider.ReadAllAnswers(questionId);
        }

       

        // POST: api/Answers
        public QuestionDTO Post([FromBody]AnswerDTO dto)
        {
            return AnswerDataProvider.CreateAnswer(dto);
        }

        [HttpPut]
        [Route("api/Questions/{questionId}/Answers/{answerId}")]
        public QuestionDTO Put(String questionId, String answerId, [FromBody]AnswerDTO answerDTO)
        {
            return AnswerDataProvider.UpdateAnswer(questionId, answerId, answerDTO);
        }

        [HttpDelete]
        [Route("api/Questions/{questionId}/Answers/{answerId}")]
        public QuestionDTO Delete(String questionId, String answerId)
        {
            return AnswerDataProvider.DeleteAnswer(questionId, answerId);
        }
    }
}
