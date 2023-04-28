using OpenFreeSchools.Data;

namespace OpenFreeSchools.API.StartupConfiguration;

public static class DatabaseConfigurationExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<OpenFreeSchoolsDbContext>(options =>
			options.UseConcernsSqlServer(connectionString)
		);
			
		return services;
	}
}