using FleetPulse.Core.DTOs;
using FleetPulse.Core.Interfaces;
using FleetPulse.Core.Models;

namespace FleetPulse.Core.Services
{
    public class TelemetryService : ITelemetryService
    {
        private readonly ITelemetryRepository _repository;

        public TelemetryService(ITelemetryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TelemetryEventResponse> CreateAsync(CreateTelemetryEventRequest request)
        {
            ValidateRequest(request);

            var telemetryEvent = new TelemetryEvent
            {
                VehicleId = request.VehicleId.Trim().ToUpperInvariant(),
                Timestamp = request.Timestamp,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                SpeedMph = request.SpeedMph,
                EngineStatus = request.EngineStatus.Trim()
            };

            var savedEvent = await _repository.AddAsync(telemetryEvent);

            return MapToResponse(savedEvent);
        }

        public async Task<List<string>> GetVehicleIdsAsync()
        {
            return await _repository.GetVehicleIdsAsync();
        }

        public async Task<TelemetryEventResponse?> GetLatestByVehicleIdAsync(string vehicleId)
        {
            vehicleId = vehicleId.Trim().ToUpperInvariant();

            var telemetryEvent = await _repository.GetLatestByVehicleIdAsync(vehicleId);

            return telemetryEvent is null ? null : MapToResponse(telemetryEvent);
        }

        public async Task<List<TelemetryEventResponse>> GetHistoryByVehicleIdAsync(
            string vehicleId,
            DateTime? from,
            DateTime? to)
        {
            ValidateDateRange(from, to);

            vehicleId = vehicleId.Trim().ToUpperInvariant();
            

            var events = await _repository.GetHistoryByVehicleIdAsync(vehicleId, from, to);

            return events.Select(MapToResponse).ToList();
        }

        public async Task<List<TelemetryEventResponse>> GetSpeedingEventsAsync(
            string vehicleId,
            double threshold)
        {
            if (threshold < 0)
            {
                throw new ArgumentException("Speed threshold cannot be negative");
            }

            vehicleId = vehicleId.Trim().ToUpperInvariant();
            
            var events = await _repository.GetSpeedingEventsAsync(vehicleId, threshold);

            return events.Select(MapToResponse).ToList();
        }

        public async Task<VehicleSummaryResponse?> GetSummaryAsync(
            string vehicleId,
            DateTime? from,
            DateTime? to)
        {
            ValidateDateRange(from, to);

            var events = await _repository.GetEventsForSummaryAsync(vehicleId, from, to);

            if (!events.Any())
            {
                return null;
            }

            vehicleId = vehicleId.Trim().ToUpperInvariant();
            
            return new VehicleSummaryResponse
            {
                VehicleId = vehicleId,
                EventCount = events.Count,
                MaxSpeed = events.Max(e => e.SpeedMph),
                AverageSpeed = events.Average(e => e.SpeedMph),
                FirstSeen = events.Min(e => e.Timestamp),
                LastSeen = events.Max(e => e.Timestamp)
            };
        }

        private static void ValidateRequest(CreateTelemetryEventRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.VehicleId))
            {
                throw new ArgumentException("Vehicle Id is required");
            }

            if (request.Latitude < -90 || request.Latitude > 90)
            {
                throw new ArgumentException("Latitude must be between -90 and 90.");
            }

            if (request.Longitude < -180 || request.Longitude > 180)
            {
                throw new ArgumentException("Longitude must be between -180 and 180");
            }

            if (request.SpeedMph < 0)
            {
                throw new ArgumentException("Speed cannot be negative");
            }

            var engineStatus = request.EngineStatus?.Trim();

            if (!string.Equals(engineStatus, "On", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(engineStatus, "Off", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("EngineStatus must be either On or Off");
            }
        }

        private static void ValidateDateRange(DateTime? from, DateTime? to)
        {
            if (from.HasValue && to.HasValue && from.Value > to.Value)
            {
                throw new ArgumentException("From date cannot be later than to date.");
            }
        }

        private static TelemetryEventResponse MapToResponse(TelemetryEvent telemetryEvent)
        {
            return new TelemetryEventResponse
            {
                Id = telemetryEvent.Id,
                VehicleId = telemetryEvent.VehicleId,
                Timestamp = telemetryEvent.Timestamp,
                Latitude = telemetryEvent.Latitude,
                Longitude = telemetryEvent.Longitude,
                SpeedMph = telemetryEvent.SpeedMph,
                EngineStatus = telemetryEvent.EngineStatus
            };
        }
    }
}