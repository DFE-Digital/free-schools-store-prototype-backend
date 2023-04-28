using System.ComponentModel.DataAnnotations;

namespace OpenFreeSchools.API.Contracts.RequestModels.Projects
{
    public class CreateProjectRequest
    {
        [Required]
        public string ProjectId { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string ApplicationNumber { get; set; }
        [Required]
        public string ApplicationWave { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }
}
