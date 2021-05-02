namespace MobileApplication.WebApi.Features.RideRequests
{
    using MobileApplication.Core.Entities;
    using MobileApplication.Core.Dtos.RideRequest;
    using MobileApplication.Core.Exceptions;
    using MobileApplication.Infrastructure.Contexts;
    using MobileApplication.WebApi.Features.RideRequests.Validators;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class AddRideRequest
    {
        public class AddRideRequestCommand : IRequest<RideRequestDto>
        {
            public RideRequestForCreationDto RideRequestToAdd { get; set; }

            public AddRideRequestCommand(RideRequestForCreationDto rideRequestToAdd)
            {
                RideRequestToAdd = rideRequestToAdd;
            }
        }

        public class CustomCreateRideRequestValidation : RideRequestForManipulationDtoValidator<RideRequestForCreationDto>
        {
            public CustomCreateRideRequestValidation()
            {
            }
        }

        public class Handler : IRequestHandler<AddRideRequestCommand, RideRequestDto>
        {
            private readonly MobileApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(MobileApplicationDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<RideRequestDto> Handle(AddRideRequestCommand request, CancellationToken cancellationToken)
            {
                if (await _db.RideRequests.AnyAsync(r => r.RideRequestId == request.RideRequestToAdd.RideRequestId))
                {
                    throw new ConflictException("RideRequest already exists with this primary key.");
                }

                var rideRequest = _mapper.Map<RideRequest> (request.RideRequestToAdd);
                _db.RideRequests.Add(rideRequest);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (saveSuccessful)
                {
                    return await _db.RideRequests
                        .ProjectTo<RideRequestDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(r => r.RideRequestId == rideRequest.RideRequestId);
                }
                else
                {
                    // add log
                    throw new Exception("Unable to save the new record. Please check the logs for more information.");
                }
            }
        }
    }
}