using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using OpenFreeSchools.API.Factories.Projects;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Gateways.Projects;

namespace ConcernsCaseWork.API.UseCases.Project
{
    public class EditProject : IUseCase<EditProjectRequest, ProjectResponse>
    {
        private readonly IProjectGateway _gateway;

        public EditProject(IProjectGateway gateway)
        {
            _gateway = gateway;
        }

        public ProjectResponse Execute(EditProjectRequest request)
        {
            return ExecuteAsync(request).Result;
        }

        public async Task<ProjectResponse> ExecuteAsync(EditProjectRequest request)
        {
            var dbModel = ProjectFactory.EditDBModel(request);
            var editedprojects = await _gateway.EditProject(dbModel);

            return ProjectFactory.CreateResponse(editedprojects);
        }
    }
}
