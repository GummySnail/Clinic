using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models.Auth;

public class SignInModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(6, ErrorMessage = "Min Length 6")]
    [MaxLength(15, ErrorMessage = "Max length 15")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }
}