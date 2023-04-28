using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using OpenFreeSchools.API.Factories.Projects;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Gateways.Projects;

namespace ConcernsCaseWork.API.UseCases.Project
{
    public class CreateProject : IUseCase<CreateProjectRequest, ProjectResponse>
	{
		private readonly IProjectGateway _gateway;

		public CreateProject(IProjectGateway gateway)
		{
			_gateway = gateway;
		}

		public ProjectResponse Execute(CreateProjectRequest request)
		{
			return ExecuteAsync(request).Result;
		}

		public async Task<ProjectResponse> ExecuteAsync(CreateProjectRequest request)
		{
			var dbModel = ProjectFactory.CreateDBModel(request);
			var createdProjects = await _gateway.CreateProject(dbModel);

			return ProjectFactory.CreateResponse(createdProjects);
		}
	}
}
