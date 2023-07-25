using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenFreeSchools.Data.Models.Projects
{
	public class Project : IAuditable
	{
		public int Id { get; set; }
        public string ProjectId { get; set; }
		[Column("Project Status.Current free school name")]
        public string CurrentFreeSchoolName { get; set; }
		[Column("Project Status.Free schools application number")]
        public string FreeSchoolsApplicationNumber { get; set; }
        [Column("Project Status.Free school application wave")]
        public string FreeSchoolApplicationWave { get; set; }
        public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string CreatedBy { get; set; }

		public override string ToString() =>
            CurrentFreeSchoolName +
			(string.IsNullOrEmpty(ProjectId)
				? ""
				: ": " + ProjectId);
	}
}
