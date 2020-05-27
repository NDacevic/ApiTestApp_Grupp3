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
    public class EmployeeRolesController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public EmployeeRolesController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        // GET: api/EmployeeRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeRole>>> GetEmployeeRole()
        {
            return await _context.EmployeeRole.ToListAsync();
        }

        // GET: api/EmployeeRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeRole>> GetEmployeeRole(int id)
        {
            var employeeRole = await _context.EmployeeRole.FindAsync(id);

            if (employeeRole == null)
            {
                return NotFound();
            }

            return employeeRole;
        }

        // PUT: api/EmployeeRoles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeRole(int id, EmployeeRole employeeRole)
        {
            if (id != employeeRole.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employeeRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeRoleExists(id))
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

        // POST: api/EmployeeRoles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EmployeeRole>> PostEmployeeRole(EmployeeRole employeeRole)
        {
            _context.EmployeeRole.Add(employeeRole);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeRoleExists(employeeRole.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeeRole", new { id = employeeRole.EmployeeId }, employeeRole);
        }

        // DELETE: api/EmployeeRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeRole>> DeleteEmployeeRole(int id)
        {
            var employeeRole = await _context.EmployeeRole.FindAsync(id);
            if (employeeRole == null)
            {
                return NotFound();
            }

            _context.EmployeeRole.Remove(employeeRole);
            await _context.SaveChangesAsync();

            return employeeRole;
        }

        private bool EmployeeRoleExists(int id)
        {
            return _context.EmployeeRole.Any(e => e.EmployeeId == id);
        }
    }
}
