using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class RacingTeam
{
    public int RacingTeamId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string TeamPrincipal { get; set; } = string.Empty;
    
    public byte[]? Logo { get; set; }
    
    // Navigation property
    public virtual ICollection<RacingDriver> Drivers { get; set; } = new List<RacingDriver>();
}