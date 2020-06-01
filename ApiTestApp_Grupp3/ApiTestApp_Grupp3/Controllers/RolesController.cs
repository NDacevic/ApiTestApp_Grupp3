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
    public class RolesController : ControllerBase
    {
        private readonly ApiTestApp_Grupp3Context _context;

        public RolesController(ApiTestApp_Grupp3Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a list of all Role objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRole()
        {
            return await _context.Role.ToListAsync();
        }

        /// <summary>
        /// Get a role object with a specific id
        /// Currently unused
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Role.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        /// <summary>
        /// Alter a Role with a specific id
        /// Currently unused
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        /// <summary>
        /// Post a new role to the database
        /// Currently unused
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.RoleId }, role);
        }

        /// <summary>
        /// Delete a role from the database
        /// currently unused
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Role>> DeleteRole(int id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Role.Remove(role);
            await _context.SaveChangesAsync();

            return role;
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.RoleId == id);
        }
    }
}
