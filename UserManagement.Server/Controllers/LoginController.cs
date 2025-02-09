using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Server.Context;
using UserManagement.Server.Extentions;
using UserManagement.Server.Models;

namespace UserManagement.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly UserManagementContext _context;
		private IConfiguration _config;

		public LoginController(UserManagementContext context, IConfiguration configuration)
		{
			_context = context;
			_config = configuration;
		}

		[HttpPost("signin")]
		[AllowAnonymous]
		public async Task<IActionResult> Signin([FromBody] Login login)
		{
			if (login != null)
			{
				login.AccountPassword = EncryptData.EncryptPass(login.AccountPassword);
				var userFound = _context.Users.FirstOrDefault(user =>
				user.Email == login.Email && user.AccountPassword == login.AccountPassword);

				if (userFound != null)
				{
					var token = BuildToken(userFound);
					await _context.SaveChangesAsync();
					return Ok(new { token });
				}
				else return NotFound(new { error = "User not found." });
			}
			else return BadRequest();
		}

		private string BuildToken(User user)
		{
			var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Auth:Key"]));
			var claims = new List<Claim> {
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};
			var expiration = DateTime.Now.AddDays(Convert.ToDouble(_config["Auth:Expiration"]));
			var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(claims: claims, expires: expiration, signingCredentials: credentials);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
