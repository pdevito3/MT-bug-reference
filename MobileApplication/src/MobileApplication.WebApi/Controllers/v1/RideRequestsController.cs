namespace MobileApplication.WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using MobileApplication.Core.Dtos.RideRequest;
    using MobileApplication.Core.Wrappers;
    using System.Threading;
    using MediatR;
    using static MobileApplication.WebApi.Features.RideRequests.GetRideRequestList;
    using static MobileApplication.WebApi.Features.RideRequests.GetRideRequest;
    using static MobileApplication.WebApi.Features.RideRequests.AddRideRequest;
    using static MobileApplication.WebApi.Features.RideRequests.DeleteRideRequest;
    using static MobileApplication.WebApi.Features.RideRequests.UpdateRideRequest;
    using static MobileApplication.WebApi.Features.RideRequests.PatchRideRequest;

    [ApiController]
    [Route("api/RideRequests")]
    [ApiVersion("1.0")]
    public class RideRequestsController: ControllerBase
    {
        private readonly IMediator _mediator;

        public RideRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Gets a list of all RideRequests.
        /// </summary>
        /// <response code="200">RideRequest list returned successfully.</response>
        /// <response code="400">RideRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the RideRequest.</response>
        /// <remarks>
        /// Requests can be narrowed down with a variety of query string values:
        /// ## Query String Parameters
        /// - **PageNumber**: An integer value that designates the page of records that should be returned.
        /// - **PageSize**: An integer value that designates the number of records returned on the given page that you would like to return. This value is capped by the internal MaxPageSize.
        /// - **SortOrder**: A comma delimited ordered list of property names to sort by. Adding a `-` before the name switches to sorting descendingly.
        /// - **Filters**: A comma delimited list of fields to filter by formatted as `{Name}{Operator}{Value}` where
        ///     - {Name} is the name of a filterable property. You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if LikeCount or CommentCount is >10
        ///     - {Operator} is one of the Operators below
        ///     - {Value} is the value to use for filtering. You can also have multiple values (for OR logic) by using a pipe delimiter, eg.`Title@= new|hot` will return posts with titles that contain the text "new" or "hot"
        ///
        ///    | Operator | Meaning                       | Operator  | Meaning                                      |
        ///    | -------- | ----------------------------- | --------- | -------------------------------------------- |
        ///    | `==`     | Equals                        |  `!@=`    | Does not Contains                            |
        ///    | `!=`     | Not equals                    |  `!_=`    | Does not Starts with                         |
        ///    | `>`      | Greater than                  |  `@=*`    | Case-insensitive string Contains             |
        ///    | `&lt;`   | Less than                     |  `_=*`    | Case-insensitive string Starts with          |
        ///    | `>=`     | Greater than or equal to      |  `==*`    | Case-insensitive string Equals               |
        ///    | `&lt;=`  | Less than or equal to         |  `!=*`    | Case-insensitive string Not equals           |
        ///    | `@=`     | Contains                      |  `!@=*`   | Case-insensitive string does not Contains    |
        ///    | `_=`     | Starts with                   |  `!_=*`   | Case-insensitive string does not Starts with |
        /// </remarks>
        [ProducesResponseType(typeof(Response<IEnumerable<RideRequestDto>>), 200)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetRideRequests")]
        public async Task<IActionResult> GetRideRequests([FromQuery] RideRequestParametersDto rideRequestParametersDto)
        {
            // add error handling
            var query = new RideRequestListQuery(rideRequestParametersDto);
            var queryResponse = await _mediator.Send(query);

            var paginationMetadata = new
            {
                totalCount = queryResponse.TotalCount,
                pageSize = queryResponse.PageSize,
                currentPageSize = queryResponse.CurrentPageSize,
                currentStartIndex = queryResponse.CurrentStartIndex,
                currentEndIndex = queryResponse.CurrentEndIndex,
                pageNumber = queryResponse.PageNumber,
                totalPages = queryResponse.TotalPages,
                hasPrevious = queryResponse.HasPrevious,
                hasNext = queryResponse.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var response = new Response<IEnumerable<RideRequestDto>>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Gets a single RideRequest by ID.
        /// </summary>
        /// <response code="200">RideRequest record returned successfully.</response>
        /// <response code="400">RideRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the RideRequest.</response>
        [ProducesResponseType(typeof(Response<RideRequestDto>), 200)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{rideRequestId}", Name = "GetRideRequest")]
        public async Task<ActionResult<RideRequestDto>> GetRideRequest(Guid rideRequestId)
        {
            // add error handling
            var query = new RideRequestQuery(rideRequestId);
            var queryResponse = await _mediator.Send(query);

            var response = new Response<RideRequestDto>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Creates a new RideRequest record.
        /// </summary>
        /// <response code="201">RideRequest created.</response>
        /// <response code="400">RideRequest has missing/invalid values.</response>
        /// <response code="409">A record already exists with this primary key.</response>
        /// <response code="500">There was an error on the server while creating the RideRequest.</response>
        [ProducesResponseType(typeof(Response<RideRequestDto>), 201)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(typeof(Response<>), 409)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<RideRequestDto>> AddRideRequest([FromBody]RideRequestForCreationDto rideRequestForCreation)
        {
            // add error handling
            var command = new AddRideRequestCommand(rideRequestForCreation);
            var commandResponse = await _mediator.Send(command);
            var response = new Response<RideRequestDto>(commandResponse);

            return CreatedAtRoute("GetRideRequest",
                new { commandResponse.RideRequestId },
                response);
        }
        
        /// <summary>
        /// Deletes an existing RideRequest record.
        /// </summary>
        /// <response code="204">RideRequest deleted.</response>
        /// <response code="400">RideRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the RideRequest.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{rideRequestId}")]
        public async Task<ActionResult> DeleteRideRequest(Guid rideRequestId)
        {
            // add error handling
            var command = new DeleteRideRequestCommand(rideRequestId);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates an entire existing RideRequest.
        /// </summary>
        /// <response code="204">RideRequest updated.</response>
        /// <response code="400">RideRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the RideRequest.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{rideRequestId}")]
        public async Task<IActionResult> UpdateRideRequest(Guid rideRequestId, RideRequestForUpdateDto rideRequest)
        {
            // add error handling
            var command = new UpdateRideRequestCommand(rideRequestId, rideRequest);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates specific properties on an existing RideRequest.
        /// </summary>
        /// <response code="204">RideRequest updated.</response>
        /// <response code="400">RideRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the RideRequest.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{rideRequestId}")]
        public async Task<IActionResult> PartiallyUpdateRideRequest(Guid rideRequestId, JsonPatchDocument<RideRequestForUpdateDto> patchDoc)
        {
            // add error handling
            var command = new PatchRideRequestCommand(rideRequestId, patchDoc);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}