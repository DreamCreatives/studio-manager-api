using AutoMapper;
using StudioManager.API.Contracts.Common;
using StudioManager.API.Contracts.Equipments;
using StudioManager.Domain.Entities;

namespace StudioManager.Application.Equipments.MapperProjections;

public sealed class EquipmentProjection : Profile
{
    public EquipmentProjection()
    {
        CreateMap<Equipment, EquipmentReadDto>();
        CreateMap<Equipment, NamedBaseDto>();
    }
}
