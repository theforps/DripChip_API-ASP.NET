using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip_API.Domain.Models;

public class LocationInfo
{
    [Key]
    public long id { get; set; }
    [JsonIgnore]
    public DateTime dateTimeOfVisitLocationPoint { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public Location locationPoint { get; set; }
    [JsonIgnore]
    public Animal animal { get; set; }
}