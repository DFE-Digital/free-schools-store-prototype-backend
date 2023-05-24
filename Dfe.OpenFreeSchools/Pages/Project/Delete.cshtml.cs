
using Dfe.OpenFreeSchools.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Pages.Project
{
    public class DeleteProjectModel : PageModel
    {
        [BindProperty]
        [MaxLength(10)]
        public string ProjectID { get; set; }

        public IDeleteProjectService _deleteProjectService { get; }
        //    public ILogger<CreateProjectModel> _logger { get; }

        public DeleteProjectModel(
            IDeleteProjectService deleteProjectService
            //   ILogger<CreateProjectModel> logger
            )
        {
            _deleteProjectService = deleteProjectService;
            //_logger = logger;
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

                var caseUrn = await _deleteProjectService.DeleteProject(ProjectID);

                return Redirect($"/");
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "error");

                // TempData["Error.Message"] = ErrorOnPostPage;
            }

            return Page();
        }
    }
}
