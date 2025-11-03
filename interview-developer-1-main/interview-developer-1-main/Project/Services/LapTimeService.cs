using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTOs;
using Project.Models;

namespace Project.Services;

public class LapTimeService : ILapTimeService
{
    private readonly SpeedFestDbContext _context;

    public LapTimeService(SpeedFestDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LapTimeDto>> GetAllLapTimesAsync()
    {
        return await _context.LapTimes
            .Include(l => l.Driver)
            .ThenInclude(d => d!.Team)
            .Select(l => new LapTimeDto
            {
                LapTimeId = l.LapTimeId,
                RacingDriverId = l.RacingDriverId,
                StartDateTime = l.StartDateTime,
                Sector1ElapsedTime = l.Sector1ElapsedTime,
                Sector2ElapsedTime = l.Sector2ElapsedTime,
                Sector3ElapsedTime = l.Sector3ElapsedTime,
                TotalLapTime = l.TotalLapTime,
                DriverName = l.Driver != null ? l.Driver.Name : string.Empty,
                TeamName = l.Driver != null && l.Driver.Team != null ? l.Driver.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<LapTimeDto?> GetLapTimeByIdAsync(int id)
    {
        var lapTime = await _context.LapTimes
            .Include(l => l.Driver)
            .ThenInclude(d => d!.Team)
            .FirstOrDefaultAsync(l => l.LapTimeId == id);

        if (lapTime == null) return null;

        return new LapTimeDto
        {
            LapTimeId = lapTime.LapTimeId,
            RacingDriverId = lapTime.RacingDriverId,
            StartDateTime = lapTime.StartDateTime,
            Sector1ElapsedTime = lapTime.Sector1ElapsedTime,
            Sector2ElapsedTime = lapTime.Sector2ElapsedTime,
            Sector3ElapsedTime = lapTime.Sector3ElapsedTime,
            TotalLapTime = lapTime.TotalLapTime,
            DriverName = lapTime.Driver?.Name ?? string.Empty,
            TeamName = lapTime.Driver?.Team?.Name ?? string.Empty
        };
    }

    public async Task<IEnumerable<LapTimeDto>> GetLapTimesByDriverIdAsync(int driverId)
    {
        return await _context.LapTimes
            .Include(l => l.Driver)
            .ThenInclude(d => d!.Team)
            .Where(l => l.RacingDriverId == driverId)
            .Select(l => new LapTimeDto
            {
                LapTimeId = l.LapTimeId,
                RacingDriverId = l.RacingDriverId,
                StartDateTime = l.StartDateTime,
                Sector1ElapsedTime = l.Sector1ElapsedTime,
                Sector2ElapsedTime = l.Sector2ElapsedTime,
                Sector3ElapsedTime = l.Sector3ElapsedTime,
                TotalLapTime = l.TotalLapTime,
                DriverName = l.Driver != null ? l.Driver.Name : string.Empty,
                TeamName = l.Driver != null && l.Driver.Team != null ? l.Driver.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<LapTimeDto>> GetFastestLapTimesAsync(int count = 10)
    {
        return await _context.LapTimes
            .Include(l => l.Driver)
            .ThenInclude(d => d!.Team)
            .Where(l => l.Sector1ElapsedTime.HasValue && 
                       l.Sector2ElapsedTime.HasValue && 
                       l.Sector3ElapsedTime.HasValue)
            .OrderBy(l => l.Sector1ElapsedTime + l.Sector2ElapsedTime + l.Sector3ElapsedTime)
            .Take(count)
            .Select(l => new LapTimeDto
            {
                LapTimeId = l.LapTimeId,
                RacingDriverId = l.RacingDriverId,
                StartDateTime = l.StartDateTime,
                Sector1ElapsedTime = l.Sector1ElapsedTime,
                Sector2ElapsedTime = l.Sector2ElapsedTime,
                Sector3ElapsedTime = l.Sector3ElapsedTime,
                TotalLapTime = l.TotalLapTime,
                DriverName = l.Driver != null ? l.Driver.Name : string.Empty,
                TeamName = l.Driver != null && l.Driver.Team != null ? l.Driver.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<LapTimeDto> CreateLapTimeAsync(CreateLapTimeDto createLapTimeDto)
    {
        var lapTime = new LapTime
        {
            RacingDriverId = createLapTimeDto.RacingDriverId,
            StartDateTime = createLapTimeDto.StartDateTime,
            Sector1ElapsedTime = createLapTimeDto.Sector1ElapsedTime,
            Sector2ElapsedTime = createLapTimeDto.Sector2ElapsedTime,
            Sector3ElapsedTime = createLapTimeDto.Sector3ElapsedTime
        };

        _context.LapTimes.Add(lapTime);
        await _context.SaveChangesAsync();

        // Load related data
        await _context.Entry(lapTime)
            .Reference(l => l.Driver)
            .LoadAsync();
        
        if (lapTime.Driver != null)
        {
            await _context.Entry(lapTime.Driver)
                .Reference(d => d.Team)
                .LoadAsync();
        }

        return new LapTimeDto
        {
            LapTimeId = lapTime.LapTimeId,
            RacingDriverId = lapTime.RacingDriverId,
            StartDateTime = lapTime.StartDateTime,
            Sector1ElapsedTime = lapTime.Sector1ElapsedTime,
            Sector2ElapsedTime = lapTime.Sector2ElapsedTime,
            Sector3ElapsedTime = lapTime.Sector3ElapsedTime,
            TotalLapTime = lapTime.TotalLapTime,
            DriverName = lapTime.Driver?.Name ?? string.Empty,
            TeamName = lapTime.Driver?.Team?.Name ?? string.Empty
        };
    }

    public async Task<LapTimeDto?> UpdateLapTimeAsync(int id, UpdateLapTimeDto updateLapTimeDto)
    {
        var lapTime = await _context.LapTimes
            .Include(l => l.Driver)
            .ThenInclude(d => d!.Team)
            .FirstOrDefaultAsync(l => l.LapTimeId == id);
        
        if (lapTime == null) return null;

        lapTime.RacingDriverId = updateLapTimeDto.RacingDriverId;
        lapTime.StartDateTime = updateLapTimeDto.StartDateTime;
        lapTime.Sector1ElapsedTime = updateLapTimeDto.Sector1ElapsedTime;
        lapTime.Sector2ElapsedTime = updateLapTimeDto.Sector2ElapsedTime;
        lapTime.Sector3ElapsedTime = updateLapTimeDto.Sector3ElapsedTime;

        await _context.SaveChangesAsync();

        return new LapTimeDto
        {
            LapTimeId = lapTime.LapTimeId,
            RacingDriverId = lapTime.RacingDriverId,
            StartDateTime = lapTime.StartDateTime,
            Sector1ElapsedTime = lapTime.Sector1ElapsedTime,
            Sector2ElapsedTime = lapTime.Sector2ElapsedTime,
            Sector3ElapsedTime = lapTime.Sector3ElapsedTime,
            TotalLapTime = lapTime.TotalLapTime,
            DriverName = lapTime.Driver?.Name ?? string.Empty,
            TeamName = lapTime.Driver?.Team?.Name ?? string.Empty
        };
    }

    public async Task<bool> DeleteLapTimeAsync(int id)
    {
        var lapTime = await _context.LapTimes.FindAsync(id);
        if (lapTime == null) return false;

        _context.LapTimes.Remove(lapTime);
        await _context.SaveChangesAsync();
        return true;
    }
}