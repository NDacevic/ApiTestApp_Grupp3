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
        /// <summary>
        /// returns all the courses from the Tests table.
        /// The questions attached to the test are also included
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetTest()
        {
            var result = await _context.Test.Include(t => t.TestQuestion)
                .ThenInclude(tq => tq.Question)
                .Select(t => new
                { 
                    t.TestId,
                    t.Grade,
                    t.Course.CourseName,
                    t.MaxPoints, t.TestDuration,
                    t.IsActive,
                    t.IsGraded,
                    t.StartDate,
                    questions = t.TestQuestion.Select(tq =>
                                                tq.Question)
                })
                .ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// returns the course of the specified id from the Tests table
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Patches a test with a specific id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonPatchTest"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTest(int id, [FromBody] JsonPatchDocument<Test> jsonPatchTest)
        {
            //find the test
            Test updateTest = await _context.Test.FirstOrDefaultAsync(x => x.TestId == id);

            if (updateTest == null)
                return NotFound();

            //apply the changes and append the modelstate that keeps track of the objects in entity framework
            jsonPatchTest.ApplyTo(updateTest, ModelState);

            //check if it's valid after changes are applied
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(updateTest))
                return BadRequest(ModelState);

            //update the table
            _context.Update(updateTest);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Posts to the Tests and TestQuestion tables
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Test>> PostTest(Test test)
        {

            _context.Database.BeginTransaction();
            test.Course = await _context.Course.Where(x => x.CourseName == test.CourseName).Select(x => x).FirstOrDefaultAsync();

            _context.Test.Add(test);
            await _context.SaveChangesAsync();

            //A list of questions is passed inside the Test object.
            //Go through these and add the Id of the question and test for writing to the TestQuestion table
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
        }

        /// <summary>
        /// Deletes a test with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
