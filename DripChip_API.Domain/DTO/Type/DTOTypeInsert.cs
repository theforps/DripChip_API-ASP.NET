using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO.Type;

public class DTOTypeInsert
{
    [Required]
    public string type { get; set; }
}