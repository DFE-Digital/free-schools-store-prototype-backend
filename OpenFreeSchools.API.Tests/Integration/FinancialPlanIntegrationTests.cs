﻿using AutoFixture;
using OpenFreeSchools.API.RequestModels.CaseActions.FinancialPlan;
using OpenFreeSchools.API.Tests.Fixtures;
using OpenFreeSchools.API.Tests.Helpers;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.Integration
{
	[Collection(ApiTestCollection.ApiTestCollectionName)]
	public class FinancialPlanIntegrationTests
	{
		private readonly Fixture _fixture;
		private readonly HttpClient _client;

		public FinancialPlanIntegrationTests(ApiTestFixture apiTestFixture)
		{
			_client = apiTestFixture.Client;
			_fixture = new();
		}

		[Fact]
		public async Task When_Post_InvalidRequest_Returns_ValidationErrors()
		{
			var request = _fixture.Create<CreateFinancialPlanRequest>();
			request.Name = new string('a', 301);
			request.CreatedBy = new string('a', 301);
			request.Notes = new string('a', 2001);

			var result = await _client.PostAsync($"/v2/case-actions/financial-plan", request.ConvertToJson());
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);


			var error = await result.Content.ReadAsStringAsync();
			error.Should().Contain("The field Name must be a string with a maximum length of 300.");
			error.Should().Contain("The field CreatedBy must be a string with a maximum length of 300.");
			error.Should().Contain("The field Notes must be a string with a maximum length of 2000.");
		}

		[Fact]
		public async Task When_Patch_InvalidRequest_Returns_ValidationErrors()
		{
			var request = _fixture.Create<PatchFinancialPlanRequest>();
			request.Name = new string('a', 301);
			request.CreatedBy = new string('a', 301);
			request.Notes = new string('a', 2001);

			var result = await _client.PatchAsync($"/v2/case-actions/financial-plan", request.ConvertToJson());
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);


			var error = await result.Content.ReadAsStringAsync();
			error.Should().Contain("The field Name must be a string with a maximum length of 300.");
			error.Should().Contain("The field CreatedBy must be a string with a maximum length of 300.");
			error.Should().Contain("The field Notes must be a string with a maximum length of 2000.");
		}
	}
}
