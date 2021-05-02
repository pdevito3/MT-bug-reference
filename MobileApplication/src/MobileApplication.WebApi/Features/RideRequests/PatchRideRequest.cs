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
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class PatchRideRequest
    {
        public class PatchRideRequestCommand : IRequest<bool>
        {
            public Guid RideRequestId { get; set; }
            public JsonPatchDocument<RideRequestForUpdateDto> PatchDoc { get; set; }

            public PatchRideRequestCommand(Guid rideRequest, JsonPatchDocument<RideRequestForUpdateDto> patchDoc)
            {
                RideRequestId = rideRequest;
                PatchDoc = patchDoc;
            }
        }

        public class CustomPatchRideRequestValidation : RideRequestForManipulationDtoValidator<RideRequestForUpdateDto>
        {
            public CustomPatchRideRequestValidation()
            {
            }
        }

        public class Handler : IRequestHandler<PatchRideRequestCommand, bool>
        {
            private readonly MobileApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(MobileApplicationDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(PatchRideRequestCommand request, CancellationToken cancellationToken)
            {
                // add logger or use decorator
                if (request.PatchDoc == null)
                {
                    // log error
                    throw new ApiException("Invalid patch document.");
                }

                var rideRequestToUpdate = await _db.RideRequests
                    .FirstOrDefaultAsync(r => r.RideRequestId == request.RideRequestId);

                if (rideRequestToUpdate == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                var rideRequestToPatch = _mapper.Map<RideRequestForUpdateDto>(rideRequestToUpdate); // map the rideRequest we got from the database to an updatable rideRequest model
                request.PatchDoc.ApplyTo(rideRequestToPatch); // apply patchdoc updates to the updatable rideRequest

                var validationResults = new CustomPatchRideRequestValidation().Validate(rideRequestToPatch);
                if (!validationResults.IsValid)
                {
                    throw new ValidationException(validationResults.Errors);
                }

                _mapper.Map(rideRequestToPatch, rideRequestToUpdate);
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