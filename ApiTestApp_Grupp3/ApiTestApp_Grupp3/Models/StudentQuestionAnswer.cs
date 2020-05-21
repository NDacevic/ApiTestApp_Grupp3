using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class StudentQuestionAnswer
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        [StringLength(3000)]
        public string Answer { get; set; } 
        
        [AllowNull]
        public bool? IsCorrect { get; set; }
    }
}
