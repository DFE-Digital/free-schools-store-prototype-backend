using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using OpenFreeSchools.API.Factories.Projects;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Gateways.Projects;

namespace ConcernsCaseWork.API.UseCases.Project
{
    public class GetAllProjects : IUseCase<GetAllProjectsRequest, ProjectResponse[]>
	{
		private readonly IProjectGateway _gateway;

		public GetAllProjects(IProjectGateway gateway)
		{
			_gateway = gateway;
		}

		public ProjectResponse[] Execute(GetAllProjectsRequest request)
		{
			return _gateway.GetProjectsByUser(request.User)
						   .Select(x => ProjectFactory.CreateResponse(x))
						   .ToArray();
		}

	}
}
