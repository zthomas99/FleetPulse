namespace FleetPulse.Core.DTOs;

public class CreateTelemetryEventRequest
{
    public required string VehicleId { get; set; }

    public DateTime Timestamp { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double SpeedMph { get; set; }

    public string EngineStatus { get; set; } = string.Empty;
}