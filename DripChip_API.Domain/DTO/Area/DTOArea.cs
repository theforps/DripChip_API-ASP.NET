using System.ComponentModel.DataAnnotations;
using DripChip_API.Domain.DTO.Location;

namespace DripChip_API.Domain.DTO.Area;

public class DTOArea
{
    public string name { get; set; }

    [MinLength(3)]
    [MaxLength(3)]
    public List<DTOLocation> areaPoints { get; set; } = new List<DTOLocation>(3)
    {
        new DTOLocation(),
        new DTOLocation(),
        new DTOLocation(),
    };
}