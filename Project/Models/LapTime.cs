using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class LapTime
{
    public int LapTimeId { get; set; }
    
    public int RacingDriverId { get; set; }
    
    [Required]
    public DateTime StartDateTime { get; set; }
    
    [Range(0.0, double.MaxValue)]
    public double? Sector1ElapsedTime { get; set; }
    
    [Range(0.0, double.MaxValue)]
    public double? Sector2ElapsedTime { get; set; }
    
    [Range(0.0, double.MaxValue)]
    public double? Sector3ElapsedTime { get; set; }
    
    // Calculated property for total lap time
    [NotMapped]
    public double? TotalLapTime => 
        Sector1ElapsedTime.HasValue && 
        Sector2ElapsedTime.HasValue && 
        Sector3ElapsedTime.HasValue
            ? Sector1ElapsedTime + Sector2ElapsedTime + Sector3ElapsedTime
            : null;
    
    // Navigation property
    [ForeignKey(nameof(RacingDriverId))]
    public virtual RacingDriver? Driver { get; set; }
}