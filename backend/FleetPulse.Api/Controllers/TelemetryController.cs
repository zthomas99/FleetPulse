using FleetPulse.Core.DTOs;
using FleetPulse.Core.Interfaces;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly ITelemetryService _telemetryService;

    public TelemetryController(
        ITelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
    }

    [HttpPost]
    public async Task<ActionResult<TelemetryEventResponse>> Create(CreateTelemetryEventRequest request)
    {
        try
        {
            var result = await _telemetryService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetLatest),
                new { vehicleId = result.VehicleId },
                result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("vehicles")]
    public async Task<ActionResult<List<string>>> GetVehicleIds()
    {
        var result = await _telemetryService.GetVehicleIdsAsync();
        return Ok(result);
    }

    [HttpGet("vehicles/{vehicleId}/latest")]
    public async Task<ActionResult<TelemetryEventResponse>> GetLatest(
        string vehicleId)
    {
        var result = await _telemetryService.GetLatestByVehicleIdAsync(vehicleId);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("vehicles/{vehicleId}/history")]
    public async Task<ActionResult<List<TelemetryEventResponse>>> GetHistory(
        string vehicleId,
        DateTime? from,
        DateTime? to)
    {
        try
        {
            var result =
            await _telemetryService.GetHistoryByVehicleIdAsync(
                vehicleId,
                from,
                to);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("vehicles/{vehicleId}/speeding")]
    public async Task<ActionResult<List<TelemetryEventResponse>>> GetSpeeding(
        string vehicleId,
        double threshold = 70)
    {
        try
        {
            var result =
            await _telemetryService.GetSpeedingEventsAsync(
                vehicleId,
                threshold);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("vehicles/{vehicleId}/summary")]
    public async Task<ActionResult<VehicleSummaryResponse>> GetSummary(
        string vehicleId,
        DateTime? from,
        DateTime? to)
    {
        try
        {
            var result =
            await _telemetryService.GetSummaryAsync(
                vehicleId,
                from,
                to);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}