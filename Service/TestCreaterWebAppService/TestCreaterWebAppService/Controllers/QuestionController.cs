using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCreaterWebAppService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestCreaterWebAppService.Data;
using Mapster;

namespace TestCreaterWebAppService.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        #region Private Fields
        private ApplicationDbContext dbContext;
        #endregion

        #region Constructor
        public QuestionController(ApplicationDbContext context)
        {            
            dbContext = context;
        }
        #endregion

        // GET: api/question/list/1
        [HttpGet("List/{quizId}")]
        public IActionResult List(int quizId)
        {
            var questions = dbContext.Questions.Where(q => q.QuizId == quizId).ToArray();
            return new JsonResult(questions.Adapt<QuestionViewModel[]>());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = dbContext.Questions.Where(i => i.Id == id).FirstOrDefault();
            
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }

            return new JsonResult(question.Adapt<QuestionViewModel>());
        }

        [HttpPut]
        public IActionResult Put([FromBody]QuestionViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
         
            var question = model.Adapt<Question>();
            
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;
                        
            dbContext.Questions.Add(question);            
            dbContext.SaveChanges();
            
            return new JsonResult(question.Adapt<QuestionViewModel>());
        }

        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel model)
        {            
            if (model == null) return new StatusCodeResult(500);
            
            var question = dbContext.Questions.Where(q => q.Id == model.Id).FirstOrDefault();
            
            if (question == null) return NotFound(new {
                Error = String.Format("Question ID {0} has not been found", model.Id)
            });
            
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;
            
            question.LastModifiedDate = question.CreatedDate;            
            dbContext.SaveChanges();

            return new JsonResult(question.Adapt<QuestionViewModel>());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
            var question = dbContext.Questions.Where(i => i.Id == id).FirstOrDefault();            
            if (question == null) return NotFound(new
            {
                Error = String.Format("Question ID {0} has not been found", id)
            });
            
            dbContext.Questions.Remove(question);            
            dbContext.SaveChanges();           
            
            return new NoContentResult();
        }
    }
}
