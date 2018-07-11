using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using TestCreaterWebAppService.ViewModels;
using TestCreaterWebAppService.Data;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        #region Private Fields
        private ApplicationDbContext dbContext;
        #endregion

        #region Constructor
        public AnswerController(ApplicationDbContext context)
        {            
            dbContext = context;
        }
        #endregion

        #region RESTful conventions methods
        /// <summary>
        /// Retrieves the Answer with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Answer</param>
        /// <returns>the Answer with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = dbContext.Answers.Where(i => i.Id == id).FirstOrDefault();
            
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }

            return new JsonResult(answer.Adapt<AnswerViewModel>());
        }

        /// <summary>
        /// Adds a new Answer to the Database
        /// </summary>
        /// <param name="model">The AnswerViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
         
            var answer = model.Adapt<Answer>();           
            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;
            
            dbContext.Answers.Add(answer);            
            dbContext.SaveChanges();

            return new JsonResult(answer.Adapt<AnswerViewModel>());
        }

        /// <summary>
        /// Edit the Answer with the given {id}
        /// </summary>
        /// <param name="model">The AnswerViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {            
            if (model == null) return new StatusCodeResult(500);
                     
            var answer = dbContext.Answers.Where(q => q.Id == model.Id).FirstOrDefault();
            
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", model.Id)
                });
            }
     
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.Notes = model.Notes;            
            answer.LastModifiedDate = answer.CreatedDate;
            
            dbContext.SaveChanges();
            
            return new JsonResult(answer.Adapt<AnswerViewModel>());
        }

        /// <summary>
        /// Deletes the Answer with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Answer</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {            
            var answer = dbContext.Answers.Where(i => i.Id == id).FirstOrDefault();

            // handle requests asking for non-existing answers
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }
         
            dbContext.Answers.Remove(answer);            
            dbContext.SaveChanges();
            
            return new NoContentResult();
        }
        #endregion

        // GET api/answer/all
        [HttpGet("List/{questionId}")]
        public IActionResult List(int questionId)
        {
            var answers = dbContext.Answers.Where(q => q.QuestionId == questionId).ToArray();
            return new JsonResult(answers.Adapt<AnswerViewModel[]>());
        }
    }
}
