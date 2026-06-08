namespace FleetPulse.Core.Models;

public class TelemetryEvent
{
    public int Id { get; set;}

    public required string VehicleId { get; set;}

    public DateTime Timestamp { get; set;}

    public double Latitude { get; set;}

    public double Longitude { get; set;}

    public double SpeedMph { get; set;}
   
   public string EngineStatus { get; set;} = string.Empty;
}