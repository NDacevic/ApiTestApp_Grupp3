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
    public class LogInStudentsController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public LogInStudentsController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        // GET: api/LogInStudents
        [HttpGet]
        public ActionResult GetStudent()
        {
            return NotFound();
        }

        // GET: api/LogInStudents/5
        /// <summary>
        /// Returns student with matching email to be able to log in 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        public async Task<ActionResult<Student>> GetStudent(string email)
        {
            var student = await _context.Student.Where(s => s.Email == email).Select(s => s).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/LogInStudents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public ActionResult PutStudent(int id, Student student)
        {

            return NotFound();
        }

        // POST: api/LogInStudents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult PostStudent(Student student)
        {

            return NotFound();
        }

        // DELETE: api/LogInStudents/5
        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {

            return NotFound();
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
