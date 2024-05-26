using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudioManager.API.Base;
using StudioManager.API.Contracts.Common;
using StudioManager.Application.Dropdowns.Equipments;
using StudioManager.Application.Dropdowns.EquipmentTypes;

namespace StudioManager.API.Controllers.V1;

[ExcludeFromCodeCoverage]
public class DropdownsController(ISender sender) : CoreController(sender)
{
    [HttpGet("Equipments")]
    [ProducesResponseType(typeof(IReadOnlyList<NamedBaseDto>), StatusCodes.Status200OK)]
    public async Task<IResult> GetDropdownEquipments(
        [FromQuery] string? ft)
    {
        return await SendAsync(new GetEquipmentsForDropdownQuery(ft));
    }
    
    [HttpGet("Equipment/Types")]
    [ProducesResponseType(typeof(IReadOnlyList<NamedBaseDto>), StatusCodes.Status200OK)]
    public async Task<IResult> GetDropdownEquipmentTypes(
        [FromQuery] string? ft)
    {
        return await SendAsync(new GetEquipmentTypesForDropdownQuery(ft));
    }
}
