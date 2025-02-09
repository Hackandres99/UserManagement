using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace UserManagement.Server.Extentions
{
	public class EmailSender : IEmailSender
	{
		private readonly string smtpServer = "smtp.gmail.com";
		private readonly int smtpPort = 587;
		private readonly string smtpUser = "andres19saavedra9castro9@gmail.com";
		private readonly string smtpPassword = "jliz mfvf ivkx msdv";

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			using (var client = new SmtpClient(smtpServer, smtpPort))
			{
				client.Credentials = new NetworkCredential(smtpUser, smtpPassword);
				client.EnableSsl = true;

				var mailMessage = new MailMessage
				{
					From = new MailAddress(smtpUser),
					Subject = subject,
					Body = htmlMessage,
					IsBodyHtml = true
				};

				mailMessage.To.Add(email);

				await client.SendMailAsync(mailMessage);
			}
		}
	}
}
