using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string IncorrectAnswer1 { get; set; }
        public string IncorrectAnswer2 { get; set; }
        public int PointValue { get; set; }

        // public Course Course {get;set} ---> FK Course

        // public IList<TestQuestion> TestQuestion { get; set; } --> Mellantabell

        // public IList<StudentQuestionAnswer> StudentQuestionAnswer { get; set; } --> Mellantabell

    }
}
