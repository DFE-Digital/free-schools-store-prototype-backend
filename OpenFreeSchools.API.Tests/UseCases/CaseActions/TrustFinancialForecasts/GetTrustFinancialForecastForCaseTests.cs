using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using OpenFreeSchools.API.Contracts.RequestModels.TrustFinancialForecasts;
using OpenFreeSchools.API.Contracts.ResponseModels.TrustFinancialForecasts;
using OpenFreeSchools.API.Exceptions;
using OpenFreeSchools.API.UseCases;
using OpenFreeSchools.API.UseCases.CaseActions.TrustFinancialForecast;
using OpenFreeSchools.Data.Gateways;
using OpenFreeSchools.Data.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFreeSchools.API.Tests.UseCases.CaseActions.TrustFinancialForecasts;

public class GetTrustFinancialForecastsForCaseTests
{
	private readonly IFixture _fixture;

	public GetTrustFinancialForecastsForCaseTests()
	{
		_fixture = new Fixture();
		_fixture.Register(Mock.Of<IConcernsCaseGateway>);
	}
	
	[Fact]
	public void GetTrustFinancialForecastsForCases_Is_Assignable_To_IUseCaseAsync()
	{
		var sut = new GetTrustFinancialForecastsForCase(Mock.Of<IConcernsCaseGateway>(), Mock.Of<ITrustFinancialForecastGateway>());

		sut.Should()
			.BeAssignableTo<IUseCaseAsync<GetTrustFinancialForecastsForCaseRequest, IEnumerable<TrustFinancialForecastResponse>>>();
	}

	[Fact]
	public async Task Execute_When_ConcernsCase_Not_Found_Throws_Exception()
	{
		// arrange 
		var mockTrustFinancialForecastGateway = new Mock<ITrustFinancialForecastGateway>();
		var mockCaseGateWay = new Mock<IConcernsCaseGateway>();

		var caseUrn = _fixture.Create<int>();
		
		var request = _fixture.Build<GetTrustFinancialForecastsForCaseRequest>()
			.With(x => x.CaseUrn, caseUrn)
			.Create();

		mockCaseGateWay.Setup(x => x.GetConcernsCaseByUrn(caseUrn, It.IsAny<bool>())).Returns(default(ConcernsCase));

		var sut = new GetTrustFinancialForecastsForCase(mockCaseGateWay.Object, mockTrustFinancialForecastGateway.Object);
		
		// act
		var action = () => sut.Execute(request, CancellationToken.None);

		// assert
		(await action.Should().ThrowAsync<NotFoundException>())
			.And.Message.Should().Be($"Concerns Case {caseUrn} not found");
	}

	[Fact]
	public async Task Execute_When_CaseUrn_Empty_Throws_Exception()
	{
		// arrange
		var mockTrustFinancialForecastGateway = new Mock<ITrustFinancialForecastGateway>();
		var mockCaseGateWay = new Mock<IConcernsCaseGateway>();
		
		var request = new GetTrustFinancialForecastsForCaseRequest();

		var sut = new GetTrustFinancialForecastsForCase(mockCaseGateWay.Object, mockTrustFinancialForecastGateway.Object);
		
		// act
		var action = () => sut.Execute(request, CancellationToken.None);

		// assert
		(await action.Should().ThrowAsync<ArgumentException>())
			.And.Message.Should().Be("Request is not valid (Parameter 'request')");
	}
				
	[Fact]
	public async Task Execute_When_CaseUrn_Invalid_Throws_Exception()
	{
		// arrange
		var mockTrustFinancialForecastGateway = new Mock<ITrustFinancialForecastGateway>();
		var mockCaseGateWay = new Mock<IConcernsCaseGateway>();
		
		var request = new GetTrustFinancialForecastsForCaseRequest { CaseUrn = 0 };

		var sut = new GetTrustFinancialForecastsForCase(mockCaseGateWay.Object, mockTrustFinancialForecastGateway.Object);
		
		// act
		var action = () => sut.Execute(request, CancellationToken.None);

		// assert
		(await action.Should().ThrowAsync<ArgumentException>())
			.And.Message.Should().Be("Request is not valid (Parameter 'request')");
	}

	[Fact]
	public async Task Execute_When_ConcernsCase_Found_Gets_TrustFinancialForecast()
	{
		// arrange
		var mockTrustFinancialForecastGateway = new Mock<ITrustFinancialForecastGateway>();
		var mockCaseGateWay = new Mock<IConcernsCaseGateway>();

		var trustFinancialForecasts = new List<TrustFinancialForecast> { CreateOpenTrustFinancialForecast() };
		var caseUrn = trustFinancialForecasts.First().CaseUrn;
		
		var request = new GetTrustFinancialForecastsForCaseRequest { CaseUrn = caseUrn };
		mockTrustFinancialForecastGateway
			.Setup(x => x.GetAllForCase(
				It.Is<int>(r => r == caseUrn), 
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(trustFinancialForecasts);
		
		mockCaseGateWay.Setup(x => x.CaseExists(caseUrn, It.IsAny<CancellationToken>())).ReturnsAsync(true);

		var sut = new GetTrustFinancialForecastsForCase(mockCaseGateWay.Object, mockTrustFinancialForecastGateway.Object);
		
		// act
		var result = await sut.Execute(request, CancellationToken.None);

		// assert
		result.Should().BeEquivalentTo(trustFinancialForecasts, options => options.ExcludingMissingMembers());
		result.Select(r => r.TrustFinancialForecastId).Should().BeEquivalentTo(trustFinancialForecasts.Select(f => f.Id));
	}

	[Fact]
	public void Constructor_Guards_Against_Null_Arguments()
	{
		_fixture.Customize(new AutoMoqCustomization());
		
		var assertion = _fixture.Create<GuardClauseAssertion>();
		
		assertion.Verify(typeof(GetTrustFinancialForecastsForCase).GetConstructors());
	}

	[Fact]
	public async Task Methods_Guards_Against_Null_Arguments()
	{
		var sut = new GetTrustFinancialForecastsForCase(Mock.Of<IConcernsCaseGateway>(), Mock.Of<ITrustFinancialForecastGateway>());
		
		var action = () => sut.Execute(null, CancellationToken.None);

		(await action.Should().ThrowAsync<ArgumentNullException>())
			.And.Message.Should().Be("Value cannot be null. (Parameter 'request')");
	}

	private TrustFinancialForecast CreateOpenTrustFinancialForecast() => _fixture.Build<TrustFinancialForecast>().Without(x => x.ClosedAt).Create();
}