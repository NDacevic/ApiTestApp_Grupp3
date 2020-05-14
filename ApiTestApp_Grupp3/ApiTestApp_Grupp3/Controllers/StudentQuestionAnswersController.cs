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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentQuestionAnswer(int id, StudentQuestionAnswer studentQuestionAnswer)
        {
            if (id != studentQuestionAnswer.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(studentQuestionAnswer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentQuestionAnswerExists(id))
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

        // POST: api/StudentQuestionAnswers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<StudentQuestionAnswer>> PostStudentQuestionAnswer(StudentQuestionAnswer studentQuestionAnswer)
        {
            _context.StudentQuestionAnswer.Add(studentQuestionAnswer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentQuestionAnswerExists(studentQuestionAnswer.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentQuestionAnswer", new { id = studentQuestionAnswer.StudentId }, studentQuestionAnswer);
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
