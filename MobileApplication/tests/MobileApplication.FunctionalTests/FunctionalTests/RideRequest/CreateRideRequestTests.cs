namespace MobileApplication.FunctionalTests.FunctionalTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CreateRideRequestTests : TestBase
    {
        [Test]
        public async Task Create_RideRequest_Returns_Created()
        {
            // Arrange
            var fakeRideRequest = new FakeRideRequest { }.Generate();

            await InsertAsync(fakeRideRequest);

            // Act
            var route = ApiRoutes.RideRequests.Create;
            var result = await _client.PostJsonRequestAsync(route, fakeRideRequest);

            // Assert
            result.StatusCode.Should().Be(201);
        }
    }
}