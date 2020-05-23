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
    public class StudentQuestionAnswersController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public StudentQuestionAnswersController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        // GET: api/StudentQuestionAnswers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentQuestionAnswer>>> GetStudentQuestionAnswer()
        {
            return await _context.StudentQuestionAnswer.ToListAsync();
        }

        // GET: api/StudentQuestionAnswers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentQuestionAnswer>> GetStudentQuestionAnswer(int id)
        {
            var studentQuestionAnswer = await _context.StudentQuestionAnswer.FindAsync(id);

            if (studentQuestionAnswer == null)
            {
                return NotFound();
            }

            return studentQuestionAnswer;
        }

        // PUT: api/StudentQuestionAnswers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutStudentQuestionAnswer(List<StudentQuestionAnswer> updatedSqaList)
        {
            //Create a bool where we store if all the questions have been posted correctly.
            bool allQuestionsExist = true;
            
            //Go through all the questions and check if they are valid to be changed.
            foreach (var updatedSqa in updatedSqaList)
            {
                if (!_context.StudentQuestionAnswer.Any(sqa => sqa.QuestionId == updatedSqa.QuestionId && sqa.StudentId == updatedSqa.StudentId && sqa.TestId == updatedSqa.TestId))
                {
                    allQuestionsExist = false;
                }
            }
            
            //if one question is bad then abort the whole process
            if(!allQuestionsExist)
                return BadRequest();
            
            //if all is good then go ahead and update all the entries
            foreach( var updatedSqa in updatedSqaList)
                _context.Entry(updatedSqa).State = EntityState.Modified;

            //save and return
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/StudentQuestionAnswers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<StudentQuestionAnswer>> PostStudentQuestionAnswer(List<StudentQuestionAnswer> studentQuestionAnswers)
        {
            //Todo: Get correct Student, Test, Question objects using LINQ
            
            foreach (StudentQuestionAnswer studentQuestionAnswer in studentQuestionAnswers)
            {
                _context.StudentQuestionAnswer.Add(studentQuestionAnswer);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            { 
                return Conflict();
            }

            return CreatedAtAction("GetStudentQuestionAnswer", studentQuestionAnswers);
        }

        // DELETE: api/StudentQuestionAnswers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StudentQuestionAnswer>> DeleteStudentQuestionAnswer(int id)
        {
            var studentQuestionAnswer = await _context.StudentQuestionAnswer.FindAsync(id);
            if (studentQuestionAnswer == null)
            {
                return NotFound();
            }

            _context.StudentQuestionAnswer.Remove(studentQuestionAnswer);
            await _context.SaveChangesAsync();

            return studentQuestionAnswer;
        }

        private bool StudentQuestionAnswerExists(int id)
        {
            return _context.StudentQuestionAnswer.Any(e => e.StudentId == id);
        }
    }
}
