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
using Microsoft.AspNetCore.JsonPatch;

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

        /// <summary>
        /// Get a list of students
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetStudent()
        {
            var studentList = await _context.Student.ToListAsync();

            return Ok(studentList);
        }

        /// <summary>
        /// Get a student with a specific Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Patches a student with the supplied information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonPatchStudent"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] JsonPatchDocument<Student> jsonPatchStudent)
        {
            //Find the student with the Id
            Student updateStudent = await _context.Student.FirstOrDefaultAsync(x => x.StudentId == id);

            if (updateStudent == null)
                return NotFound();

            //Apply the changes
            jsonPatchStudent.ApplyTo(updateStudent, ModelState);

            //Check for errors after changes have been applied
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(updateStudent))
                return BadRequest(ModelState);

            //Save the changes
            _context.Update(updateStudent);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Adds a new student to the students table
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
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
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Delete a student with a specific Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
