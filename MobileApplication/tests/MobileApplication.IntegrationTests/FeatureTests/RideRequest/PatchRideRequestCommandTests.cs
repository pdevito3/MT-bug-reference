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
    using static MobileApplication.WebApi.Features.RideRequests.PatchRideRequest;
    using static TestFixture;

    public class PatchRideRequestCommandTests : TestBase
    {
        [Test]
        public async Task PatchRideRequestCommand_Updates_Existing_RideRequest_In_Db()
        {
            // Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            await InsertAsync(fakeRideRequestOne);
            var rideRequest = await ExecuteDbContextAsync(db => db.RideRequests.SingleOrDefaultAsync());
            var rideRequestId = rideRequest.RideRequestId;

            var patchDoc = new JsonPatchDocument<RideRequestForUpdateDto>();
            var newValue = "Easily Identified Value For Test";
            patchDoc.Replace(r => r.RideType, newValue);

            // Act
            var command = new PatchRideRequestCommand(rideRequestId, patchDoc);
            await SendAsync(command);
            var updatedRideRequest = await ExecuteDbContextAsync(db => db.RideRequests.Where(r => r.RideRequestId == rideRequestId).SingleOrDefaultAsync());

            // Assert
            updatedRideRequest.RideType.Should().Be(newValue);
        }
    }
}