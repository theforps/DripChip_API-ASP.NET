namespace DripChip_API.Domain.DTO.Animal;

public class DTOAnimalSearch
{
    public DateTime startDateTime { get; set; } = new DateTime(1500,01,01);
    public DateTime endDateTime { get; set; } = new DateTime(2500,01,01);
    public int? chipperId { get; set; }
    public long? chippingLocationId { get; set; }
    public string lifeStatus { get; set; }
    public string gender { get; set; }
    public int from { get; set; } = 0;
    public int size { get; set; } = 10;
}