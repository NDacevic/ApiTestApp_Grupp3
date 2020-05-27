using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTestApp_Grupp3.Data;
using ApiTestApp_Grupp3.Models;
using Newtonsoft.Json;

namespace ApiTestApp_Grupp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FullStudentsTestsQuestionsController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public FullStudentsTestsQuestionsController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        // GET: api/FullStudentsTestsQuestions
        [HttpGet]
        public async Task<ActionResult> GetFullStudentsTestsQuestions()
        {
            List<Student> studentList = new List<Student>();

            studentList = await _context.Student.Select(x => x).ToListAsync();

            foreach (var student in studentList)
            {
                await GetTestsForStudent(student);
            }

            foreach (var student in studentList)
            {
                foreach (var test in student.Tests)
                {
                    await GetQuestionsForTest(student, test);
                }
            }

            return Ok(studentList);
        }

        // GET: api/FullStudentsTestsQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetStudentTestsQuestions(int id)
        {
            Student student = await _context.Student.FirstOrDefaultAsync(x => x.StudentId == id);

            if (student == null)
                return NotFound();

            await GetTestsForStudent(student);

            foreach (var test in student.Tests)
            {
                await GetQuestionsForTest(student, test);
            }

            return Ok(student);
        }

        /// <summary>
        /// Find the tests associated with a given student. Checks at the same time so the tests added are unique.
        /// </summary>
        /// <param name="student"></param>
        private async Task GetTestsForStudent(Student student)
        {
            //Create a list for storing of the id's
            List<int> testIdList = new List<int>();

            //Find all the test id's for a student and keep only unique id's
            testIdList = await _context.StudentQuestionAnswer.Where(x => x.StudentId == student.StudentId).Select(x => x.TestId).ToListAsync();
            testIdList = testIdList.Select(x => x).Distinct().ToList();

            //initialize the testlist and add all the tests
            student.Tests = new List<Test>();
            foreach (var testId in testIdList)
            {
                var tempTest = await FindTestandAppendCourse(testId);

                //Before adding the test we make a deep copy by serializing and deserializing
                string jsonString = JsonConvert.SerializeObject(tempTest);
                student.Tests.Add(JsonConvert.DeserializeObject<Test>(jsonString));
            }
        }

        /// <summary>
        /// Gets questions for a chosen student and test
        /// </summary>
        /// <param name="student"></param>
        /// <param name="test"></param>
        private async Task GetQuestionsForTest(Student student, Test test)
        {
            //compile a list of all the question id's for a specific student and test.
            var questionIdList = await _context.StudentQuestionAnswer.Where(x => x.StudentId == student.StudentId && x.TestId == test.TestId).Select(x => x.QuestionId).ToListAsync();
            
            //initialize the list of questions for a test.
            test.Questions = new List<Question>();

            //go through the id's and append question objects to a test
            foreach (var q in questionIdList)
            {
                var tempQuestion = await _context.Question.Where(x => x.QuestionId == q).Select(x => x).FirstOrDefaultAsync();

                //Append the students answer to the question object
                tempQuestion.QuestionAnswer = await _context.StudentQuestionAnswer.Where(x =>
                    x.StudentId == student.StudentId &&
                    x.TestId == test.TestId &&
                    x.QuestionId == tempQuestion.QuestionId)
                    .Select(qa => new StudentQuestionAnswer
                    {
                        StudentId = qa.StudentId,
                        TestId = qa.TestId,
                        QuestionId = qa.QuestionId,
                        Answer = qa.Answer,
                        IsCorrect = qa.IsCorrect
                    })
                    .FirstOrDefaultAsync();

                //make a deep copy through serializing and deserializing before adding to the question list
                var jsonString = JsonConvert.SerializeObject(tempQuestion);
                test.Questions.Add(JsonConvert.DeserializeObject<Question>(jsonString));
            }
        }

        /// <summary>
        /// Takes an ID and finds the test. Populates the test with course information. also
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        private async Task<Test> FindTestandAppendCourse(int testId)
        {
            //Find a test with the course object included
            var test = await _context.Test.Include(x => x.Course).Where(test => test.TestId == testId).Select(test => test).FirstOrDefaultAsync();
            //Take the course name and append it to the CourseName property in the test.
            //This makes it slightly easier parsing the information on the client side
            test.CourseName = test.Course.CourseName;

            return test;
        }
       
    }
}
