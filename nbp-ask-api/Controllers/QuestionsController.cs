using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using nbp_ask_data.DataProvider;
using nbp_ask_data.DTOs;

namespace nbp_ask_api.Controllers
{
    public class QuestionsController : ApiController
    {
        // GET: api/Questions
        public IEnumerable<QuestionDTO> Get()
        {
            return QuestionDataProvider.ReadAllQuestions();
        }

        // GET: api/Questions/5
        public QuestionDTO Get(String id)
        {
            return QuestionDataProvider.ReadQuestion(id);
        }

        // POST: api/Questions
        public QuestionDTO Post([FromBody]QuestionDTO dto)
        {
            return QuestionDataProvider.CreateQuestion(dto);
        }

        // PUT: api/Questions/5
        public QuestionDTO Put(String id, [FromBody]QuestionDTO dto)
        {
            return QuestionDataProvider.UpdateQuestion(dto, id);
        }

        // DELETE: api/Questions/5
        public bool Delete(String id)
        {
            return QuestionDataProvider.DeleteQuestion(id);
        }
    }
}
