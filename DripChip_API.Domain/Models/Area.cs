using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.Models;

public class Area
{
    [Key]
    public long id { get; set; }
    public string name { get; set; }

    [MinLength(3)]
    [MaxLength(3)]
    public List<Location> areaPoints { get; set; } = new List<Location>(3)
     {
        new Location(),
        new Location(),
        new Location(),
    };
}