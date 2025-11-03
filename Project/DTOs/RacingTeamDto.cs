namespace Project.DTOs;

public class RacingTeamDto
{
    public int RacingTeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TeamPrincipal { get; set; } = string.Empty;
    public List<RacingDriverDto> Drivers { get; set; } = new();
}

public class CreateRacingTeamDto
{
    public string Name { get; set; } = string.Empty;
    public string TeamPrincipal { get; set; } = string.Empty;
}

public class UpdateRacingTeamDto
{
    public string Name { get; set; } = string.Empty;
    public string TeamPrincipal { get; set; } = string.Empty;
}