namespace MobileApplication.WebApi.Features.RideRequests.Mappings
{
    using MobileApplication.Core.Dtos.RideRequest;
    using AutoMapper;
    using MobileApplication.Core.Entities;

    public class RideRequestProfile : Profile
    {
        public RideRequestProfile()
        {
            //createmap<to this, from this>
            CreateMap<RideRequest, RideRequestDto>()
                .ReverseMap();
            CreateMap<RideRequestForCreationDto, RideRequest>();
            CreateMap<RideRequestForUpdateDto, RideRequest>()
                .ReverseMap();
        }
    }
}