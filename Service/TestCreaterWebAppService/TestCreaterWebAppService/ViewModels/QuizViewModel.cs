using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCreaterWebAppService.ViewModels
{
    public class QuizViewModel
    {
        public QuizViewModel()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }

        public int Type { get; set; }
        public int Flags { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public int ViewCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class QuestionViewModel
    {
        public QuestionViewModel()
        {
        }

        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }

        public int Type { get; set; }
        public int Flags { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]        
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class AnswerViewModel
    {
        public AnswerViewModel()
        {
        }

        public int Id { get; set; }
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }

        public int Type { get; set; }
        public int Flags { get; set; }
        public int Value { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class ResultViewModel
    {
        public ResultViewModel()
        {
        }

        public int Id { get; set; }
        public int QuizId { get; set; }
        
        public string Text { get; set; }
        public string Notes { get; set; }

        public string Score { get; set; }

        public int Type { get; set; }
        public int Flags { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}
