using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTestApp_Grupp3.Data;
using ApiTestApp_Grupp3.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;
using System.Net.Http.Headers;

namespace ApiTestApp_Grupp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public StudentsController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult> GetStudent()
        {
            List<Student> studentList = new List<Student>();
            List<Test> tempTestList = new List<Test>();
            List<int> testIdList = new List<int>();

            studentList = await _context.Student.Select(x => x).ToListAsync();

            foreach (var student in studentList)
            {
                testIdList = await _context.StudentQuestionAnswer.Where(x => x.StudentId == 1).Select(x => x.TestId).ToListAsync();
                testIdList = testIdList.Select(x => x).Distinct().ToList();
                
                student.Tests = new List<Test>();

                foreach (var testId in testIdList)
                {
                    var tempTest = _context.Test.Include(x => x.Course).Where(test => test.TestId == testId).Select(test => test).FirstOrDefault();
                    tempTest.CourseName = tempTest.Course.CourseName;

                    string jsonString = JsonConvert.SerializeObject(tempTest);
                    student.Tests.Add(JsonConvert.DeserializeObject<Test>(jsonString));
                }
            }

            foreach(var student in studentList)
            {
                foreach(var test in student.Tests)
                {
                    var questionList = await _context.StudentQuestionAnswer.Where(x => x.StudentId == student.StudentId && x.TestId == test.TestId).Select(x => x.QuestionId).ToListAsync();
                    test.Questions = new List<Question>();

                    foreach (var q in questionList)
                    {
                        var tempQuestion = await _context.Question.Where(x => x.QuestionId == q).Select(x => x).FirstOrDefaultAsync();

                        tempQuestion.QuestionAnswer = await _context.StudentQuestionAnswer.Where(x =>
                            x.StudentId == student.StudentId &&
                            x.TestId == test.TestId &&
                            x.QuestionId == tempQuestion.QuestionId)
                            .Select(qa => new StudentQuestionAnswer {
                                StudentId = qa.StudentId,
                                TestId = qa.TestId,
                                QuestionId = qa.QuestionId,
                                Answer = qa.Answer,
                                IsCorrect = qa.IsCorrect})
                            .FirstOrDefaultAsync();
                        
                        var jsonString = JsonConvert.SerializeObject(tempQuestion);
                        test.Questions.Add(JsonConvert.DeserializeObject<Question>(jsonString));
                    }
                }
            }

            return Ok(studentList);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            try
            {
                _context.Student.Add(student);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
