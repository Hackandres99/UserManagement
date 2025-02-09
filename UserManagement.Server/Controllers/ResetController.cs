using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Cryptography;
using UserManagement.Server.Context;
using UserManagement.Server.Models;
using UserManagement.Server.Extentions;

namespace UserManagement.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResetController : ControllerBase
	{
		private readonly UserManagementContext _context;
		private readonly IEmailSender _emailSender;

		public ResetController(UserManagementContext context, IEmailSender emailSender)
		{
			_context = context;
			_emailSender = emailSender;
		}

		[HttpPost("password")]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword([FromBody] Forgot request)
		{
			if (string.IsNullOrWhiteSpace(request.Email))
				return BadRequest("Email is required.");

			var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
			if (user == null) return NotFound(new { error = "User not found." });

			// Generate a temporary password
			var temporaryPassword = GenerateTemporaryPassword();
			user.AccountPassword = EncryptData.EncryptPass(temporaryPassword);
			await _context.SaveChangesAsync();

			var subject = "Password Reset";
			var message = $"Your temporary password is: {temporaryPassword}. Please login and change it as soon as possible.";
			await _emailSender.SendEmailAsync(request.Email, subject, message);

			return Ok(new { message = "Temporary password sent to your email." });
		}

		private string GenerateTemporaryPassword()
		{
			var bytes = new byte[8];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(bytes);
			}
			return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}

	}
}

