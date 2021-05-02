namespace MobileApplication.FunctionalTests.FunctionalTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetRideRequestTests : TestBase
    {
        [Test]
        public async Task Get_RideRequest_Record_Returns_NoContent()
        {
            // Arrange
            var fakeRideRequest = new FakeRideRequest { }.Generate();

            await InsertAsync(fakeRideRequest);

            // Act
            var route = ApiRoutes.RideRequests.GetRecord.Replace(ApiRoutes.RideRequests.RideRequestId, fakeRideRequest.RideRequestId.ToString());
            var result = await _client.GetRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}