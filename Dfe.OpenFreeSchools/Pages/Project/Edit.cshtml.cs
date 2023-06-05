
using Dfe.OpenFreeSchools.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Pages.Project
{
    public class EditProjectModel : PageModel
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
        public IEditProjectService _editProjectService { get; }
    //    public ILogger<EditProjectModel> _logger { get; }

        public EditProjectModel(
            IEditProjectService editProjectService
         //   ILogger<EditProjectModel> logger
            )
        {
            _editProjectService = editProjectService;
            //_logger = logger;
        }

        public void OnGet()
        {
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

                var caseUrn = await _editProjectService.EditProject(ProjectID, SchoolName, ApplicationNumber, ApplicationWave, "Sukhy");

                return Redirect($"/");
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
