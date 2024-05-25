using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudioManager.API.Base;
using StudioManager.API.Contracts.Equipments;
using StudioManager.API.Contracts.Pagination;
using StudioManager.Application.Equipments.Create;
using StudioManager.Application.Equipments.Delete;
using StudioManager.Application.Equipments.GetAll;
using StudioManager.Application.Equipments.GetById;
using StudioManager.Application.Equipments.Update;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;

namespace StudioManager.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/[controller]")]
[ExcludeFromCodeCoverage]
public class EquipmentsController(ISender sender) : CoreController(sender)
{
    [HttpPost]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> CreateEquipment([FromBody] EquipmentWriteDto dto)
    {
        var command = new CreateEquipmentCommand(dto);
        return await SendAsync(command);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> DeleteEquipment(Guid id)
    {
        var command = new DeleteEquipmentCommand(id);
        return await SendAsync(command);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> UpdateEquipment(Guid id, [FromBody] EquipmentWriteDto dto)
    {
        var command = new UpdateEquipmentCommand(id, dto);
        return await SendAsync(command);
    }

    [HttpGet]
    [ProducesResponseType(typeof(QueryResult<PagingResultDto<EquipmentReadDto>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetEquipments(
        [FromQuery] PaginationDto pagination,
        [FromQuery] IEnumerable<Guid> equipmentTypes,
        [FromQuery] string? ft)
    {
        var filter = CreateFilter(ft, equipmentTypes);
        var command = new GetAllEquipmentsQuery(filter, pagination);
        return await SendAsync(command);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(QueryResult<EquipmentReadDto>), StatusCodes.Status200OK)]
    public async Task<IResult> GetEquipmentById(Guid id)
    {
        return await SendAsync(new GetEquipmentByIdQuery(id));
    }

    private static EquipmentFilter CreateFilter(
        string? search,
        IEnumerable<Guid> equipmentTypes)
    {
        return new EquipmentFilter
        {
            Search = search,
            EquipmentTypeIds = equipmentTypes
        };
    }
}
