﻿using System;
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

        // GET: api/TestResults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestResult>>> GetTestResult()
        {
            return await _context.TestResult.ToListAsync();
        }

        // GET: api/TestResults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestResult>> GetTestResult(int id)
        {
            var testResult = await _context.TestResult.FindAsync(id);

            if (testResult == null)
            {
                return NotFound();
            }

            return testResult;
        }

        // PUT: api/TestResults/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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

        // DELETE: api/TestResults/5
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