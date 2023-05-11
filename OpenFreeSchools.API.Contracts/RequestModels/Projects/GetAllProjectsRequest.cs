using System.ComponentModel.DataAnnotations;

namespace OpenFreeSchools.API.Contracts.RequestModels.Projects
{
    public class GetAllProjectsRequest
    {
        [Required]
        public string User { get; set; }
    }
}
