using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        [StringLength(50)]
        public string QuestionType { get; set; }
        [StringLength(3000)]
        public string QuestionText { get; set; }

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


        //This exists so that the API can do the work of finding the correct FK when a new question is posted
        //instead of having an extra class on the client side and downloading the Courses as objects
        [NotMapped]
        public string CourseName { get; set; } 

        [NotMapped]
        public StudentQuestionAnswer QuestionAnswer { get; set; } //Object used for returning the answer & correction information for a specific student and test to the client.

    }
}