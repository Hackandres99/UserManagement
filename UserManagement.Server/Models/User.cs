namespace UserManagement.Server.Models;

public partial class User
{
	public Guid Id { get; set; }

	public string Firstname { get; set; } = null!;

	public string Lastname { get; set; } = null!;

	public string? Username { get; set; }

	public string Email { get; set; } = null!;

	public string AccountPassword { get; set; } = null!;

	public string? AvatarImagePath { get; set; }

	public DateTime? CreationDate { get; set; }

	public DateTime? UpdateDate { get; set; }
}
