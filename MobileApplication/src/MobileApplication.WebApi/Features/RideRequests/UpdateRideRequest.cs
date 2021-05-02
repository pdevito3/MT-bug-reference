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

    public class UpdateRideRequest
    {
        public class UpdateRideRequestCommand : IRequest<bool>
        {
            public Guid RideRequestId { get; set; }
            public RideRequestForUpdateDto RideRequestToUpdate { get; set; }

            public UpdateRideRequestCommand(Guid rideRequest, RideRequestForUpdateDto rideRequestToUpdate)
            {
                RideRequestId = rideRequest;
                RideRequestToUpdate = rideRequestToUpdate;
            }
        }

        public class CustomUpdateRideRequestValidation : RideRequestForManipulationDtoValidator<RideRequestForUpdateDto>
        {
            public CustomUpdateRideRequestValidation()
            {
            }
        }

        public class Handler : IRequestHandler<UpdateRideRequestCommand, bool>
        {
            private readonly MobileApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(MobileApplicationDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(UpdateRideRequestCommand request, CancellationToken cancellationToken)
            {
                // add logger or use decorator

                var recordToUpdate = await _db.RideRequests
                    .FirstOrDefaultAsync(r => r.RideRequestId == request.RideRequestId);

                if (recordToUpdate == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                _mapper.Map(request.RideRequestToUpdate, recordToUpdate);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (!saveSuccessful)
                {
                    // add log
                    throw new Exception("Unable to save the requested changes. Please check the logs for more information.");
                }

                return true;
            }
        }
    }
}