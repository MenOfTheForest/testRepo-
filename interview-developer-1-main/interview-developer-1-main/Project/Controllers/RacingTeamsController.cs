using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacingTeamsController : ControllerBase
{
    private readonly IRacingTeamService _teamService;
    private readonly ILogger<RacingTeamsController> _logger;

    public RacingTeamsController(IRacingTeamService teamService, ILogger<RacingTeamsController> logger)
    {
        _teamService = teamService;
        _logger = logger;
    }

    /// <summary>
    /// Get all racing teams
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RacingTeamDto>>> GetAllTeams()
    {
        try
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all teams");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get a racing team by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RacingTeamDto>> GetTeamById(int id)
    {
        try
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound($"Team with ID {id} not found");
            }
            return Ok(team);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting team with ID {TeamId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Create a new racing team
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RacingTeamDto>> CreateTeam([FromBody] CreateRacingTeamDto createTeamDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await _teamService.CreateTeamAsync(createTeamDto);
            return CreatedAtAction(nameof(GetTeamById), new { id = team.RacingTeamId }, team);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating team");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Update an existing racing team
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<RacingTeamDto>> UpdateTeam(int id, [FromBody] UpdateRacingTeamDto updateTeamDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await _teamService.UpdateTeamAsync(id, updateTeamDto);
            if (team == null)
            {
                return NotFound($"Team with ID {id} not found");
            }
            return Ok(team);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating team with ID {TeamId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Delete a racing team
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeam(int id)
    {
        try
        {
            var success = await _teamService.DeleteTeamAsync(id);
            if (!success)
            {
                return NotFound($"Team with ID {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting team with ID {TeamId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}