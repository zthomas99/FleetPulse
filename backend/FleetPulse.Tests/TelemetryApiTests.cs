using FleetPulse.Core.DTOs;
using FleetPulse.Core.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace FleetPulse.Tests;

public class TelemetryApiTests
{
    [Fact]
    public async Task POST_valid_telemetry_returns_201()
    {
        var telemetryResponse = new TelemetryEventResponse
        {
            Id = 1,
            VehicleId = "V123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 55,
            EngineStatus = "On"
        };

        var serviceMock = new Mock<ITelemetryService>();
        serviceMock
            .Setup(s => s.CreateAsync(It.IsAny<CreateTelemetryEventRequest>()))
            .ReturnsAsync(telemetryResponse);

        var controller = new TelemetryController(serviceMock.Object);

        var request = new CreateTelemetryEventRequest
        {
            VehicleId = "v123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 55,
            EngineStatus = "On"
        };

        var result = await controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetLatest", createdResult.ActionName);
        Assert.Equal(telemetryResponse, createdResult.Value);
    }

    [Fact]
    public async Task POST_invalid_telemetry_returns_400()
    {
        var serviceMock = new Mock<ITelemetryService>();
        serviceMock
            .Setup(s => s.CreateAsync(It.IsAny<CreateTelemetryEventRequest>()))
            .ThrowsAsync(new ArgumentException("Speed cannot be negative"));

        var controller = new TelemetryController(serviceMock.Object);

        var request = new CreateTelemetryEventRequest
        {
            VehicleId = "v123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = -5,
            EngineStatus = "On"
        };

        var result = await controller.Create(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Speed cannot be negative", badRequest.Value);
    }

    [Fact]
    public async Task GET_missing_vehicle_returns_404()
    {
        var serviceMock = new Mock<ITelemetryService>();
        serviceMock
            .Setup(s => s.GetLatestByVehicleIdAsync("UNKNOWN"))
            .ReturnsAsync((TelemetryEventResponse?)null);

        var controller = new TelemetryController(serviceMock.Object);

        var result = await controller.GetLatest("UNKNOWN");

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GET_vehicle_ids_returns_ok()
    {
        var vehicleIds = new List<string> { "V100", "V200" };
        var serviceMock = new Mock<ITelemetryService>();
        serviceMock
            .Setup(s => s.GetVehicleIdsAsync())
            .ReturnsAsync(vehicleIds);

        var controller = new TelemetryController(serviceMock.Object);

        var result = await controller.GetVehicleIds();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(vehicleIds, okResult.Value);
    }
}
