using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO.Location;

public class DTOLocationInfoEdit
{
    [Required] [Range(1,long.MaxValue)]
    public long visitedLocationPointId { get; set; }
    [Required] [Range(1,long.MaxValue)]
    public long locationPointId { get; set; }
}