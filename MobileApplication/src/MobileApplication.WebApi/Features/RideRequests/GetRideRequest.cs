namespace MobileApplication.WebApi.Features.RideRequests
{
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

    public class GetRideRequest
    {
        public class RideRequestQuery : IRequest<RideRequestDto>
        {
            public Guid RideRequestId { get; set; }

            public RideRequestQuery(Guid rideRequestId)
            {
                RideRequestId = rideRequestId;
            }
        }

        public class Handler : IRequestHandler<RideRequestQuery, RideRequestDto>
        {
            private readonly MobileApplicationDbContext _db;
            private readonly IMapper _mapper;

            public Handler(MobileApplicationDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<RideRequestDto> Handle(RideRequestQuery request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var result = await _db.RideRequests
                    .ProjectTo<RideRequestDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(r => r.RideRequestId == request.RideRequestId);

                if (result == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                return result;
            }
        }
    }
}