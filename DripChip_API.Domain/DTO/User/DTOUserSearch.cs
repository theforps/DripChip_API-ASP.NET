namespace DripChip_API.Domain.DTO;

public class DTOUserSearch
{
    public string firstName { get; set; } = "";
    public string lastName { get; set; } = "";
    public string email { get; set; } = "";
    public int from { get; set; } = 1;
    public int size { get; set; } = 10;
}