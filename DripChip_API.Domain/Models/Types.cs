using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.Models;

public class Types
{
    [Key]
    public long id { get; set; }
    public string type { get; set; }
}