using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip_API.Domain.Models;

public class Types
{
    [Key]
    public long id { get; set; }
    [JsonIgnore]
    public string type { get; set; }
    [JsonIgnore]
    public List<Animal> Animals { get; set; } 
}