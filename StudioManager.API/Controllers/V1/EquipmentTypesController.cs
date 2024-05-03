using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudioManager.API.Base;
using StudioManager.Domain.Common;

namespace StudioManager.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/Equipment/Types")]
public class EquipmentTypesController(ISender sender) : CoreController(sender)
{
    [HttpGet("{something}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IResult Get(string something)
    {
        var result = QueryResult.Success(something);
        return FromSucceededResult(result);
    }
}