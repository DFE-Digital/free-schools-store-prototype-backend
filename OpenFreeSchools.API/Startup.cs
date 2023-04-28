using OpenFreeSchools.API.Extensions;
using OpenFreeSchools.API.Middleware;
using OpenFreeSchools.API.StartupConfiguration;
using OpenFreeSchools.Middleware;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace OpenFreeSchools.API
{
	/// <summary>
	/// THIS STARTUP ISN'T USED WHEN API IS HOSTED THROUGH WEBSITE. It is used when running API tests
	/// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddConcernsApiProject(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
	        app.UseOpenFreeSchoolsSwagger(provider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseMiddleware<UrlDecoderMiddleware>();
            app.UseMiddleware<CorrelationIdMiddleware>();
			app.UseMiddleware<UserContextReceiverMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenFreeSchoolsEndpoints();
        }
    }
}
