using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public int ClassId { get; set; }

        public List <Test> Tests { get; set; } //Lista med Tests, ska den stå här?

        // public IList<StudentQuestionAnswer> StudentQuestionAnswer { get; set; } --> Mellantabell

        // public IList<TestResult> TestResult { get; set; } --> Mellantabell




    }
}
