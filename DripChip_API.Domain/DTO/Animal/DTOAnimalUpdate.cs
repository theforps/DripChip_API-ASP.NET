using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip_API.Domain.DTO.Animal;

public class DTOAnimalUpdate
{
    [Required][Range(0.001, float.MaxValue)] public float weight { get; set; }
    [Required][Range(0.001, float.MaxValue)] public float length { get; set; }
    [Required][Range(0.001, float.MaxValue)] public float height { get; set; }
    public string gender { get; set; }
    public string lifeStatus { get; set; }
    [Required][Range(1,float.MaxValue)] public int chipperId { get; set; }
    [Required][Range(1,float.MaxValue)] public long chippingLocationId { get; set; }
    [JsonIgnore]
    public DateTime deathDateTime { get; set; }
}

