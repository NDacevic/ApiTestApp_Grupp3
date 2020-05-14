using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        [StringLength(50)]
        public string QuestionType { get; set; }
        public string QuestionText { get; set; } //Inget tak på strängen här eftersom det beror på lärarens fråga

        [StringLength(50)]
        public string CorrectAnswer { get; set; }

        [StringLength(50)]
        public string IncorrectAnswer1 { get; set; }

        [StringLength(50)]
        public string IncorrectAnswer2 { get; set; }
        public int PointValue { get; set; }

        public Course Course { get; set; } //---> FK Course

        public List<TestQuestion> TestQuestion { get; set; } //--> Mellantabell

        public List<StudentQuestionAnswer> StudentQuestionAnswer { get; set; } //--> Mellantabell

    }
}
