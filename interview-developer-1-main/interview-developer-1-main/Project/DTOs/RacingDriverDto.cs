namespace Project.DTOs;

public class RacingDriverDto
{
    public int RacingDriverId { get; set; }
    public int RacingTeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DriverNumber { get; set; }
    public string TeamName { get; set; } = string.Empty;
}

public class CreateRacingDriverDto
{
    public int RacingTeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DriverNumber { get; set; }
}

public class UpdateRacingDriverDto
{
    public int RacingTeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DriverNumber { get; set; }
}