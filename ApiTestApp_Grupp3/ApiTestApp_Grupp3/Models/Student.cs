using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(250)]
        public string password { get; set; }
        public int ClassId { get; set; }

        [NotMapped]
        public List <Test> Tests { get; set; } //Lista med Tests

        public List<StudentQuestionAnswer> StudentQuestionAnswer { get; set; } // --> Mellantabell

        public List<TestResult> TestResult { get; set; } //--> Mellantabell
    }
}
