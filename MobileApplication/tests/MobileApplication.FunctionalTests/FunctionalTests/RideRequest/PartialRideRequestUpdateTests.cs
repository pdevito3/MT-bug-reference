namespace MobileApplication.FunctionalTests.FunctionalTests.RideRequest
{
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using MobileApplication.Core.Dtos.RideRequest;
    using MobileApplication.FunctionalTests.TestUtilities;
    using Microsoft.AspNetCore.JsonPatch;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PartialRideRequestUpdateTests : TestBase
    {
        [Test]
        public async Task Patch_RideRequest_Returns_NoContent()
        {
            // Arrange
            var fakeRideRequest = new FakeRideRequest { }.Generate();
            var patchDoc = new JsonPatchDocument<RideRequestForUpdateDto>();
            patchDoc.Replace(r => r.RideType, "Easily Identified Value For Test");

            await InsertAsync(fakeRideRequest);

            // Act
            var route = ApiRoutes.RideRequests.Patch.Replace(ApiRoutes.RideRequests.RideRequestId, fakeRideRequest.RideRequestId.ToString());
            var result = await _client.PatchJsonRequestAsync(route, patchDoc);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}