using AutoMapper;
using StudioManager.API.Contracts.EquipmentTypes;
using StudioManager.Domain.Entities;

namespace StudioManager.Application.EquipmentTypes.MapperProjections;

public sealed class EquipmentTypeProjection : Profile
{
    public EquipmentTypeProjection()
    {
        CreateMap<EquipmentType, EquipmentTypeReadDto>();
    }
}
