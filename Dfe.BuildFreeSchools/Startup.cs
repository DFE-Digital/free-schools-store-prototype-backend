using Dfe.BuildFreeSchools.Services.Project;

namespace Dfe.BuildFreeSchools
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICreateProjectService, CreateProjectService>();
        }
    }
}
