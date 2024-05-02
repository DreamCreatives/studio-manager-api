using Microsoft.EntityFrameworkCore;
using SolutionManager.Infrastructure.Common;

namespace SolutionManager.Infrastructure;

public sealed class StudioManagerDbContext
    (DbContextOptions<StudioManagerDbContext> options) : DbContextBase(options);