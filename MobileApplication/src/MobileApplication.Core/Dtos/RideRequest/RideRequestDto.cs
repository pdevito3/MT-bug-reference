namespace MobileApplication.Core.Dtos.RideRequest
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RideRequestDto 
    {
        public Guid RideRequestId { get; set; }
        public string RideType { get; set; }        public bool IsEco { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}