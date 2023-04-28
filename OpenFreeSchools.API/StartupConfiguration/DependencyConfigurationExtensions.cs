using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways.Projects;
using OpenFreeSchools.Logging;
using OpenFreeSchools.UserContext;

namespace OpenFreeSchools.API.StartupConfiguration
{
	public static class DependencyConfigurationExtensions
	{
		public static IServiceCollection AddUseCases(this IServiceCollection services)
		{
			var allTypes = typeof(IUseCase<,>).Assembly.GetTypes();

			foreach (var type in allTypes)
			{
				foreach (var @interface in type.GetInterfaces())
				{
					if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IUseCase<,>))
					{
						services.AddScoped(@interface, type);
					}
				}
			}

			return services;
		}

		public static IServiceCollection AddUseCaseAsyncs(this IServiceCollection services)
		{
			var allTypes = typeof(IUseCaseAsync<,>).Assembly.GetTypes();

			foreach (var type in allTypes)
			{
				foreach (var @interface in type.GetInterfaces())
				{
					if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IUseCaseAsync<,>))
					{
						services.AddScoped(@interface, type);
					}
				}
			}

			return services;
		}

		public static IServiceCollection AddApiDependencies(this IServiceCollection services)
		{
			services.AddScoped<IProjectGateway, ProjectGateway>();

			services.AddScoped<IServerUserInfoService, ServerUserInfoService>();
			
			services.AddScoped<ICorrelationContext, CorrelationContext>();

			return services;
		}
	}
}
