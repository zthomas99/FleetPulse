using FleetPulse.Core.Models;

namespace FleetPulse.Core.Interfaces
{
    public interface ITelemetryRepository
    {
        Task<TelemetryEvent> AddAsync(TelemetryEvent telemetryEvent);

        Task<TelemetryEvent?> GetLatestByVehicleIdAsync(string vehcileId);

        Task<List<string>> GetVehicleIdsAsync();
        
        Task<List<TelemetryEvent>> GetHistoryByVehicleIdAsync(
            string vehicleId, 
            DateTime? from, 
            DateTime? to);

        Task<List<TelemetryEvent>> GetSpeedingEventsAsync(
            string vehicleId,
            double threshold);

        Task<List<TelemetryEvent>> GetEventsForSummaryAsync(
            string vehicleId,
            DateTime? from,
            DateTime? to);
    }
}