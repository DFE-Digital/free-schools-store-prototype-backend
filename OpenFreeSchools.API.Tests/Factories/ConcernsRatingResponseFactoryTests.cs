using OpenFreeSchools.API.Factories;
using OpenFreeSchools.API.ResponseModels;
using OpenFreeSchools.Data.Models;
using System;
using FluentAssertions;
using Xunit;

namespace OpenFreeSchools.API.Tests.Factories
{
    public class ConcernsRatingResponseFactoryTests
    {
        [Fact]
        public void ReturnsConcernsRatingResponse_WhenGivenAnConcernsRating()
        {
            var concernsRating = new ConcernsRating
            {
                Id = 5,
                Name = "Test concerns rating",
                CreatedAt = new DateTime(2021, 10,07),
                UpdatedAt = new DateTime(2021, 10,07)
            };

            var expected = new ConcernsRatingResponse
            {
                Name = concernsRating.Name,
                CreatedAt = concernsRating.CreatedAt,
                UpdatedAt = concernsRating.UpdatedAt,
                Id = concernsRating.Id
            };

            var result = ConcernsRatingResponseFactory.Create(concernsRating);
            result.Should().BeEquivalentTo(expected);
        }
    }
}