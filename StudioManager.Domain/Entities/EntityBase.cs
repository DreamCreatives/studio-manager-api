using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace StudioManager.Domain.Entities;

public abstract class EntityBase
{
    private readonly List<INotification> _entityEvents = [];
    public Guid Id { get; protected init; }

    [NotMapped] public IReadOnlyCollection<INotification> DomainEvents => _entityEvents;

    public void AddDomainEvent(INotification domainEvent)
    {
        _entityEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _entityEvents.Clear();
    }
}
