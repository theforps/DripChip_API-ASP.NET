using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DripChip_API.Domain.DTO;

public class DTOUserRegistration
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    [EmailAddress]
    [RegularExpression(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)", ErrorMessage = "The email field is not a valid e-mail address.")]
    public string email { get; set; }
    public string password { get; set; }
}