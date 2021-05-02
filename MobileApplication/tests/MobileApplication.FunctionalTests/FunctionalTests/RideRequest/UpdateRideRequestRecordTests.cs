namespace MobileApplication.FunctionalTests.FunctionalTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class UpdateRideRequestRecordTests : TestBase
    {
        [Test]
        public async Task Put_RideRequest_Returns_NoContent()
        {
            // Arrange
            var fakeRideRequest = new FakeRideRequest { }.Generate();
            var updatedRideRequestDto = new FakeRideRequestForUpdateDto { }.Generate();

            await InsertAsync(fakeRideRequest);

            // Act
            var route = ApiRoutes.RideRequests.Put.Replace(ApiRoutes.RideRequests.RideRequestId, fakeRideRequest.RideRequestId.ToString());
            var result = await _client.PutJsonRequestAsync(route, updatedRideRequestDto);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}