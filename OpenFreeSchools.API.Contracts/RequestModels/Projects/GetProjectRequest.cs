using System.ComponentModel.DataAnnotations;

namespace OpenFreeSchools.API.Contracts.RequestModels.Projects
{
    public class GetProjectRequest
    {
        [Required]
        public string ProjectId { get; set; }
    }
}
