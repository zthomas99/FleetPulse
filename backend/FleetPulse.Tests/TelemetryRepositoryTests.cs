using FleetPulse.Core.Models;
using FleetPulse.Infrastructure.Data;
using FleetPulse.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

namespace FleetPulse.Tests;

public class TelemetryRepositoryTests
{
    private static FleetPulseDbContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<FleetPulseDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new FleetPulseDbContext(options);
    }

    [Fact]
    public async Task AddAsync_saves_event()
    {
        await using var context = CreateContext("TelemetryRepositoryTests_AddAsync");
        var repository = new TelemetryRepository(context);

        var telemetryEvent = new TelemetryEvent
        {
            VehicleId = "V123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 55,
            EngineStatus = "On"
        };

        var saved = await repository.AddAsync(telemetryEvent);

        Assert.True(saved.Id > 0);
        Assert.Equal("V123", saved.VehicleId);
        Assert.Equal(1, await context.TelemetryEvents.CountAsync());
    }

    [Fact]
    public async Task GetLatest_returns_newest_event()
    {
        await using var context = CreateContext("TelemetryRepositoryTests_GetLatest");
        var repository = new TelemetryRepository(context);

        var olderEvent = new TelemetryEvent
        {
            VehicleId = "V123",
            Timestamp = new DateTime(2026, 6, 7, 9, 0, 0, DateTimeKind.Utc),
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 30,
            EngineStatus = "On"
        };

        var newerEvent = new TelemetryEvent
        {
            VehicleId = "V123",
            Timestamp = new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc),
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 45,
            EngineStatus = "Off"
        };

        context.TelemetryEvents.AddRange(olderEvent, newerEvent);
        await context.SaveChangesAsync();

        var result = await repository.GetLatestByVehicleIdAsync("V123");

        Assert.NotNull(result);
        Assert.Equal(newerEvent.Timestamp, result!.Timestamp);
        Assert.Equal(45, result.SpeedMph);
    }

    [Fact]
    public async Task GetVehicleIdsAsync_returns_distinct_sorted_vehicle_ids()
    {
        await using var context = CreateContext("TelemetryRepositoryTests_GetVehicleIds");
        var repository = new TelemetryRepository(context);

        var events = new[]
        {
            new TelemetryEvent { VehicleId = "V200", Timestamp = DateTime.UtcNow, Latitude = 33.749, Longitude = -84.388, SpeedMph = 60, EngineStatus = "On" },
            new TelemetryEvent { VehicleId = "V100", Timestamp = DateTime.UtcNow.AddMinutes(1), Latitude = 33.749, Longitude = -84.388, SpeedMph = 65, EngineStatus = "Off" },
            new TelemetryEvent { VehicleId = "V200", Timestamp = DateTime.UtcNow.AddMinutes(2), Latitude = 33.749, Longitude = -84.388, SpeedMph = 70, EngineStatus = "On" },
            new TelemetryEvent { VehicleId = "V150", Timestamp = DateTime.UtcNow.AddMinutes(3), Latitude = 33.749, Longitude = -84.388, SpeedMph = 55, EngineStatus = "Off" }
        };

        context.TelemetryEvents.AddRange(events);
        await context.SaveChangesAsync();

        var result = await repository.GetVehicleIdsAsync();

        Assert.Equal(3, result.Count);
        Assert.Equal(new List<string> { "V100", "V150", "V200" }, result);
    }

    [Fact]
    public async Task GetHistory_filters_by_date_range()
    {
        await using var context = CreateContext("TelemetryRepositoryTests_GetHistory");
        var repository = new TelemetryRepository(context);

        var events = new[]
        {
            new TelemetryEvent
            {
                VehicleId = "V123",
                Timestamp = new DateTime(2026, 6, 7, 9, 0, 0, DateTimeKind.Utc),
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 30,
                EngineStatus = "On"
            },
            new TelemetryEvent
            {
                VehicleId = "V123",
                Timestamp = new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc),
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 40,
                EngineStatus = "Off"
            },
            new TelemetryEvent
            {
                VehicleId = "V123",
                Timestamp = new DateTime(2026, 6, 7, 11, 0, 0, DateTimeKind.Utc),
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 50,
                EngineStatus = "On"
            },
            new TelemetryEvent
            {
                VehicleId = "V123",
                Timestamp = new DateTime(2026, 6, 7, 12, 0, 0, DateTimeKind.Utc),
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 60,
                EngineStatus = "Off"
            }
        };

        context.TelemetryEvents.AddRange(events);
        await context.SaveChangesAsync();

        var from = new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc);
        var to = new DateTime(2026, 6, 7, 11, 0, 0, DateTimeKind.Utc);

        var result = await repository.GetHistoryByVehicleIdAsync("V123", from, to);

        Assert.Equal(2, result.Count);
        Assert.Equal(11, result[0].Timestamp.Hour);
        Assert.Equal(10, result[1].Timestamp.Hour);
    }

    [Fact]
    public async Task GetSpeedingEvents_returns_only_speeding_records()
    {
        await using var context = CreateContext("TelemetryRepositoryTests_GetSpeedingEvents");
        var repository = new TelemetryRepository(context);

        var events = new[]
        {
            new TelemetryEvent
            {
                VehicleId = "V123",
                Timestamp = DateTime.UtcNow,
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 55,
                EngineStatus = "On"
            },
            new TelemetryEvent
            {
                VehicleId = "V123",
                Timestamp = DateTime.UtcNow.AddMinutes(1),
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 75,
                EngineStatus = "On"
            },
            new TelemetryEvent
            {
                VehicleId = "V123",
                Timestamp = DateTime.UtcNow.AddMinutes(2),
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 80,
                EngineStatus = "Off"
            }
        };

        context.TelemetryEvents.AddRange(events);
        await context.SaveChangesAsync();

        var result = await repository.GetSpeedingEventsAsync("V123", 70);

        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.True(item.SpeedMph > 70));
        Assert.Equal(80, result[0].SpeedMph);
        Assert.Equal(75, result[1].SpeedMph);
    }
}
