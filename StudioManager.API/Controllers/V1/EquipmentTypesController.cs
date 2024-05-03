using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudioManager.API.Base;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Application.EquipmentTypes.Create;
using StudioManager.Application.EquipmentTypes.GetAll;
using StudioManager.Application.EquipmentTypes.Update;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters;

namespace StudioManager.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/Equipment/Types")]
public class EquipmentTypesController(ISender sender) : CoreController(sender)
{
    [HttpPost]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> CreateEquipmentTypeAsync([FromBody] EquipmentTypeWriteDto dto)
    {
        var command = new CreateEquipmentTypeCommand(dto);
        return await SendAsync(command);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> UpdateEquipmentTypeAsync(
        Guid id,
        [FromBody] EquipmentTypeWriteDto dto)
    {
        var command = new UpdateEquipmentTypeCommand(id, dto);
        return await SendAsync(command);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EquipmentTypeReadDto>), StatusCodes.Status200OK)]
    public async Task<IResult> GetEquipmentTypesAsync(
        [FromQuery] string? ft = null)
    {
        var filter = CreateFilter(ft);
        var query = new GetEquipmentTypesQuery { Filter = filter };
        return await SendAsync(query);
    }
    
    private static EquipmentTypeFilter CreateFilter(string? ft)
    {
        return new EquipmentTypeFilter
        {
            Name = ft
        };
    }
}