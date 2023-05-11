﻿using OpenFreeSchools.API.Contracts.RequestModels.Projects;
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
	}
}
