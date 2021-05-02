namespace MobileApplication.Core.Dtos.RideRequest
{
    using MobileApplication.Core.Dtos.Shared;

    public class RideRequestParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}