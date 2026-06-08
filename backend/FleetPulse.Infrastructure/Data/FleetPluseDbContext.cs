using FleetPulse.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetPulse.Infrastructure.Data;

public class FleetPulseDbContext : DbContext
{
    public FleetPulseDbContext(DbContextOptions<FleetPulseDbContext> options) : base(options)
    {
    }

    public DbSet<TelemetryEvent> TelemetryEvents => Set<TelemetryEvent>();
}
