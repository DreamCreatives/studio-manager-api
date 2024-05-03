using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StudioManager.Infrastructure.Common;

namespace StudioManager.Infrastructure;

[ExcludeFromCodeCoverage]
public sealed class StudioManagerReadDbContext(
    DbContextOptions<StudioManagerReadDbContext> options) : DbContextBase(options);