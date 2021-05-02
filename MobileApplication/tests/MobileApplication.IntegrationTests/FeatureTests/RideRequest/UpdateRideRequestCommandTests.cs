namespace MobileApplication.IntegrationTests.FeatureTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.IntegrationTests.TestUtilities;
    using MobileApplication.Core.Dtos.RideRequest;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using static MobileApplication.WebApi.Features.RideRequests.UpdateRideRequest;
    using static TestFixture;

    public class UpdateRideRequestCommandTests : TestBase
    {
        [Test]
        public async Task UpdateRideRequestCommand_Updates_Existing_RideRequest_In_Db()
        {
            // Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            var updatedRideRequestDto = new FakeRideRequestForUpdateDto { }.Generate();
            await InsertAsync(fakeRideRequestOne);

            var rideRequest = await ExecuteDbContextAsync(db => db.RideRequests.SingleOrDefaultAsync());
            var rideRequestId = rideRequest.RideRequestId;

            // Act
            var command = new UpdateRideRequestCommand(rideRequestId, updatedRideRequestDto);
            await SendAsync(command);
            var updatedRideRequest = await ExecuteDbContextAsync(db => db.RideRequests.Where(r => r.RideRequestId == rideRequestId).SingleOrDefaultAsync());

            // Assert
            updatedRideRequest.Should().BeEquivalentTo(updatedRideRequestDto, options =>
                options.ExcludingMissingMembers());
        }
    }
}