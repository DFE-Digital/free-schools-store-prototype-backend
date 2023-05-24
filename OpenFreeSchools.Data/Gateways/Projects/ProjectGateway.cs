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
		Project[] GetProjectsByUser(string user);
        Task<Project> DeleteProject(Project request);
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

        public Project[] GetProjectsByUser(string user)
        {
            try
            {
				var projects = _openFreeSchoolsDbContext.Projects.Where(x => x.CreatedBy == user).ToArray();
                return projects;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Failed to create Project with Id {Id}, {ex}", user, ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An application exception has occurred whilst creating Project with Id {Id}, {ex}", user, ex);
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

        public async Task<Project> DeleteProject(Project request)
        {
            try
            {
                request.UpdatedAt = request.CreatedAt;
                _openFreeSchoolsDbContext.Projects.Where(p => p.ProjectId == request.ProjectId).ExecuteDelete();
                await _openFreeSchoolsDbContext.SaveChangesAsync();
                return request;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Failed to delete Project with Id {Id}, {ex}", request.Id, ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An application exception has occurred whilst deleting Project with Id {Id}, {ex}", request.Id, ex);
                throw;
            }
        }
    }
}
