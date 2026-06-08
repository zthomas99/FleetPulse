using FleetPulse.Core.DTOs;
using FleetPulse.Core.Interfaces;
using FleetPulse.Core.Models;
using FleetPulse.Core.Services;


using Moq;

namespace FleetPulse.Tests;

public class TelemetryServiceTests
{
    [Fact]
    public async Task CreateAsync_valid_request_returns_response()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var request = new CreateTelemetryEventRequest

        {
            VehicleId = "v123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 72,
            EngineStatus = "On"
        };

        repositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<TelemetryEvent>()))
            .ReturnsAsync((TelemetryEvent telemetryEvent) =>
            {
                telemetryEvent.Id = 1;
                return telemetryEvent;
            });

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("V123", result.VehicleId);
        Assert.Equal(72, result.SpeedMph);
        Assert.Equal("On", result.EngineStatus);
    }

    [Fact]
    public async Task CreateAsync_negative_speed_throws_exception()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var request = new CreateTelemetryEventRequest
        {
            VehicleId = "V123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = -5,
            EngineStatus = "On"
        };

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync(request));

        // Assert
        Assert.Equal("Speed cannot be negative", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_invalid_engine_status_throws_exception()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var request = new CreateTelemetryEventRequest
        {
            VehicleId = "V123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 55,
            EngineStatus = "Running"
        };

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync(request));

        // Assert
        Assert.Equal("EngineStatus must be either On or Off", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WithWhitespaceVehicleIdAndEngineStatus_NormalizesInput()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var request = new CreateTelemetryEventRequest
        {
            VehicleId = " v123 ",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 55,
            EngineStatus = "On "
        };

        repositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<TelemetryEvent>()))
            .ReturnsAsync((TelemetryEvent telemetryEvent) =>
            {
                telemetryEvent.Id = 2;
                return telemetryEvent;
            });

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.Equal(2, result.Id);
        Assert.Equal("V123", result.VehicleId);
        Assert.Equal("On", result.EngineStatus);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidLatitude_ThrowsArgumentException()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var request = new CreateTelemetryEventRequest
        {
            VehicleId = "V123",
            Timestamp = DateTime.UtcNow,
            Latitude = 95,
            Longitude = -84.388,
            SpeedMph = 10,
            EngineStatus = "On"
        };

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync(request));

        // Assert
        Assert.Equal("Latitude must be between -90 and 90.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidLongitude_ThrowsArgumentException()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var request = new CreateTelemetryEventRequest
        {
            VehicleId = "V123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -195,
            SpeedMph = 10,
            EngineStatus = "On"
        };

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync(request));

        // Assert
        Assert.Equal("Longitude must be between -180 and 180", exception.Message);
    }

    [Fact]
    public async Task GetLatestByVehicleIdAsync_WithExistingEvent_ReturnsTelemetryEventResponse()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var telemetryEvent = new TelemetryEvent
        {
            Id = 5,
            VehicleId = "V123",
            Timestamp = DateTime.UtcNow,
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 55,
            EngineStatus = "Off"
        };

        repositoryMock
            .Setup(repo => repo.GetLatestByVehicleIdAsync("V123"))
            .ReturnsAsync(telemetryEvent);

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var result = await service.GetLatestByVehicleIdAsync(" v123 ");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result!.Id);
        Assert.Equal("V123", result.VehicleId);
        Assert.Equal("Off", result.EngineStatus);

        repositoryMock.Verify(repo => repo.GetLatestByVehicleIdAsync("V123"), Times.Once);
    }

    [Fact]
    public async Task GetVehicleIdsAsync_returns_vehicle_ids()
    {
        var repositoryMock = new Mock<ITelemetryRepository>();
        repositoryMock
            .Setup(repo => repo.GetVehicleIdsAsync())
            .ReturnsAsync(new List<string> { "V100", "V200" });

        var service = new TelemetryService(repositoryMock.Object);

        var result = await service.GetVehicleIdsAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(new List<string> { "V100", "V200" }, result);
    }

    [Fact]
    public async Task GetLatestByVehicleIdAsync_WithNoEvent_ReturnsNull()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        repositoryMock
            .Setup(repo => repo.GetLatestByVehicleIdAsync("V123"))
            .ReturnsAsync((TelemetryEvent?)null);

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var result = await service.GetLatestByVehicleIdAsync(" v123 ");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetHistoryByVehicleIdAsync_WithEvents_ReturnsTelemetryEventResponses()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var events = new List<TelemetryEvent>
        {
            new TelemetryEvent
            {
                Id = 10,
                VehicleId = "V123",
                Timestamp = new DateTime(2026, 6, 7, 9, 0, 0, DateTimeKind.Utc),
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 30,
                EngineStatus = "On"
            },
            new TelemetryEvent
            {
                Id = 11,
                VehicleId = "V123",
                Timestamp = new DateTime(2026, 6, 7, 9, 30, 0, DateTimeKind.Utc),
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 45,
                EngineStatus = "Off"
            }
        };

        repositoryMock
            .Setup(repo => repo.GetHistoryByVehicleIdAsync(
                It.Is<string>(id => id == "V123"),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()))
            .ReturnsAsync(events);

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var result = await service.GetHistoryByVehicleIdAsync(" v123 ", null, null);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(10, result[0].Id);
        Assert.Equal(11, result[1].Id);
        Assert.Equal("V123", result[0].VehicleId);
        Assert.Equal("V123", result[1].VehicleId);
    }

    [Fact]
    public async Task GetHistoryByVehicleIdAsync_WithInvalidDateRange_ThrowsArgumentException()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.GetHistoryByVehicleIdAsync(
                "V123",
                new DateTime(2026, 6, 8),
                new DateTime(2026, 6, 7)));

        // Assert
        Assert.Equal("From date cannot be later than to date.", exception.Message);
    }

    [Fact]
    public async Task GetSpeedingEventsAsync_WithNegativeThreshold_ThrowsArgumentException()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.GetSpeedingEventsAsync("V123", -1));

        // Assert
        Assert.Equal("Speed threshold cannot be negative", exception.Message);
    }

    [Fact]
    public async Task GetSpeedingEventsAsync_WithEvents_ReturnsTelemetryEventResponses()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var events = new List<TelemetryEvent>
        {
            new TelemetryEvent
            {
                Id = 20,
                VehicleId = "V123",
                Timestamp = DateTime.UtcNow,
                Latitude = 33.749,
                Longitude = -84.388,
                SpeedMph = 80,
                EngineStatus = "On"
            }
        };

        repositoryMock
            .Setup(repo => repo.GetSpeedingEventsAsync(
                It.Is<string>(id => id == "V123"),
                70))
            .ReturnsAsync(events);

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var result = await service.GetSpeedingEventsAsync(" v123 ", 70);

        // Assert
        Assert.Single(result);
        Assert.Equal(20, result[0].Id);
        Assert.Equal("V123", result[0].VehicleId);
        Assert.Equal(80, result[0].SpeedMph);
    }

    [Fact]
    public async Task GetSummaryAsync_no_events_returns_null()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        repositoryMock
            .Setup(repo => repo.GetEventsForSummaryAsync(
                It.IsAny<string>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<TelemetryEvent>());

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var result = await service.GetSummaryAsync("v123", null, null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetSummaryAsync_bad_date_range_throws_exception()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();
        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.GetSummaryAsync(
                "V123",
                new DateTime(2026, 6, 8),
                new DateTime(2026, 6, 7)));

        // Assert
        Assert.Equal("From date cannot be later than to date.", exception.Message);
    }

    [Fact]
    public async Task GetSummaryAsync_returns_correct_summary()
    {
        // Arrange
        var repositoryMock = new Mock<ITelemetryRepository>();

        var events = new List<TelemetryEvent>
    {
        new TelemetryEvent
        {
            Id = 1,
            VehicleId = "V123",
            Timestamp = new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc),
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 40,
            EngineStatus = "On"
        },
        new TelemetryEvent
        {
            Id = 2,
            VehicleId = "V123",
            Timestamp = new DateTime(2026, 6, 7, 11, 0, 0, DateTimeKind.Utc),
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 60,
            EngineStatus = "On"
        },
        new TelemetryEvent
        {
            Id = 3,
            VehicleId = "V123",
            Timestamp = new DateTime(2026, 6, 7, 12, 0, 0, DateTimeKind.Utc),
            Latitude = 33.749,
            Longitude = -84.388,
            SpeedMph = 80,
            EngineStatus = "On"
        }
    };

        repositoryMock
            .Setup(repo => repo.GetEventsForSummaryAsync(
                It.IsAny<string>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()))
            .ReturnsAsync(events);

        var service = new TelemetryService(repositoryMock.Object);

        // Act
        var result = await service.GetSummaryAsync("v123", null, null);

        // Assert
        Assert.NotNull(result);

        Assert.Equal("V123", result.VehicleId);
        Assert.Equal(3, result.EventCount);
        Assert.Equal(80, result.MaxSpeed);
        Assert.Equal(60, result.AverageSpeed);

        Assert.Equal(
            new DateTime(2026, 6, 7, 10, 0, 0, DateTimeKind.Utc),
            result.FirstSeen);

        Assert.Equal(
            new DateTime(2026, 6, 7, 12, 0, 0, DateTimeKind.Utc),
            result.LastSeen);

        repositoryMock.Verify(repo =>
            repo.GetEventsForSummaryAsync(
                It.IsAny<string>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()),
            Times.Once);
    }
}