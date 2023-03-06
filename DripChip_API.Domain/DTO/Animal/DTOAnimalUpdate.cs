using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO.Animal;

public class DTOAnimalUpdate
{
    [Required][Range(1,float.MaxValue)]
    public float weight { get; set; }
    [Required][Range(1,float.MaxValue)]
    public float length { get; set; }
    [Required][Range(1,float.MaxValue)]
    public float height { get; set; }
    public string gender { get; set; }
    public string lifeStatus { get; set; }
    [Required][Range(1,float.MaxValue)]
    public int chepperId { get; set; }
    [Required][Range(1,float.MaxValue)]
    public long chippingLocationId { get; set; }
}