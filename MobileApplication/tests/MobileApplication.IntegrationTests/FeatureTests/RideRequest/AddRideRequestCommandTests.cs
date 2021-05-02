namespace MobileApplication.IntegrationTests.FeatureTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static MobileApplication.WebApi.Features.RideRequests.AddRideRequest;
    using static TestFixture;

    public class AddRideRequestCommandTests : TestBase
    {
        [Test]
        public async Task AddRideRequestCommand_Adds_New_RideRequest_To_Db()
        {
            // Arrange
            var fakeRideRequestOne = new FakeRideRequestForCreationDto { }.Generate();

            // Act
            var command = new AddRideRequestCommand(fakeRideRequestOne);
            var rideRequestReturned = await SendAsync(command);
            var rideRequestCreated = await ExecuteDbContextAsync(db => db.RideRequests.SingleOrDefaultAsync());

            // Assert
            rideRequestReturned.Should().BeEquivalentTo(fakeRideRequestOne, options =>
                options.ExcludingMissingMembers());
            rideRequestCreated.Should().BeEquivalentTo(fakeRideRequestOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}