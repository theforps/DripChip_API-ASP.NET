namespace DripChip_API.Domain.DTO.Animal;

public class DTOAnimal
{
    public long id { get; set; }
    public List<long> animalTypes { get; set; }
    public float weight { get; set; }
    public float length { get; set; }
    public float height { get; set; }
    public string gender { get; set; }
    public string lifeStatus { get; set; }
    public DateTimeOffset chippingDateTime { get; set; }
    public int chipperId { get; set; }
    public long chippingLocationId { get; set; }
    public List<long> visitedLocations { get; set; }
    public DateTimeOffset? deathDateTime { get; set; }
}