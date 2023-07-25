namespace Dfe.BuildFreeSchools.Services.Project
{
    public interface ICreateProjectService
    {
        Task<long> CreateProject(string ProjectID, string CurrentFreeSchoolName, string FreeSchoolsApplicationNumber, string FreeSchoolApplicationWave, string CreatedBy);
    }
}
