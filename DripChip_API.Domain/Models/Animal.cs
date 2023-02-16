using System.ComponentModel.DataAnnotations;
using DripChip_API.Domain.Models;

namespace DripChip_API.Domain.Models;

public class Animal
{
    [Key]
    public long id { get; set; }
    List<Types> animalTypes { get; set; }
    public float weight { get; set; }
    public float length { get; set; }
    public float height { get; set; }
    public string gender { get; set; }
    public string lifeStatus { get; set; }
    public DateTime chippingDateTime { get; set; }
    public int chipperId { get; set; }
    public long chippingLocationId { get; set; }
    //public List<long> visitedLocations { get; set; }
    public DateTime? deathDateTime { get; set; }
}