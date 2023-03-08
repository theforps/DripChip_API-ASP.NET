using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO.Animal;

public class DTOAnimalSearchLocation
{
    public string startDateTime { get; set; } = DateTime.MinValue.ToString();
    public string endDateTime { get; set; } = DateTime.MaxValue.ToString();
    [Range(0, Int32.MaxValue)] public int from { get; set; } = 0;
    [Range(1, Int32.MaxValue)] public int size { get; set; } = 10;
}