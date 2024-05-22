using AutoMapper;
using StudioManager.API.Contracts.Reservations;
using StudioManager.Domain.Entities;

namespace StudioManager.Application.Reservations.MapperProjections;

public sealed class ReservationProjection : Profile
{
    public ReservationProjection()
    {
        CreateMap<Reservation, ReservationReadDto>();
    }
}