using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacingDriversController : ControllerBase
{
    private readonly IRacingDriverService _driverService;
    private readonly ILogger<RacingDriversController> _logger;

    public RacingDriversController(IRacingDriverService driverService, ILogger<RacingDriversController> logger)
    {
        _driverService = driverService;
        _logger = logger;
    }

    /// <summary>
    /// Get all racing drivers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RacingDriverDto>>> GetAllDrivers()
    {
        try
        {
            var drivers = await _driverService.GetAllDriversAsync();
            return Ok(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all drivers");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get a racing driver by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RacingDriverDto>> GetDriverById(int id)
    {
        try
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            if (driver == null)
            {
                return NotFound($"Driver with ID {id} not found");
            }
            return Ok(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting driver with ID {DriverId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get all drivers for a specific team
    /// </summary>
    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<RacingDriverDto>>> GetDriversByTeamId(int teamId)
    {
        try
        {
            var drivers = await _driverService.GetDriversByTeamIdAsync(teamId);
            return Ok(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting drivers for team {TeamId}", teamId);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Create a new racing driver
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RacingDriverDto>> CreateDriver([FromBody] CreateRacingDriverDto createDriverDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var driver = await _driverService.CreateDriverAsync(createDriverDto);
            return CreatedAtAction(nameof(GetDriverById), new { id = driver.RacingDriverId }, driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating driver");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Update an existing racing driver
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<RacingDriverDto>> UpdateDriver(int id, [FromBody] UpdateRacingDriverDto updateDriverDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var driver = await _driverService.UpdateDriverAsync(id, updateDriverDto);
            if (driver == null)
            {
                return NotFound($"Driver with ID {id} not found");
            }
            return Ok(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating driver with ID {DriverId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Delete a racing driver
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDriver(int id)
    {
        try
        {
            var success = await _driverService.DeleteDriverAsync(id);
            if (!success)
            {
                return NotFound($"Driver with ID {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting driver with ID {DriverId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}