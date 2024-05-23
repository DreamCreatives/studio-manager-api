using System.Diagnostics.CodeAnalysis;
using MediatR;
using StudioManager.Domain.Entities;

namespace StudioManager.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContextBase dbContext)
    {
        var domainEntities = dbContext.ChangeTracker
            .Entries<EntityBase>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        foreach (var @event in domainEvents) await mediator.Publish(@event);
    }
}
