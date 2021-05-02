namespace MobileApplication.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("RideRequest")]
    public class RideRequest
    {
        [Key]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public Guid RideRequestId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string RideType { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public bool IsEco { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}