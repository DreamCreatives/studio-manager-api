using AutoMapper;
using StudioManager.API.Contracts.Equipment;
using StudioManager.Domain.Entities;

namespace StudioManager.Application.Equipments.MapperProjections;

public sealed class EquipmentProjection : Profile
{
    public EquipmentProjection()
    {
        CreateMap<Equipment, EquipmentReadDto>();
    }
}