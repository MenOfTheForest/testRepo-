using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class RacingDriver
{
    public int RacingDriverId { get; set; }
    
    public int RacingTeamId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Range(1, 99)]
    public int DriverNumber { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(RacingTeamId))]
    public virtual RacingTeam? Team { get; set; }
    
    public virtual ICollection<LapTime> LapTimes { get; set; } = new List<LapTime>();
}