using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFreeSchools.Data.Models.Projects
{
	public class Project : IAuditable
	{
		public int Id { get; set; }
        public string ProjectId { get; set; }
        public string SchoolName { get; set; }
        public string ApplicationNumber { get; set; }
		public string ApplicationWave { get; set; }
        public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string CreatedBy { get; set; }

		public override string ToString() =>
            SchoolName +
			(string.IsNullOrEmpty(ProjectId)
				? ""
				: ": " + ProjectId);
	}
}
