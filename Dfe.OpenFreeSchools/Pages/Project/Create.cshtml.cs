
using Dfe.OpenFreeSchools.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Pages.Project
{
    public class CreateProjectModel : PageModel
    {
        [BindProperty]
        [MaxLength(10)]
        public string ProjectID { get; set; }

        [BindProperty]
        [MaxLength(20)]
        public string SchoolName { get; set; }

        [BindProperty]
        [MaxLength(10)]
        public string ApplicationNumber { get; set; }

        [BindProperty]
        [MaxLength(10)]
        public string ApplicationWave { get; set; }
        public ICreateProjectService _createProjectService { get; }
    //    public ILogger<CreateProjectModel> _logger { get; }

        public CreateProjectModel(
            ICreateProjectService createProjectService
         //   ILogger<CreateProjectModel> logger
            )
        {
            _createProjectService = createProjectService;
            //_logger = logger;
        }

        public void OnGet()
        {
            ProjectID = "hi";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //_logger.LogMethodEntered();

            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var caseUrn = await _createProjectService.CreateProject(ProjectID, SchoolName, ApplicationNumber, ApplicationWave, "Test");

                return Redirect($"/case/{caseUrn}/management");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "error");

                // TempData["Error.Message"] = ErrorOnPostPage;
            }

            return Page();
        }
    }
}
