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
    public class ResultController : Controller
    {
        #region Private Fields
        private ApplicationDbContext dbContext;
        #endregion

        #region Constructor
        public ResultController(ApplicationDbContext context)
        {            
            dbContext = context;
        }
        #endregion

        #region RESTful conventions methods
        /// <summary>
        /// Retrieves the Result with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Result</param>
        /// <returns>the Result with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = dbContext.Results.Where(i => i.Id == id).FirstOrDefault();

            // handle requests asking for non-existing results
            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", id)
                });
            }

            return new JsonResult(result.Adapt<ResultViewModel>());
        }

        /// <summary>
        /// Adds a new Result to the Database
        /// </summary>
        /// <param name="model">The ResultViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel model)
        {            
            if (model == null) return new StatusCodeResult(500);
            
            var result = model.Adapt<Result>();
            
            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;
            
            dbContext.Results.Add(result);            
            dbContext.SaveChanges();
            
            return new JsonResult(result.Adapt<ResultViewModel>());
        }

        /// <summary>
        /// Edit the Result with the given {id}
        /// </summary>
        /// <param name="model">The ResultViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel model)
        {            
            if (model == null) return new StatusCodeResult(500);
         
            var result = dbContext.Results.Where(q => q.Id == model.Id).FirstOrDefault();
            
            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", model.Id)
                });
            }
           
            result.QuizId = model.QuizId;
            result.Text = model.Text;
            result.MinValue = model.MinValue;
            result.MaxValue = model.MaxValue;
            result.Notes = model.Notes;

            // properties set from server-side
            result.LastModifiedDate = result.CreatedDate;

            // persist the changes into the Database.
            dbContext.SaveChanges();
            
            return new JsonResult(result.Adapt<ResultViewModel>());
        }

        /// <summary>
        /// Deletes the Result with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Result</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {            
            var result = dbContext.Results.Where(i => i.Id == id).FirstOrDefault();
            
            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", id)
                });
            }
            
            dbContext.Results.Remove(result);            
            dbContext.SaveChanges();
            
            return new NoContentResult();
        }
        #endregion

        // GET api/result/all
        [HttpGet("list/{quizId}")]
        public IActionResult All(int quizId)
        {
            var results = dbContext.Results.Where(q => q.QuizId == quizId).ToArray();
            return new JsonResult(results.Adapt<ResultViewModel[]>());
        }
    }
}
