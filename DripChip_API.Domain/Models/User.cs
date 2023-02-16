using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.Models;

public class User
{
    [Key]
    public int id { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}