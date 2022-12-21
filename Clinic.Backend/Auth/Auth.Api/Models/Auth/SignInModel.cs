using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models.Auth;

public class SignInModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}