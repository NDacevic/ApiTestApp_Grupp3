using System;
using System.Collections.Generic;
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

        
        public List<Question> questions { get; set; } //--> Lista med frågor. Ska den stå kvar här??

        public List<StudentQuestionAnswer> result { get; set; } // --> Lista med resultat

         public Course Course { get; set; } // ---> FK till Course

         public IList<TestQuestion> TestQuestion { get; set; } //--> Mellantabell

         public IList<StudentQuestionAnswer> StudentQuestionAnswer { get; set; } //--> Mellantabell

         public IList<TestResult> TestResult { get; set; } // --> Mellantabell





    }
}
