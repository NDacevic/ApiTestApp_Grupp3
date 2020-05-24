using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTestApp_Grupp3.Data;
using ApiTestApp_Grupp3.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace ApiTestApp_Grupp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public TestsController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        // GET: api/Tests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetTest()
        {
            var result = await _context.Test.Include(t => t.TestQuestion)
                .ThenInclude(tq => tq.Question)
                .Select(t => new { t.TestId, t.Grade, t.Course.CourseName, t.MaxPoints, t.TestDuration, t.IsActive, t.IsGraded, t.StartDate, questions = t.TestQuestion.Select(tq => tq.Question)})
                .ToListAsync();
            return Ok(result);
        }

        // GET: api/Tests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            var test = await _context.Test.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            return test;
        }

        // PUT: api/Tests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTest(int id, [FromBody] JsonPatchDocument<Test> jsonPatchTest)
        {
            Test updateTest = await _context.Test.FirstOrDefaultAsync(x => x.TestId == id);

            if (updateTest == null)
                return NotFound();

            jsonPatchTest.ApplyTo(updateTest, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(updateTest))
                return BadRequest(ModelState);

            _context.Update(updateTest);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Tests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Test>> PostTest(Test test)
        {

            _context.Database.BeginTransaction();
            test.Course = await _context.Course.Where(x => x.CourseName == test.CourseName).Select(x => x).FirstOrDefaultAsync();

            _context.Test.Add(test);
            await _context.SaveChangesAsync();

            foreach(Question tq in test.Questions)
            {
                TestQuestion tempTQ = new TestQuestion();
                tempTQ.TestId = test.TestId;
                tempTQ.QuestionId = tq.QuestionId;
                _context.TestQuestion.Add(tempTQ);
            }
                await _context.SaveChangesAsync();

            _context.Database.CommitTransaction();

            return Ok();
            //return CreatedAtAction("GetTest", new { id = test.TestId }, test);
        }

        // DELETE: api/Tests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Test>> DeleteTest(int id)
        {
            var test = await _context.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            _context.Test.Remove(test);
            await _context.SaveChangesAsync();

            return test;
        }

        private bool TestExists(int id)
        {
            return _context.Test.Any(e => e.TestId == id);
        }
    }
}
