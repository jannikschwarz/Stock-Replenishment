using Microsoft.EntityFrameworkCore;
using Replenishment.Domain.Entities;

namespace Replenishment.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<ReplenishmentRequest> Requests => Set<ReplenishmentRequest>();

    public DbSet<ReplenishmentRequestItem> RequestItems => Set<ReplenishmentRequestItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}