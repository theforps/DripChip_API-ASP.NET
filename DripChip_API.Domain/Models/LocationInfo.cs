using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip_API.Domain.Models;

public class LocationInfo
{
    [Key]
    public long id { get; set; }
    [JsonIgnore]
    public DateTime dateTimeOfVisitLocationPoint { get; set; }
    [JsonIgnore]
    public Location locationPoint { get; set; }
    [JsonIgnore]
    public List<Animal> Animals { get; set; }
}