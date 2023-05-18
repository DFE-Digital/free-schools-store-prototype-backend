using System.ComponentModel.DataAnnotations;

namespace OpenFreeSchools.API.Contracts.RequestModels.Projects
{
    public class DeleteProjectRequest
    {
        [Required]
        public string ProjectId { get; set; }
    }
}
