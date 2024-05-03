using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudioManager.Domain.Entities;
using StudioManager.Domain.ErrorMessages;

namespace StudioManager.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public abstract class DbContextBase(
    DbContextOptions options) : DbContext(options)
{
    public DbSet<EquipmentType> EquipmentTypes { get; set; }
    
    private bool HasOpenTransaction => _transaction is not null;
    private IDbContextTransaction? _transaction;
    
    public async Task BeginTransactionAsync()
    {
        if (HasOpenTransaction)
        {
            throw new InvalidOperationException(DB.HAS_OPEN_TRANSACTION);
        }
        _transaction = await Database.BeginTransactionAsync();
    }
    
    public async Task CommitTransactionAsync()
    {
        if (!HasOpenTransaction)
        {
            throw new InvalidOperationException(DB.NO_OPEN_TRANSACTION);
        }
        await _transaction!.CommitAsync();
        _transaction.Dispose();
        _transaction = null;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}