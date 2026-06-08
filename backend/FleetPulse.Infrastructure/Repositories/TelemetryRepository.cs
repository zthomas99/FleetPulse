using FleetPulse.Core.Interfaces;
using FleetPulse.Core.Models;
using FleetPulse.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace FleetPulse.Infrastructure.Repositories;

public class TelemetryRepository : ITelemetryRepository
{
    private readonly FleetPulseDbContext _context;

    public TelemetryRepository(FleetPulseDbContext context)
    {
        _context = context;
    }

    public async Task<TelemetryEvent> AddAsync(TelemetryEvent telemetryEvent)
    {
        _context.TelemetryEvents.Add(telemetryEvent);

        await _context.SaveChangesAsync();

        return telemetryEvent;
    }

    public async Task<List<string>> GetVehicleIdsAsync()
    {
        return await _context.TelemetryEvents
            .Select(e => e.VehicleId)
            .Distinct()
            .OrderBy(id => id)
            .ToListAsync();
    }
    
    public async Task<TelemetryEvent?> GetLatestByVehicleIdAsync(string vehicleId)
    {
        return await _context.TelemetryEvents
        .Where(e => e.VehicleId == vehicleId)
        .OrderByDescending(e => e.Timestamp)
        .FirstOrDefaultAsync();
    }

    public async Task<List<TelemetryEvent>> GetHistoryByVehicleIdAsync(
        string vehicleId,
        DateTime? from,
        DateTime? to)
    {
        var query = _context.TelemetryEvents
            .Where(e => e.VehicleId == vehicleId);

        if (from.HasValue)
        {
            query = query.Where(e => e.Timestamp >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(e => e.Timestamp <= to.Value);
        }

        return await query
            .OrderByDescending(e => e.Timestamp)
            .ToListAsync();
    }

    public async Task<List<TelemetryEvent>> GetSpeedingEventsAsync(
        string vehicleId,
        double threshold
    )
    {
        return await _context.TelemetryEvents
            .Where(e => e.VehicleId == vehicleId && e.SpeedMph > threshold)
            .OrderByDescending(e => e.Timestamp)
            .ToListAsync();
    }

    public async Task<List<TelemetryEvent>> GetEventsForSummaryAsync(
        string vehicleId,
        DateTime? from,
        DateTime? to)
    {
        return await GetHistoryByVehicleIdAsync(vehicleId, from, to);
    }

}