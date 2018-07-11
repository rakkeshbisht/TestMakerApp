using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestCreaterWebAppService.ViewModels;
using Newtonsoft.Json;
using TestCreaterWebAppService.Data;
using Mapster;

namespace TestCreaterWebAppService.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        private ApplicationDbContext dbContext;

        public QuizController(ApplicationDbContext context)
        {
            dbContext = context;
        }
        // GET: api/quiz/LatestQuizList/10
        [HttpGet("LatestQuizList/{num}")]
        public IActionResult LatestQuizList(int num = 10)
        {
            var quizList = dbContext.Quizzes.OrderByDescending(x => x.CreatedDate).Take(num).ToArray();

            return new JsonResult(quizList.Adapt<QuizViewModel[]>());

        }

        // GET: api/quiz/ByTitle/1
        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var quizList = dbContext.Quizzes.OrderBy(x => x.Title).Take(num).ToArray();

            return new JsonResult(quizList.Adapt<QuizViewModel[]>());
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quiz = dbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();

            // handle requests asking for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                });
            }

            return new JsonResult(quiz.Adapt<QuizViewModel>());
        }

        [HttpPut]
        public IActionResult Put(QuizViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error) if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);

            // map the ViewModel to the Model
            var quiz = model.Adapt<Quiz>();
            
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;
            
            quiz.UserId = dbContext.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id;
            
            dbContext.Quizzes.Add(quiz);            
            dbContext.SaveChanges();

            return new JsonResult(quiz.Adapt<QuizViewModel>());
        }

        [HttpPost]
        public IActionResult Post(QuizViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);

            // retrieve the quiz to edit
            var quiz = dbContext.Quizzes.Where(q => q.Id == model.Id).FirstOrDefault();
            
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", model.Id)
                });
            }
            
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            // properties set from server-side
            quiz.LastModifiedDate = quiz.CreatedDate;
            
            dbContext.SaveChanges();

            // return the updated Quiz to the client.
            return new JsonResult(quiz.Adapt<QuizViewModel>());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {            
            var quiz = dbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();

            // handle requests asking for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                });
            }            
            dbContext.Quizzes.Remove(quiz);
            
            dbContext.SaveChanges();
          
            return new NoContentResult();
        }
    }
}
