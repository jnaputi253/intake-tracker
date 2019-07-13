using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using IntakeTracker.Infrastructure;

namespace IntakeTracker.Tests
{
    public static class TestUtil
    {
        public static void AssertCollectionSize<TEntity>(Response response, int expectedSize)
        {
            response.Data.Should().BeAssignableTo<IEnumerable<TEntity>>()
                .Which.Count().Should().Be(expectedSize);
        }

        public static void AssertResponseStatus(Response response, string message, HttpStatusCode statusCode)
        {
            response.Should().Match<Response>(testResponse =>
                testResponse.Message == message &&
                testResponse.StatusCode == statusCode);
        }
    }
}
