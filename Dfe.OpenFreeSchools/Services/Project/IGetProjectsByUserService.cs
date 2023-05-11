using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Services.Project
{
    public interface IGetProjectsByUserService
    {
        public Task<ProjectResponse[]> GetProjects(string CreatedBy);
    }
}
