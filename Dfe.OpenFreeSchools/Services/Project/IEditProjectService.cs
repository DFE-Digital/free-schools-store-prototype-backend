using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Services.Project
{
    public interface IEditProjectService
    {
        Task<long> EditProject(string ProjectID, string CurrentFreeSchoolName, string FreeSchoolsApplicationNumber, string FreeSchoolApplicationWave, string CreatedBy);
    }
}
