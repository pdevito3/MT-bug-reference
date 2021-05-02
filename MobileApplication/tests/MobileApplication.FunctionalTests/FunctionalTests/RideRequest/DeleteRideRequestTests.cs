namespace MobileApplication.FunctionalTests.FunctionalTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class DeleteRideRequestTests : TestBase
    {
        [Test]
        public async Task Delete_RideRequestReturns_NoContent()
        {
            // Arrange
            var fakeRideRequest = new FakeRideRequest { }.Generate();

            await InsertAsync(fakeRideRequest);

            // Act
            var route = ApiRoutes.RideRequests.Delete.Replace(ApiRoutes.RideRequests.RideRequestId, fakeRideRequest.RideRequestId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}