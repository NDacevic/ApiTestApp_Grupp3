using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class Test
    {
       public int TestId { get; set; }
       public int Grade { get; set; }
       public int MaxPoints { get; set; }
       public DateTime StartDate { get; set; }
       public int TestDuration { get; set; }
       public bool IsActive { get; set; }
       public bool IsGraded { get; set; }

       [NotMapped]
       public List<Question> questions { get; set; } //--> Lista med frågor.

       public Course Course { get; set; } // ---> FK till Course

        //public List<TestQuestion> TestQuestion { get; set; } //--> Mellantabell

        public List<StudentQuestionAnswer> StudentQuestionAnswer { get; set; } //--> Mellantabell

        //public List<TestResult> TestResult { get; set; } // --> Mellantabell




    }
}
