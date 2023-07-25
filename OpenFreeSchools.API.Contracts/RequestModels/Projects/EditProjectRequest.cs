using System.ComponentModel.DataAnnotations;

namespace OpenFreeSchools.API.Contracts.RequestModels.Projects
{
    public class EditProjectRequest
    {
        [Required]
        public string ProjectId { get; set; }
        [Required]
        public string CurrentFreeSchoolName { get; set; }
        [Required]
        public string FreeSchoolsApplicationNumber { get; set; }
        [Required]
        public string FreeSchoolApplicationWave { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }
}
