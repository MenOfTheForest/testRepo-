namespace Project.DTOs;

public class LapTimeDto
{
    public int LapTimeId { get; set; }
    public int RacingDriverId { get; set; }
    public DateTime StartDateTime { get; set; }
    public double? Sector1ElapsedTime { get; set; }
    public double? Sector2ElapsedTime { get; set; }
    public double? Sector3ElapsedTime { get; set; }
    public double? TotalLapTime { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
}

public class CreateLapTimeDto
{
    public int RacingDriverId { get; set; }
    public DateTime StartDateTime { get; set; }
    public double? Sector1ElapsedTime { get; set; }
    public double? Sector2ElapsedTime { get; set; }
    public double? Sector3ElapsedTime { get; set; }
}

public class UpdateLapTimeDto
{
    public int RacingDriverId { get; set; }
    public DateTime StartDateTime { get; set; }
    public double? Sector1ElapsedTime { get; set; }
    public double? Sector2ElapsedTime { get; set; }
    public double? Sector3ElapsedTime { get; set; }
}