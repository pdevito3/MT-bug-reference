using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public interface IRideTypeRequested
    {
        Guid RequestId { get; set; }
        string RideType { get; set; }
    }
}