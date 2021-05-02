namespace MobileApplication.WebApi.Features.RideRequests.Validators
{
    using MobileApplication.Core.Dtos.RideRequest;
    using FluentValidation;
    using System;

    public class RideRequestForManipulationDtoValidator<T> : AbstractValidator<T> where T : RideRequestForManipulationDto
    {
        public RideRequestForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}