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
    public class EmployeesController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public EmployeesController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            //This is currently used for testing. Change this when API connects successfully.
            //return Ok("Api works. Wohoo");
            return await _context.Employee.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTest(int id, [FromBody] JsonPatchDocument<Employee> jsonPatchEmployee)
        {
            Employee updateEmployee = await _context.Employee.FirstOrDefaultAsync(x => x.EmployeeId == id);

            if (updateEmployee == null)
                return NotFound();

            jsonPatchEmployee.ApplyTo(updateEmployee, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(updateEmployee))
                return BadRequest(ModelState);

            _context.Update(updateEmployee);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            try
            {
                //Adding the new employee to Employee table
               _context.Employee.Add(employee);
                await _context.SaveChangesAsync();

                //Finding the matching RoleId based on the RoleName in employee 
                int id = await _context.Role.Where(x => x.RoleName == employee.Role.RoleName).Select(x => x.RoleId).FirstOrDefaultAsync();
                //Adding the EmployeeID and RoleId to the EmployeeRole table 
                _context.EmployeeRole.Add(new EmployeeRole() { EmployeeId = employee.EmployeeId, RoleId = id });
                await _context.SaveChangesAsync();

            }
            catch
            {

                return BadRequest();
            }

            return Ok();
        }


        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}
