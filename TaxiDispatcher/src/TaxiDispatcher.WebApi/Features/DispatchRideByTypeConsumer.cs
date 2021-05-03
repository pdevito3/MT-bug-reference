using MassTransit;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace TaxiDispatcher.WebApi.Features
{
    public class DispatchRideByTypeConsumer :
        IConsumer<IRideTypeRequested>
    {
        public async Task Consume(ConsumeContext<IRideTypeRequested> context)
        {
            var consumption = "test";
        }
    }
}