using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudioManager.API.Base;
using StudioManager.API.Contracts.Pagination;
using StudioManager.API.Contracts.Users;
using StudioManager.Application.Users.Create;
using StudioManager.Application.Users.Delete;
using StudioManager.Application.Users.GetAll;
using StudioManager.Application.Users.GetById;
using StudioManager.Application.Users.Update;
using StudioManager.Domain.Common.Results;
using StudioManager.Domain.Filters.Builders;

namespace StudioManager.API.Controllers.V1;

public sealed class UsersController(ISender sender) : CoreController(sender)
{
    [HttpPost]
    [ProducesResponseType<CommandResult>(StatusCodes.Status200OK)]
    public async Task<IResult> CreateUserAsync([FromBody] UserWriteDto user) => await SendAsync(new CreateUserCommand(user));
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType<CommandResult>(StatusCodes.Status200OK)]
    public async Task<IResult> UpdateUserAsync(Guid id, [FromBody] UserWriteDto user) => await SendAsync(new UpdateUserCommand(id, user));

    [HttpGet("{id:guid}")]
    [ProducesResponseType<UserReadDto>(StatusCodes.Status200OK)]
    public async Task<IResult> GetByIdAsync(Guid id) => await SendAsync(new GetUserByIdQuery(id));
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType<CommandResult>(StatusCodes.Status200OK)]
    public async Task<IResult> DeleteUserAsync(Guid id) => await SendAsync(new DeleteUserCommand(id));
    
    [HttpGet]
    [ProducesResponseType<PagingResultDto<UserReadDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetAllUsersAsync(
        [FromQuery] PaginationDto pagination,
        [FromQuery] string? ft)
    {
        var filter = UserFilterBuilder.New()
            .WithSearch(ft)
            .Build();

        return await SendAsync(new GetAllUsersQuery(filter, pagination));
    }
}
