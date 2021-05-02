namespace MobileApplication.IntegrationTests.FeatureTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static MobileApplication.WebApi.Features.RideRequests.DeleteRideRequest;
    using static TestFixture;

    public class DeleteRideRequestCommandTests : TestBase
    {
        [Test]
        public async Task DeleteRideRequestCommand_Deletes_RideRequest_From_Db()
        {
            // Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            await InsertAsync(fakeRideRequestOne);
            var rideRequest = await ExecuteDbContextAsync(db => db.RideRequests.SingleOrDefaultAsync());
            var rideRequestId = rideRequest.RideRequestId;

            // Act
            var command = new DeleteRideRequestCommand(rideRequestId);
            await SendAsync(command);
            var rideRequests = await ExecuteDbContextAsync(db => db.RideRequests.ToListAsync());

            // Assert
            rideRequests.Count.Should().Be(0);
        }
    }
}