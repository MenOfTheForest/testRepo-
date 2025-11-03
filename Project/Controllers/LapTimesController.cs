using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LapTimesController : ControllerBase
{
    private readonly ILapTimeService _lapTimeService;
    private readonly ILogger<LapTimesController> _logger;

    public LapTimesController(ILapTimeService lapTimeService, ILogger<LapTimesController> logger)
    {
        _lapTimeService = lapTimeService;
        _logger = logger;
    }

    /// <summary>
    /// Get all lap times
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LapTimeDto>>> GetAllLapTimes()
    {
        try
        {
            var lapTimes = await _lapTimeService.GetAllLapTimesAsync();
            return Ok(lapTimes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all lap times");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get a lap time by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<LapTimeDto>> GetLapTimeById(int id)
    {
        try
        {
            var lapTime = await _lapTimeService.GetLapTimeByIdAsync(id);
            if (lapTime == null)
            {
                return NotFound($"Lap time with ID {id} not found");
            }
            return Ok(lapTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting lap time with ID {LapTimeId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get all lap times for a specific driver
    /// </summary>
    [HttpGet("driver/{driverId}")]
    public async Task<ActionResult<IEnumerable<LapTimeDto>>> GetLapTimesByDriverId(int driverId)
    {
        try
        {
            var lapTimes = await _lapTimeService.GetLapTimesByDriverIdAsync(driverId);
            return Ok(lapTimes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting lap times for driver {DriverId}", driverId);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get the fastest lap times
    /// </summary>
    [HttpGet("fastest")]
    public async Task<ActionResult<IEnumerable<LapTimeDto>>> GetFastestLapTimes([FromQuery] int count = 10)
    {
        try
        {
            if (count <= 0 || count > 100)
            {
                return BadRequest("Count must be between 1 and 100");
            }

            var lapTimes = await _lapTimeService.GetFastestLapTimesAsync(count);
            return Ok(lapTimes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting fastest lap times");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Create a new lap time
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<LapTimeDto>> CreateLapTime([FromBody] CreateLapTimeDto createLapTimeDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lapTime = await _lapTimeService.CreateLapTimeAsync(createLapTimeDto);
            return CreatedAtAction(nameof(GetLapTimeById), new { id = lapTime.LapTimeId }, lapTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating lap time");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Update an existing lap time
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<LapTimeDto>> UpdateLapTime(int id, [FromBody] UpdateLapTimeDto updateLapTimeDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lapTime = await _lapTimeService.UpdateLapTimeAsync(id, updateLapTimeDto);
            if (lapTime == null)
            {
                return NotFound($"Lap time with ID {id} not found");
            }
            return Ok(lapTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating lap time with ID {LapTimeId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Delete a lap time
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLapTime(int id)
    {
        try
        {
            var success = await _lapTimeService.DeleteLapTimeAsync(id);
            if (!success)
            {
                return NotFound($"Lap time with ID {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting lap time with ID {LapTimeId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}