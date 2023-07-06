using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Services.Project
{
    public interface IGetProjectByIdService
    {
        public Task<ProjectResponse> GetProject(string ProjectID);
    }
}
