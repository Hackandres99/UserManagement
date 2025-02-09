using Microsoft.EntityFrameworkCore;
using UserManagement.Server.Context;

namespace UserManagement.Server.Extentions
{
	public static class SQLServer
	{
		public static IServiceCollection AddSQLServerContext(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("SQLServer");
			services.AddDbContext<UserManagementContext>(options => options.UseSqlServer(connectionString));
			return services;
		}
	}
}
