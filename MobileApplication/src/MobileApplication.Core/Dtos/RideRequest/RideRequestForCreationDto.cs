namespace MobileApplication.Core.Dtos.RideRequest
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RideRequestForCreationDto : RideRequestForManipulationDto
    {
        public Guid RideRequestId { get; set; } = Guid.NewGuid();
        public override string RideType { get; set; } = "small";

        // add-on property marker - Do Not Delete This Comment
    }
}