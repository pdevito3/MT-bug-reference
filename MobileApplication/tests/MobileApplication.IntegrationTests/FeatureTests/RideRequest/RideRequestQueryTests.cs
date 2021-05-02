namespace MobileApplication.IntegrationTests.FeatureTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static MobileApplication.WebApi.Features.RideRequests.GetRideRequest;
    using static TestFixture;

    public class RideRequestQueryTests : TestBase
    {
        [Test]
        public async Task RideRequestQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            await InsertAsync(fakeRideRequestOne);

            // Act
            var query = new RideRequestQuery(fakeRideRequestOne.RideRequestId);
            var rideRequests = await SendAsync(query);

            // Assert
            rideRequests.Should().BeEquivalentTo(fakeRideRequestOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}