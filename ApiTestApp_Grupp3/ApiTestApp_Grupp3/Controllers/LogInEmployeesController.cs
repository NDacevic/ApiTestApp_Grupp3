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
    public class LogInEmployeesController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public LogInEmployeesController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        // GET: api/LogInEmployees
        [HttpGet]
        public ActionResult GetEmployee()
        {
            return NotFound();
        }
        /// <summary>
        /// Returns the employee with matching email to be able to log in
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        // GET: api/LogInEmployees/5
        [HttpGet("{email}")]
        public async Task<ActionResult<Employee>> GetEmployee(string email)
        {
            var employee = await _context.Employee.Where(e => e.Email == email).Select(e => e).FirstOrDefaultAsync();
            employee.Role = await _context.EmployeeRole.Include(x => x.Role).Where(er => er.EmployeeId == employee.EmployeeId).Select(r => r.Role).FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);

        }

        // PUT: api/LogInEmployees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutEmployee(int id, Employee employee)
        {

            return NotFound();
        }

        // POST: api/LogInEmployees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult PostEmployee(Employee employee)
        {
            return NotFound();
        }

        // DELETE: api/LogInEmployees/5
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        { 

            return NotFound();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}
