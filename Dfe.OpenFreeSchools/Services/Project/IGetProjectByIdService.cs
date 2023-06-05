using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Services.Project
{
    public interface IGetProjectByIdService
    {
        Task<long> GetProject(string ProjectID);
    }
}
