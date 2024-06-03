using AutoMapper;
using StudioManager.API.Contracts.Users;
using StudioManager.Domain.Entities;

namespace StudioManager.Application.Users.MapperProjections;

public sealed class UserProjection : Profile
{
    public UserProjection()
    {
        CreateMap<User, UserReadDto>();
    }
}
