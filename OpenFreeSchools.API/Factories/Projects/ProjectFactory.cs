using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using OpenFreeSchools.Data.Enums;
using OpenFreeSchools.Data.Models;
using OpenFreeSchools.Data.Models.Projects;

namespace OpenFreeSchools.API.Factories.Projects
{
    public class ProjectFactory
	{
		public static Project CreateDBModel(CreateProjectRequest createProjectRequest)
		{
			return new Project
			{
				ProjectId = createProjectRequest.ProjectId,
				SchoolName = createProjectRequest.SchoolName,
                ApplicationNumber = createProjectRequest.ApplicationNumber,
                ApplicationWave = createProjectRequest.ApplicationWave,
                CreatedAt = DateTime.Now,
				CreatedBy = createProjectRequest.CreatedBy			
			};
		}      

        public static ProjectResponse CreateResponse(Project model)
		{
			return new ProjectResponse
			{
				Id = model.Id,
				ProjectId = model.ProjectId,
				SchoolName= model.SchoolName,
				ApplicationNumber = model.ApplicationNumber,
				ApplicationWave = model.ApplicationWave,
				CreatedAt = model.CreatedAt,
				CreatedBy = model.CreatedBy,
				UpdatedAt= model.UpdatedAt,
			};
		}

            public static Project EditDBModel(EditProjectRequest editProjectRequest)
            {
                return new Project
                {
                    ProjectId = editProjectRequest.ProjectId,
                    SchoolName = editProjectRequest.SchoolName,
                    ApplicationNumber = editProjectRequest.ApplicationNumber,
                    ApplicationWave = editProjectRequest.ApplicationWave,
                    CreatedAt = DateTime.Now,
                    CreatedBy = editProjectRequest.CreatedBy
                };
            }
        
            public static ProjectResponse EditResponse(Project model)
            {
                return new ProjectResponse
                {
                    Id = model.Id,
                    ProjectId = model.ProjectId,
                    SchoolName = model.SchoolName,
                    ApplicationNumber = model.ApplicationNumber,
                    ApplicationWave = model.ApplicationWave,
                    CreatedAt = model.CreatedAt,
                    CreatedBy = model.CreatedBy,
                    UpdatedAt = model.UpdatedAt,
                };
            }

        public static Project DeleteDBModel(DeleteProjectRequest deleteProjectRequest)
        {
            return new Project
            {
                ProjectId = deleteProjectRequest.ProjectId,
            };
        }

        public static ProjectResponse DeleteResponse(Project model)
        {
            return new ProjectResponse
            {
                Id = model.Id,
                ProjectId = model.ProjectId,
                SchoolName = model.SchoolName,
                ApplicationNumber = model.ApplicationNumber,
                ApplicationWave = model.ApplicationWave,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedAt = model.UpdatedAt,
            };
        }
    }
}
