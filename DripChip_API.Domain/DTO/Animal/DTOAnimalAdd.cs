using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip_API.Domain.DTO.Animal;

public class DTOAnimalAdd
{
    [Required] public List<long> animalTypes { get; set; }

    [Required] [Range(0.001, float.MaxValue)] public float weight { get; set; } 

    [Required] [Range(0.001, float.MaxValue)] public float length { get; set; } 

    [Required] [Range(0.001, float.MaxValue)] public float height { get; set; } 

    [Required] public string gender { get; set; } 

    [Required] [Range(1, Int32.MaxValue)] public int chipperId { get; set; } 
    [Required] [Range(1, long.MaxValue)] public long chippingLocationId { get; set; }
    [JsonIgnore] public string lifeStatus = "ALIVE";
    [JsonIgnore] public DateTimeOffset chippingDateTime = DateTime.UtcNow;
}