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
			var userId = AuthorizedUserId();
			if (userId == null) return Unauthorized();

			var user = await _context.Users.FindAsync(userId);
			if (user == null) return NotFound();

			return Ok(user);
		}

        [HttpPut("me")]
        public async Task<IActionResult> PutUser(User user)
        {
			var userId = AuthorizedUserId();
			if (user.Id != userId) return Unauthorized();
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
			var userId = AuthorizedUserId();
			if (userId == null) return Unauthorized();

			var user = await _context.Users.FindAsync(userId);
			if (user == null) return NotFound();

			_context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

		[HttpPost("upload-my-avatar")]
		public async Task<IActionResult> UploadAvatar(IFormFile file)
		{
			var userId = AuthorizedUserId();
			if (userId == null) return Unauthorized();

			if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");

			if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

			var avatarImagePath = $"{Guid.NewGuid()}_{file.FileName}";
			var filePath = Path.Combine(uploadPath, avatarImagePath);

			// Guardar el nuevo archivo
			using (var stream = new FileStream(filePath, FileMode.Create))
				await file.CopyToAsync(stream);

			var user = await _context.Users.FindAsync(userId);
			if (user == null) return NotFound("User not found");

			// Eliminar el avatar anterior si existe
			if (!string.IsNullOrEmpty(user.AvatarImagePath))
			{
				var oldAvatarPath = Path.Combine(uploadPath, user.AvatarImagePath);
				if (System.IO.File.Exists(oldAvatarPath)) System.IO.File.Delete(oldAvatarPath);
			}

			// Actualizar el avatar del usuario
			user.AvatarImagePath = avatarImagePath;
			await _context.SaveChangesAsync();

			return Ok(new { avatarImagePath });
		}

		private Guid? AuthorizedUserId()
		{
			var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Guid.TryParse(id, out Guid userId);

			return userId;
		}

		private bool UserExists(Guid? id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
