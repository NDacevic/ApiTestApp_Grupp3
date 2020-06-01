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
    public class TestResultsController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public TestResultsController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of TestResult objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestResult>>> GetTestResult()
        {
            return await _context.TestResult.ToListAsync();
        }

        /// <summary>
        /// GET Modified to return a list of TestResult for a specific testid. (There will always be multiple entries)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TestResult>> GetTestResult(int id)
        {
            var tests = await _context.TestResult.Where(tr => tr.TestId == id).Select(tr => tr).ToListAsync();
            List<TestResult> testResults = new List<TestResult>();

            foreach (var x in tests)
            {
                testResults.Add(x);
            }

            if (testResults.Count == 0)
            {
                return NotFound();
            }

            return Ok(testResults);
        }

        /// <summary>
        /// Edits a TestResult in the databse
        /// Currently unused
        /// </summary>
        /// <param name="id"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestResult(int id, TestResult testResult)
        {
            if (id != testResult.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(testResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestResultExists(id))
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

        // POST: api/TestResults
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TestResult>> PostTestResult(TestResult testResult)
        {
            _context.TestResult.Add(testResult);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TestResultExists(testResult.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTestResult", new { id = testResult.StudentId }, testResult);
        }

        /// <summary>
        /// Delete a TestResult with a specific Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TestResult>> DeleteTestResult(int id)
        {
            var testResult = await _context.TestResult.FindAsync(id);
            if (testResult == null)
            {
                return NotFound();
            }

            _context.TestResult.Remove(testResult);
            await _context.SaveChangesAsync();

            return testResult;
        }

        private bool TestResultExists(int id)
        {
            return _context.TestResult.Any(e => e.StudentId == id);
        }
    }
}
