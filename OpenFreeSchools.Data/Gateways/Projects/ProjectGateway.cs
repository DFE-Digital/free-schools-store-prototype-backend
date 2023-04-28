using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenFreeSchools.Data.Models;
using OpenFreeSchools.Data.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFreeSchools.Data.Gateways.Projects
{
	public interface IProjectGateway
	{
		Task<Project> CreateProject(Project request);
	}

	public class ProjectGateway : IProjectGateway
	{
		private readonly ILogger<ProjectGateway> _logger;
		private readonly OpenFreeSchoolsDbContext _openFreeSchoolsDbContext;

		public ProjectGateway(ILogger<ProjectGateway> logger, OpenFreeSchoolsDbContext openFreeSchoolsDbContext)
		{
			_logger = logger;
			_openFreeSchoolsDbContext= openFreeSchoolsDbContext;
		}

        public async Task<Project> GetProject(Project request)
        {
            try
            {
                request.UpdatedAt = request.CreatedAt;
				//_openFreeSchoolsDbContext.Projects.All;
                await _openFreeSchoolsDbContext.SaveChangesAsync();
                return request;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Failed to create Project with Id {Id}, {ex}", request.Id, ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An application exception has occurred whilst creating Project with Id {Id}, {ex}", request.Id, ex);
                throw;
            }
        }

        public async Task<Project> CreateProject(Project request)
		{
			try
			{
				request.UpdatedAt = request.CreatedAt;
				_openFreeSchoolsDbContext.Projects.Add(request);
				await _openFreeSchoolsDbContext.SaveChangesAsync();
				return request;
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError("Failed to create Project with Id {Id}, {ex}", request.Id, ex);
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError("An application exception has occurred whilst creating Project with Id {Id}, {ex}", request.Id, ex);
				throw;
			}
		}
	}
}
