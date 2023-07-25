using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenFreeSchools.API.Contracts.ResponseModels.Project
{
    public class ProjectResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
		[JsonPropertyName("projectId")]
		public string ProjectId { get; set; }
        [JsonPropertyName("currentFreeSchoolName")]
        public string CurrentFreeSchoolName { get; set; }
        [JsonPropertyName("freeSchoolsApplicationNumber")]
        public string FreeSchoolsApplicationNumber { get; set; }
		[JsonPropertyName("freeSchoolApplicationWave")]
		public string FreeSchoolApplicationWave { get; set; }
		[JsonPropertyName("createdAt")]
		public DateTime CreatedAt { get; set; }
		[JsonPropertyName("updatedAt")]
		public DateTime? UpdatedAt { get; set; }
		[JsonPropertyName("createdBy")]
		public string CreatedBy { get; set; }

    }
}
