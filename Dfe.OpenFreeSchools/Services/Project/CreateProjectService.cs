using Dfe.OpenFreeSchools;
using OpenFreeSchools.API.Contracts.RequestModels.Projects;
using OpenFreeSchools.API.Contracts.ResponseModels;
using OpenFreeSchools.API.Contracts.ResponseModels.Project;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dfe.OpenFreeSchools.Services.Project
{
    public class CreateProjectService : ICreateProjectService
    {
        //   public ILogger<CreateProjectService> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public CreateProjectService(IHttpClientFactory clientFactory)
        { //(ILogger<CreateProjectService> logger, IHttpClientFactory clientFactory) { 
          //  _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<long> CreateProject(string ProjectID, string CurrentFreeSchoolName, string FreeSchoolsApplicationNumber, string FreeSchoolApplicationWave, string CreatedBy)
        {
            //   _logger.LogInformation("CreateProjectService::CreateProject execution");
            try
            {
                //        _logger.LogInformation("CaseService::PostCase");

                // Create a request
                var request = new StringContent(
                    JsonSerializer.Serialize(new CreateProjectRequest() { FreeSchoolsApplicationNumber = FreeSchoolsApplicationNumber, FreeSchoolApplicationWave = FreeSchoolApplicationWave, ProjectId = ProjectID, CurrentFreeSchoolName = CurrentFreeSchoolName, CreatedBy = CreatedBy }),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                // Create http client
                var client = _clientFactory.CreateClient();

                

                // Execute request
                var response = await client.PostAsync($"https://localhost:3001/api/Project", request);

                // Check status code
                response.EnsureSuccessStatusCode();

                // Read response content
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize content to POCO
                var apiWrapperNewCaseDto = JsonSerializer.Deserialize<ApiSingleResponseV2<ProjectResponse>>(content);

                // Unwrap response
                if (apiWrapperNewCaseDto is { Data: { } })
                {
                    return apiWrapperNewCaseDto.Data.Id;
                }

                throw new Exception("Academies API error unwrap response");
            }
            catch (Exception ex)
            {
                //       _logger.LogError("CaseService::PostCase::Exception message::{Message}", ex.Message);

                throw;
            }
        }
    }
}
