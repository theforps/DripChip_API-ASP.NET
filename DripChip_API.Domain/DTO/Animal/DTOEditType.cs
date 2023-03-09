using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO.Animal;

public class DTOEditType
{
    [Required] [Range(1, long.MaxValue)]
    public long oldTypeId { get; set; }
    [Required] [Range(1, long.MaxValue)]
    public long newTypeId { get; set; }
}