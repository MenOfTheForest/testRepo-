using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTOs;
using Project.Models;

namespace Project.Services;

public class RacingTeamService : IRacingTeamService
{
    private readonly SpeedFestDbContext _context;

    public RacingTeamService(SpeedFestDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RacingTeamDto>> GetAllTeamsAsync()
    {
        var teams = await _context.RacingTeams
         .Include(t => t.Drivers)
            .ToListAsync();

        return teams.Select(t => new RacingTeamDto
        {
   RacingTeamId = t.RacingTeamId,
 Name = t.Name,
            TeamPrincipal = t.TeamPrincipal,
            Drivers = t.Drivers.Select(d => new RacingDriverDto
       {
 RacingDriverId = d.RacingDriverId,
              RacingTeamId = d.RacingTeamId,
       Name = d.Name,
             DriverNumber = d.DriverNumber,
              TeamName = t.Name
 }).ToList()
        }).ToList();
    }

    public async Task<RacingTeamDto?> GetTeamByIdAsync(int id)
    {
        var team = await _context.RacingTeams
            .Include(t => t.Drivers)
            .FirstOrDefaultAsync(t => t.RacingTeamId == id);

        if (team == null) return null;

        return new RacingTeamDto
        {
            RacingTeamId = team.RacingTeamId,
            Name = team.Name,
            TeamPrincipal = team.TeamPrincipal,
            Drivers = team.Drivers.Select(d => new RacingDriverDto
            {
                RacingDriverId = d.RacingDriverId,
                RacingTeamId = d.RacingTeamId,
                Name = d.Name,
                DriverNumber = d.DriverNumber,
                TeamName = team.Name
            }).ToList()
        };
    }

    public async Task<RacingTeamDto> CreateTeamAsync(CreateRacingTeamDto createTeamDto)
    {
        var team = new RacingTeam
        {
            Name = createTeamDto.Name,
            TeamPrincipal = createTeamDto.TeamPrincipal
        };

        _context.RacingTeams.Add(team);
        await _context.SaveChangesAsync();

        return new RacingTeamDto
        {
            RacingTeamId = team.RacingTeamId,
            Name = team.Name,
            TeamPrincipal = team.TeamPrincipal,
            Drivers = new List<RacingDriverDto>()
        };
    }

    public async Task<RacingTeamDto?> UpdateTeamAsync(int id, UpdateRacingTeamDto updateTeamDto)
    {
        var team = await _context.RacingTeams.FindAsync(id);
        if (team == null) return null;

        team.Name = updateTeamDto.Name;
        team.TeamPrincipal = updateTeamDto.TeamPrincipal;

        await _context.SaveChangesAsync();

        return new RacingTeamDto
        {
            RacingTeamId = team.RacingTeamId,
            Name = team.Name,
            TeamPrincipal = team.TeamPrincipal,
            Drivers = new List<RacingDriverDto>()
        };
    }

    public async Task<bool> DeleteTeamAsync(int id)
    {
        var team = await _context.RacingTeams.FindAsync(id);
        if (team == null) return false;

        _context.RacingTeams.Remove(team);
        await _context.SaveChangesAsync();
        return true;
    }
}