using Microsoft.EntityFrameworkCore;
using UserManagement.Server.Models;

namespace UserManagement.Server.Context;

public partial class UserManagementContext : DbContext
{
	public UserManagementContext()
	{
	}

	public UserManagementContext(DbContextOptions<UserManagementContext> options)
		: base(options)
	{
	}

	public virtual DbSet<User> Users { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer("Server=DESKTOP-88NKHV0\\MSSQLSERVER1; Database=UserManagement; Integrated Security=True; TrustServerCertificate=True;");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07A7A70827");

			entity.ToTable("user", tb =>
			{
				tb.HasTrigger("Trigger_creationDate");
				tb.HasTrigger("Trigger_updateDate");
				tb.HasTrigger("Trigger_username");
			});

			entity.HasIndex(e => e.Email, "UK_email").IsUnique();

			entity.HasIndex(e => e.Username, "UK_username").IsUnique();

			entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
			entity.Property(e => e.AccountPassword)
				.HasMaxLength(256)
				.HasColumnName("accountPassword");
			entity.Property(e => e.AvatarImagePath)
				.HasMaxLength(256)
				.HasColumnName("avatarImagePath");
			entity.Property(e => e.CreationDate)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime")
				.HasColumnName("creationDate");
			entity.Property(e => e.Email)
				.HasMaxLength(100)
				.HasColumnName("email");
			entity.Property(e => e.Firstname)
				.HasMaxLength(25)
				.HasColumnName("firstname");
			entity.Property(e => e.Lastname)
				.HasMaxLength(25)
				.HasColumnName("lastname");
			entity.Property(e => e.UpdateDate)
				.HasColumnType("datetime")
				.HasColumnName("updateDate");
			entity.Property(e => e.Username)
				.HasMaxLength(50)
				.HasColumnName("username");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
