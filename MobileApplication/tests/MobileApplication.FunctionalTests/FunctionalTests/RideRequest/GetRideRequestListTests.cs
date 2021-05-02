namespace MobileApplication.FunctionalTests.FunctionalTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetRideRequestListTests : TestBase
    {
        [Test]
        public async Task Get_RideRequest_List_Returns_NoContent()
        {
            // Arrange
            // N/A

            // Act
            var result = await _client.GetRequestAsync(ApiRoutes.RideRequests.GetList);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}