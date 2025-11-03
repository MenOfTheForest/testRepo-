using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTOs;
using Project.Models;

namespace Project.Services;

public class RacingDriverService : IRacingDriverService
{
    private readonly SpeedFestDbContext _context;

    public RacingDriverService(SpeedFestDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RacingDriverDto>> GetAllDriversAsync()
    {
        return await _context.RacingDrivers
            .Include(d => d.Team)
            .Select(d => new RacingDriverDto
            {
                RacingDriverId = d.RacingDriverId,
                RacingTeamId = d.RacingTeamId,
                Name = d.Name,
                DriverNumber = d.DriverNumber,
                TeamName = d.Team != null ? d.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<RacingDriverDto?> GetDriverByIdAsync(int id)
    {
        var driver = await _context.RacingDrivers
            .Include(d => d.Team)
            .FirstOrDefaultAsync(d => d.RacingDriverId == id);

        if (driver == null) return null;

        return new RacingDriverDto
        {
            RacingDriverId = driver.RacingDriverId,
            RacingTeamId = driver.RacingTeamId,
            Name = driver.Name,
            DriverNumber = driver.DriverNumber,
            TeamName = driver.Team?.Name ?? string.Empty
        };
    }

    public async Task<IEnumerable<RacingDriverDto>> GetDriversByTeamIdAsync(int teamId)
    {
        return await _context.RacingDrivers
            .Include(d => d.Team)
            .Where(d => d.RacingTeamId == teamId)
            .Select(d => new RacingDriverDto
            {
                RacingDriverId = d.RacingDriverId,
                RacingTeamId = d.RacingTeamId,
                Name = d.Name,
                DriverNumber = d.DriverNumber,
                TeamName = d.Team != null ? d.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<RacingDriverDto> CreateDriverAsync(CreateRacingDriverDto createDriverDto)
    {
        var driver = new RacingDriver
        {
            RacingTeamId = createDriverDto.RacingTeamId,
            Name = createDriverDto.Name,
            DriverNumber = createDriverDto.DriverNumber
        };

        _context.RacingDrivers.Add(driver);
        await _context.SaveChangesAsync();

        // Load the team to get the team name
        await _context.Entry(driver)
            .Reference(d => d.Team)
            .LoadAsync();

        return new RacingDriverDto
        {
            RacingDriverId = driver.RacingDriverId,
            RacingTeamId = driver.RacingTeamId,
            Name = driver.Name,
            DriverNumber = driver.DriverNumber,
            TeamName = driver.Team?.Name ?? string.Empty
        };
    }

    public async Task<RacingDriverDto?> UpdateDriverAsync(int id, UpdateRacingDriverDto updateDriverDto)
    {
        var driver = await _context.RacingDrivers
            .Include(d => d.Team)
            .FirstOrDefaultAsync(d => d.RacingDriverId == id);
        
        if (driver == null) return null;

        driver.RacingTeamId = updateDriverDto.RacingTeamId;
        driver.Name = updateDriverDto.Name;
        driver.DriverNumber = updateDriverDto.DriverNumber;

        await _context.SaveChangesAsync();

        return new RacingDriverDto
        {
            RacingDriverId = driver.RacingDriverId,
            RacingTeamId = driver.RacingTeamId,
            Name = driver.Name,
            DriverNumber = driver.DriverNumber,
            TeamName = driver.Team?.Name ?? string.Empty
        };
    }

    public async Task<bool> DeleteDriverAsync(int id)
    {
        var driver = await _context.RacingDrivers.FindAsync(id);
        if (driver == null) return false;

        _context.RacingDrivers.Remove(driver);
        await _context.SaveChangesAsync();
        return true;
    }
}