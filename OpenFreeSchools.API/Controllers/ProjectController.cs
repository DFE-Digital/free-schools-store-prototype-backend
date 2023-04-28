using Microsoft.AspNetCore.Mvc;
using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using OpenFreeSchools.API.UseCases;

namespace ConcernsCaseWork.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		private readonly ILogger<ProjectController> _logger;
		private readonly IUseCase<CreateProjectRequest, ProjectResponse> _createProjectUseCase;

		public ProjectController(ILogger<ProjectController> logger, IUseCase<CreateProjectRequest, ProjectResponse> createProjectUseCase)
		{
			_logger = logger;
			_createProjectUseCase = createProjectUseCase;
		}

		[HttpGet]
		[MapToApiVersion("1.0")]
		public async Task<ActionResult<ApiSingleResponseV2<ProjectResponse>>> GetProject(CancellationToken cancellationToken = default)
		{
			var response = new ProjectResponse
			{
				Id = 1,
				ProjectId = "boop",
				SchoolName = "Name",
				CreatedAt = DateTime.UtcNow,
				CreatedBy = "You",
				UpdatedAt = DateTime.UtcNow
			};

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
	}
}
