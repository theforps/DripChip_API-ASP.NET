namespace DripChip_API.Domain.DTO.Location;

public class DTOLocationInfo
{
    public long id { get; set; }
    public DateTimeOffset dateTimeOfVisitLocationPoint { get; set; } = DateTime.UtcNow;
    public long locationPointId { get; set; }
}