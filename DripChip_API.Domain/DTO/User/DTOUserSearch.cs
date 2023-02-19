using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO;

public class DTOUserSearch
{
    public string firstName { get; set; } = "";
    public string lastName { get; set; } = "";
    public string email { get; set; } = "";
    [Range(0, Int32.MaxValue)]
    public int from { get; set; } = 0;
    [Range(1, Int32.MaxValue)]
    public int size { get; set; } = 10;
}