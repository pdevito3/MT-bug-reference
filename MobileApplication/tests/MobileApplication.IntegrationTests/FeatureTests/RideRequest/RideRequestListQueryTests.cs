namespace MobileApplication.IntegrationTests.FeatureTests.RideRequest
{
    using MobileApplication.Core.Dtos.RideRequest;
    using MobileApplication.SharedTestHelpers.Fakes.RideRequest;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static MobileApplication.WebApi.Features.RideRequests.GetRideRequestList;
    using static TestFixture;

    public class RideRequestListQueryTests : TestBase
    {
        
        [Test]
        public async Task RideRequestListQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            var fakeRideRequestTwo = new FakeRideRequest { }.Generate();
            var queryParameters = new RideRequestParametersDto();

            await InsertAsync(fakeRideRequestOne, fakeRideRequestTwo);

            // Act
            var query = new RideRequestListQuery(queryParameters);
            var rideRequests = await SendAsync(query);

            // Assert
            rideRequests.Should().HaveCount(2);
        }
        
        [Test]
        public async Task RideRequestListQuery_Returns_Expected_Page_Size_And_Number()
        {
            //Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            var fakeRideRequestTwo = new FakeRideRequest { }.Generate();
            var fakeRideRequestThree = new FakeRideRequest { }.Generate();
            var queryParameters = new RideRequestParametersDto() { PageSize = 1, PageNumber = 2 };

            await InsertAsync(fakeRideRequestOne, fakeRideRequestTwo, fakeRideRequestThree);

            //Act
            var query = new RideRequestListQuery(queryParameters);
            var rideRequests = await SendAsync(query);

            // Assert
            rideRequests.Should().HaveCount(1);
        }
        
        [Test]
        public async Task RideRequestListQuery_Returns_Sorted_RideRequest_RideType_List_In_Asc_Order()
        {
            //Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            var fakeRideRequestTwo = new FakeRideRequest { }.Generate();
            fakeRideRequestOne.RideType = "bravo";
            fakeRideRequestTwo.RideType = "alpha";
            var queryParameters = new RideRequestParametersDto() { SortOrder = "RideType" };

            await InsertAsync(fakeRideRequestOne, fakeRideRequestTwo);

            //Act
            var query = new RideRequestListQuery(queryParameters);
            var rideRequests = await SendAsync(query);

            // Assert
            rideRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeRideRequestTwo, options =>
                    options.ExcludingMissingMembers());
            rideRequests
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeRideRequestOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task RideRequestListQuery_Returns_Sorted_RideRequest_RideType_List_In_Desc_Order()
        {
            //Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            var fakeRideRequestTwo = new FakeRideRequest { }.Generate();
            fakeRideRequestOne.RideType = "bravo";
            fakeRideRequestTwo.RideType = "alpha";
            var queryParameters = new RideRequestParametersDto() { SortOrder = "RideType" };

            await InsertAsync(fakeRideRequestOne, fakeRideRequestTwo);

            //Act
            var query = new RideRequestListQuery(queryParameters);
            var rideRequests = await SendAsync(query);

            // Assert
            rideRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeRideRequestTwo, options =>
                    options.ExcludingMissingMembers());
            rideRequests
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeRideRequestOne, options =>
                    options.ExcludingMissingMembers());
        }

        
        [Test]
        public async Task RideRequestListQuery_Filters_RideRequest_RideRequestId()
        {
            //Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            var fakeRideRequestTwo = new FakeRideRequest { }.Generate();
            fakeRideRequestOne.RideRequestId = Guid.NewGuid();
            fakeRideRequestTwo.RideRequestId = Guid.NewGuid();
            var queryParameters = new RideRequestParametersDto() { Filters = $"RideRequestId == {fakeRideRequestTwo.RideRequestId}" };

            await InsertAsync(fakeRideRequestOne, fakeRideRequestTwo);

            //Act
            var query = new RideRequestListQuery(queryParameters);
            var rideRequests = await SendAsync(query);

            // Assert
            rideRequests.Should().HaveCount(1);
            rideRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeRideRequestTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task RideRequestListQuery_Filters_RideRequest_RideType()
        {
            //Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            var fakeRideRequestTwo = new FakeRideRequest { }.Generate();
            fakeRideRequestOne.RideType = "alpha";
            fakeRideRequestTwo.RideType = "bravo";
            var queryParameters = new RideRequestParametersDto() { Filters = $"RideType == {fakeRideRequestTwo.RideType}" };

            await InsertAsync(fakeRideRequestOne, fakeRideRequestTwo);

            //Act
            var query = new RideRequestListQuery(queryParameters);
            var rideRequests = await SendAsync(query);

            // Assert
            rideRequests.Should().HaveCount(1);
            rideRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeRideRequestTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task RideRequestListQuery_Filters_RideRequest_IsEco()
        {
            //Arrange
            var fakeRideRequestOne = new FakeRideRequest { }.Generate();
            var fakeRideRequestTwo = new FakeRideRequest { }.Generate();
            fakeRideRequestOne.IsEco = false;
            fakeRideRequestTwo.IsEco = true;
            var queryParameters = new RideRequestParametersDto() { Filters = $"IsEco == {fakeRideRequestTwo.IsEco}" };

            await InsertAsync(fakeRideRequestOne, fakeRideRequestTwo);

            //Act
            var query = new RideRequestListQuery(queryParameters);
            var rideRequests = await SendAsync(query);

            // Assert
            rideRequests.Should().HaveCount(1);
            rideRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeRideRequestTwo, options =>
                    options.ExcludingMissingMembers());
        }

    }
}