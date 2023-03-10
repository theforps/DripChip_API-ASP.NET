using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO.Location;

public class DTOLocation
{
    [Range(-90,90)] [Required] public double latitude { get; set; }
    [Range(-180,180)] [Required] public double longitude { get; set; }
}