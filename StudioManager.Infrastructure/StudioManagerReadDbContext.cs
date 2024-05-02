using Microsoft.EntityFrameworkCore;
using SolutionManager.Infrastructure.Common;

namespace SolutionManager.Infrastructure;

public sealed class StudioManagerReadDbContext(
    DbContextOptions<StudioManagerDbContext> options) : DbContextBase(options);