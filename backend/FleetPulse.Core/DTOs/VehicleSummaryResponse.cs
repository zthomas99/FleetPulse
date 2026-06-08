namespace FleetPulse.Core.DTOs;

public class VehicleSummaryResponse
{
    public required string VehicleId { get; set; }

    public int EventCount { get; set; }

    public double MaxSpeed { get; set; }

    public double AverageSpeed { get; set; }

    public DateTime FirstSeen { get; set; }

    public DateTime LastSeen { get; set; }              
}