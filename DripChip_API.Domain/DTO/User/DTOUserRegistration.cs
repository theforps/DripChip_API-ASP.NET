using System.ComponentModel.DataAnnotations;

namespace DripChip_API.Domain.DTO;

public class DTOUserRegistration
{
    [Required]
    public string firstName { get; set; }
    [Required]
    public string lastName { get; set; }
    [EmailAddress]
    [Required]
    [RegularExpression(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)", ErrorMessage = "The email field is not a valid e-mail address.")]
    public string email { get; set; }
    [Required]
    public string password { get; set; }
}