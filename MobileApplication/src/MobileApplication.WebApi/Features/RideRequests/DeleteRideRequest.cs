namespace MobileApplication.WebApi.Features.RideRequests
{
    using MobileApplication.Core.Entities;
    using MobileApplication.Core.Dtos.RideRequest;
    using MobileApplication.Core.Exceptions;
    using MobileApplication.Infrastructure.Contexts;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class DeleteRideRequest
    {
        public class DeleteRideRequestCommand : IRequest<bool>
        {
            public Guid RideRequestId { get; set; }

            public DeleteRideRequestCommand(Guid rideRequest)
            {
                RideRequestId = rideRequest;
            }
        }

        public class Handler : IRequestHandler<DeleteRideRequestCommand, bool>
        {
            private readonly MobileApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(MobileApplicationDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(DeleteRideRequestCommand request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var recordToDelete = await _db.RideRequests
                    .FirstOrDefaultAsync(r => r.RideRequestId == request.RideRequestId);

                if (recordToDelete == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                _db.RideRequests.Remove(recordToDelete);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (!saveSuccessful)
                {
                    // add log
                    throw new Exception("Unable to save the new record. Please check the logs for more information.");
                }

                return true;
            }
        }
    }
}