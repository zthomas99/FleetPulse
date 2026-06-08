using FleetPulse.Core.DTOs;

namespace FleetPulse.Core.Interfaces
{
    public interface ITelemetryService
    {
        Task<TelemetryEventResponse> CreateAsync(CreateTelemetryEventRequest request);

        Task<TelemetryEventResponse?> GetLatestByVehicleIdAsync(string vehicleId);

        Task<List<string>> GetVehicleIdsAsync();
        
        Task<List<TelemetryEventResponse>> GetHistoryByVehicleIdAsync(
            string vehicleId,
            DateTime? from,
            DateTime? to
        );
        
        Task<List<TelemetryEventResponse>> GetSpeedingEventsAsync(
            string vehicleId,
            double threshold);
        
        Task<VehicleSummaryResponse?> GetSummaryAsync(
            string vehicleId,
            DateTime? from,
            DateTime? to);

    }
}