using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using OpenFreeSchools.API.Factories.Projects;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Gateways.Projects;

namespace ConcernsCaseWork.API.UseCases.Project
{
    public class getProjectById : IUseCase<GetProjectRequest, ProjectResponse>
	{
		private readonly IProjectGateway _gateway;

		public getProjectById(IProjectGateway gateway)
		{
			_gateway = gateway;
		}

		public ProjectResponse Execute(GetProjectRequest request)
		{

            var project = _gateway.GetProjectById(request.ProjectId);

            return ProjectFactory.CreateResponse(project);

		}

	}
}
