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
    using MassTransit;
    using Messages;

    public class MakeRideTypeRequest
    {
        public class MakeRideTypeRequestCommand : IRequest<RideRequestDto>
        {
            public RideRequestForCreationDto RideRequestToAdd { get; set; }

            public MakeRideTypeRequestCommand(RideRequestForCreationDto rideRequestToAdd)
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

        public class Handler : IRequestHandler<MakeRideTypeRequestCommand, RideRequestDto>
        {
            private readonly MobileApplicationDbContext _db;
            private readonly IMapper _mapper;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(MobileApplicationDbContext db, IMapper mapper, IPublishEndpoint publishEndpoint)
            {
                _mapper = mapper;
                _db = db;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<RideRequestDto> Handle(MakeRideTypeRequestCommand request, CancellationToken cancellationToken)
            {
                if (await _db.RideRequests.AnyAsync(r => r.RideRequestId == request.RideRequestToAdd.RideRequestId))
                {
                    throw new ConflictException("RideRequest already exists with this primary key.");
                }

                var rideRequest = _mapper.Map<RideRequest>(request.RideRequestToAdd);
                _db.RideRequests.Add(rideRequest);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (saveSuccessful)
                {
                    await _publishEndpoint.Publish<IRideTypeRequested>(new
                    {
                        rideRequest.RideType,
                        RequestId = rideRequest.RideRequestId.ToString(),
                    });

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