using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;

namespace StudioManager.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public abstract class DbContextBase(
    DbContextOptions options,
    IMediator mediator) : DbContext(options)
{
    private IDbContextTransaction? _transaction;
    public DbSet<EquipmentType> EquipmentTypes { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<User> Users { get; set; }

    private bool HasOpenTransaction => _transaction is not null;

    public async Task BeginTransactionAsync()
    {
        if (HasOpenTransaction) throw new InvalidOperationException(DB.HAS_OPEN_TRANSACTION);
        _transaction = await Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (!HasOpenTransaction) throw new InvalidOperationException(DB.NO_OPEN_TRANSACTION);
        await _transaction!.CommitAsync();
        _transaction.Dispose();
        _transaction = null;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEventsAsync(this);
        return await base.SaveChangesAsync(cancellationToken);
    }
}
