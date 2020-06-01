using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTestApp_Grupp3.Data;
using ApiTestApp_Grupp3.Models;

namespace ApiTestApp_Grupp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public QuestionsController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a list of all questions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestion()
        {
            return await _context.Question.ToListAsync();
        }

        /// <summary>
        /// Modified Get a list of questions that are all connected to the same course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpGet("{course}")] //Get all questions on the given coursename
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestion(string course)
        {
            List<Question> tempList = new List<Question>();
            int courseId=0;
            var q =  _context.Course.Where(i =>i.CourseName.Contains(course));
            foreach (Course f in q)
            {
                courseId = f.CourseId;
            }
            var studentList = _context.Question.Where(s => s.Course.CourseId == courseId).ToList();
                foreach(Question courseName in studentList)
            {
                courseName.CourseName = course;
            }
            return studentList;
        }

        /// <summary>
        /// Edit a question
        /// Currently unused
        /// </summary>
        /// <param name="id"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.QuestionId)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        /// <summary>
        /// Posts a question to the database. Before saving it appends the course name to the question object
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            question.Course = await  _context.Course.Where(x => x.CourseName == question.CourseName).Select(x => x).FirstOrDefaultAsync();
            _context.Question.Add(question);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Delete a question with a specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Question>> DeleteQuestion(int id)
        {
            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Question.Remove(question);
            await _context.SaveChangesAsync();

            return question;
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}
