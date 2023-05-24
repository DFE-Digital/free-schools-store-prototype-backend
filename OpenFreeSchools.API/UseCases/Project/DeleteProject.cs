using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using OpenFreeSchools.API.Factories.Projects;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Gateways.Projects;

namespace ConcernsCaseWork.API.UseCases.Project
{
    public class DeleteProject : IUseCase<DeleteProjectRequest, ProjectResponse>
	{
		private readonly IProjectGateway _gateway;

		public DeleteProject(IProjectGateway gateway)
		{
            _gateway = gateway;
		}

		public ProjectResponse Execute(DeleteProjectRequest request)
		{
			return ExecuteAsync(request).Result;
		}

		public async Task<ProjectResponse> ExecuteAsync(DeleteProjectRequest request)
		{
			var dbModel = ProjectFactory.DeleteDBModel(request);
			var deletedProjects = await _gateway.DeleteProject(dbModel);

			return ProjectFactory.DeleteResponse(deletedProjects);
		}
	}
}
