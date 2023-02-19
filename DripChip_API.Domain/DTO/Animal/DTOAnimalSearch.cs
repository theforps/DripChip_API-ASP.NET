﻿using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO.Animal;

public class DTOAnimalSearch
{
    public string startDateTime { get; set; } = "1000-01-01T00:00:00";
    public string endDateTime { get; set; } = "3000-01-01T00:00:00";
    [Range(1,Int32.MaxValue)]
    public int? chipperId { get; set; }
    [Range(1,Int32.MaxValue)]
    public long? chippingLocationId { get; set; }
    public string? lifeStatus { get; set; }
    public string? gender { get; set; }
    [Range(0,Int32.MaxValue)]
    public int from { get; set; } = 0;
    [Range(1,Int32.MaxValue)]
    public int size { get; set; } = 10;
}