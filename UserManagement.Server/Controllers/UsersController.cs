using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserManagement.Server.Context;
using UserManagement.Server.Extentions;
using UserManagement.Server.Models;

namespace UserManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManagementContext _context;

        public UsersController(UserManagementContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<ActionResult<User>> GetUser()
        {
			var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (id == null) return Unauthorized();

			Guid.TryParse(id, out Guid userId);
			var user = await _context.Users.FindAsync(userId);
			if (user == null) return NotFound();

			return Ok(user);
		}

        [HttpPut("me")]
        public async Task<IActionResult> PutUser(User user)
        {
			var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Guid.TryParse(id, out Guid userId);
			if (id == null || user.Id != userId) return Unauthorized();
            user.CreationDate = null;
            user.UpdateDate = null;
			user.AccountPassword = EncryptData.EncryptPass(user.AccountPassword);

			_context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userId)) return NotFound();
				else throw;
			}

            return NoContent();
        }

        [HttpPost("signup")]
		[AllowAnonymous]
		public async Task<ActionResult<User>> PostUser(User user)
        {
			user.AccountPassword = EncryptData.EncryptPass(user.AccountPassword);
			_context.Users.Add(user);
            await _context.SaveChangesAsync();
			await _context.Entry(user).ReloadAsync();

			return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteUser()
        {
			var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Guid.TryParse(id, out Guid userId);
			if (id == null) return Unauthorized();

			var user = await _context.Users.FindAsync(userId);
			if (user == null) return NotFound();

			_context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
