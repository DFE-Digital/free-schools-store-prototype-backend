using ConcernsCaseWork.API.UseCases.Project;
using Microsoft.AspNetCore.Mvc;
using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using OpenFreeSchools.API.ResponseModels;
using OpenFreeSchools.API.UseCases;

namespace ConcernsCaseWork.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		private readonly ILogger<ProjectController> _logger;
		private readonly IUseCase<CreateProjectRequest, ProjectResponse> _createProjectUseCase;
        private readonly IUseCase<GetAllProjectsRequest, ProjectResponse[]> _getAllProjectsUseCase;
        private readonly IUseCase<GetProjectRequest, ProjectResponse> _getProjectUseCase;
        private readonly IUseCase<DeleteProjectRequest, ProjectResponse> _deleteProjectUseCase;
        private readonly IUseCase<EditProjectRequest, ProjectResponse> _editProjectUseCase;

        public ProjectController(ILogger<ProjectController> logger, 
								 IUseCase<CreateProjectRequest, ProjectResponse> createProjectUseCase, 
								 IUseCase<GetAllProjectsRequest, ProjectResponse[]> getAllProjectsUseCase,
                                 IUseCase<GetProjectRequest, ProjectResponse> getProjectUseCase,
								 IUseCase<DeleteProjectRequest, ProjectResponse> deleteProjectUseCase,
                                 IUseCase<EditProjectRequest, ProjectResponse> editProjectUseCase)

        {
			_logger = logger;
			_createProjectUseCase = createProjectUseCase;
            _getAllProjectsUseCase = getAllProjectsUseCase;
            _getProjectUseCase = getProjectUseCase;
            _deleteProjectUseCase = deleteProjectUseCase;
            _editProjectUseCase = editProjectUseCase;
		}

		[HttpGet]
		[MapToApiVersion("1.0")]
		public async Task<ActionResult<ApiResponseV2<ProjectResponse[]>>> GetAllProjects(string user, CancellationToken cancellationToken = default)
		{
			var projects = _getAllProjectsUseCase.Execute(new GetAllProjectsRequest() { User = user});

			var pagingResponse = PagingResponseFactory.Create(1, projects.Count(), projects.Count(), Request);
			var response = new ApiResponseV2<ProjectResponse>(projects, pagingResponse);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
		}

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseV2<ProjectResponse>>> GetProject(string projectId, CancellationToken cancellationToken = default)
        {
            var project = _getProjectUseCase.Execute(new GetProjectRequest() { ProjectId = projectId });

            var response = new ApiResponseV2<ProjectResponse>(project);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
        }

        [HttpPost]
		[MapToApiVersion("1.0")]
		public async Task<ActionResult<ApiSingleResponseV2<ProjectResponse>>> Create(CreateProjectRequest request, CancellationToken cancellationToken = default)
		{
			var createdProject = _createProjectUseCase.Execute(request);
			var response = new ApiSingleResponseV2<ProjectResponse>(createdProject);

			return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
		}

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ApiSingleResponseV2<ProjectResponse>>> Edit(EditProjectRequest request, CancellationToken cancellationToken = default)
        {
            var editedProject = _editProjectUseCase.Execute(request);
            var response = new ApiSingleResponseV2<ProjectResponse>(editedProject);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
        }

        [HttpDelete]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ApiSingleResponseV2<ProjectResponse>>> Delete(string projectId, CancellationToken cancellationToken = default)
        {

            var deletedProject = _deleteProjectUseCase.Execute(new DeleteProjectRequest() {  ProjectId = projectId });
            var response = new ApiSingleResponseV2<ProjectResponse>(deletedProject);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
