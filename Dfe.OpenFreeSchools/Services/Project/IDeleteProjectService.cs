using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Services.Project
{
    public interface IDeleteProjectService
    {
        Task<long> DeleteProject(string ProjectID);
    }
}
