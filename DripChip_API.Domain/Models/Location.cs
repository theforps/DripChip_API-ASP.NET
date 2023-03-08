using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip_API.Domain.Models;

public class Location
{
    [Key]
    public long id { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
}