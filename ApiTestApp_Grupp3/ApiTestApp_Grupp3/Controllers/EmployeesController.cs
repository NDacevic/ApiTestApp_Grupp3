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

        /// <summary>
        /// Get a list of all the employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            return await _context.Employee.ToListAsync();
        }

        /// <summary>
        /// Get an employee with a specific id
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Patches an employee with a specific Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonPatchEmployee"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTest(int id, [FromBody] JsonPatchDocument<Employee> jsonPatchEmployee)
        {
            //find the employee with the Id
            Employee updateEmployee = await _context.Employee.FirstOrDefaultAsync(x => x.EmployeeId == id);

            if (updateEmployee == null)
                return NotFound();

            //Apply the changes and append the entitystate object
            jsonPatchEmployee.ApplyTo(updateEmployee, ModelState);

            //Check for errors
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(updateEmployee))
                return BadRequest(ModelState);

            //Save the changes
            _context.Update(updateEmployee);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Adds an employee to the database and connects the employee with a specific role
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Delete an employee with a speicifc Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
