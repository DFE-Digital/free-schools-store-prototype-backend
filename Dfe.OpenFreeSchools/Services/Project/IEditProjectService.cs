using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Services.Project
{
    public interface IEditProjectService
    {
        Task<long> EditProject(string ProjectID, string SchoolName, string ApplicationNumber, string ApplicationWave, string CreatedBy);
    }
}
