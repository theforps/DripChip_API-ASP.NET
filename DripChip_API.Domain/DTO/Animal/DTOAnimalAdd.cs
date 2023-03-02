using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO.Animal;

public class DTOAnimalAdd
{
    [Required] public List<long> animalTypes { get; set; }

    [Required] [Range(1, float.MaxValue)] public float weight { get; set; } 

    [Required] [Range(1, float.MaxValue)] public float length { get; set; } 

    [Required] [Range(1, float.MaxValue)] public float height { get; set; } 

    [Required] public string gender { get; set; } 

    [Required] [Range(1, Int32.MaxValue)] public int chipperId { get; set; } 
    [Required] [Range(1, long.MaxValue)] public long chippingLocationId { get; set; } 
}