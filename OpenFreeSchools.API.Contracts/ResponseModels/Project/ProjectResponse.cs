using System.ComponentModel.DataAnnotations;

namespace OpenFreeSchools.API.Contracts.ResponseModels.Project
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string SchoolName { get; set; }
        public string ApplicationNumber { get; set; }
        public string ApplicationWave { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }

    }
}
