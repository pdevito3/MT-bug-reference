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

    public class PublishSubmitOrder
    {
        public class PublishSubmitOrderCommand : IRequest<Guid>
        {
            public string CustomerType { get; set; }

            public PublishSubmitOrderCommand(string customerType)
            {
                CustomerType = customerType;
            }
        }

        public class Handler : IRequestHandler<PublishSubmitOrderCommand, Guid>
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

            public async Task<Guid> Handle(PublishSubmitOrderCommand request, CancellationToken cancellationToken)
            {
                var guid = Guid.NewGuid();
                await _publishEndpoint.Publish<ISubmitOrder>(new
                {
                    request.CustomerType,
                    TrasactionId = guid
                });

                return guid;
            }
        }
    }
}