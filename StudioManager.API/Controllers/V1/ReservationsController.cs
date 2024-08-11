using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudioManager.API.Base;
using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Application.Reservations.Create;
using StudioManager.Application.Reservations.Delete;
using StudioManager.Application.Reservations.GetAll;
using StudioManager.Application.Reservations.GetById;
using StudioManager.Application.Reservations.Update;
using StudioManager.Domain.Filters;
using StudioManager.Infrastructure.Common.Results;

namespace StudioManager.API.Controllers.V1;

[ExcludeFromCodeCoverage]
public sealed class ReservationsController(ISender sender) : CoreController(sender)
{
    [HttpPost]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> CreateReservation([FromBody] ReservationWriteDto reservation)
    {
        return await SendAsync(new CreateReservationCommand(reservation));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> UpdateReservation(Guid id, [FromBody] ReservationWriteDto reservation)
    {
        return await SendAsync(new UpdateReservationCommand(id, reservation));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> DeleteReservation(Guid id)
    {
        return await SendAsync(new DeleteReservationCommand(id));
    }

    [HttpGet]
    [ProducesResponseType(typeof(QueryResult<PagingResultDto<ReservationReadDto>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetAllReservations(
        [FromQuery] string? ft,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] PaginationDto pagination)
    {
        var filter = new ReservationFilter
        {
            Search = ft,
            StartDate = startDate,
            EndDate = endDate
        };

        return await SendAsync(new GetAllReservationsQuery(filter, pagination));
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(QueryResult<ReservationReadDto>), StatusCodes.Status200OK)]
    public async Task<IResult> GetReservationById(Guid id)
    {
        return await SendAsync(new GetReservationByIdQuery(id));
    }
}
