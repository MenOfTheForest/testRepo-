using Project.DTOs;

namespace Project.Services;

public interface IRacingTeamService
{
    Task<IEnumerable<RacingTeamDto>> GetAllTeamsAsync();
    Task<RacingTeamDto?> GetTeamByIdAsync(int id);
    Task<RacingTeamDto> CreateTeamAsync(CreateRacingTeamDto createTeamDto);
    Task<RacingTeamDto?> UpdateTeamAsync(int id, UpdateRacingTeamDto updateTeamDto);
    Task<bool> DeleteTeamAsync(int id);
}

public interface IRacingDriverService
{
    Task<IEnumerable<RacingDriverDto>> GetAllDriversAsync();
    Task<RacingDriverDto?> GetDriverByIdAsync(int id);
    Task<IEnumerable<RacingDriverDto>> GetDriversByTeamIdAsync(int teamId);
    Task<RacingDriverDto> CreateDriverAsync(CreateRacingDriverDto createDriverDto);
    Task<RacingDriverDto?> UpdateDriverAsync(int id, UpdateRacingDriverDto updateDriverDto);
    Task<bool> DeleteDriverAsync(int id);
}

public interface ILapTimeService
{
    Task<IEnumerable<LapTimeDto>> GetAllLapTimesAsync();
    Task<LapTimeDto?> GetLapTimeByIdAsync(int id);
    Task<IEnumerable<LapTimeDto>> GetLapTimesByDriverIdAsync(int driverId);
    Task<IEnumerable<LapTimeDto>> GetFastestLapTimesAsync(int count = 10);
    Task<LapTimeDto> CreateLapTimeAsync(CreateLapTimeDto createLapTimeDto);
    Task<LapTimeDto?> UpdateLapTimeAsync(int id, UpdateLapTimeDto updateLapTimeDto);
    Task<bool> DeleteLapTimeAsync(int id);
}