using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class SignUpDTO
{
    [Required(ErrorMessage = "The field {0} is required!")]
    [EmailAddress(ErrorMessage = "The field {0} is not a valid format!")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "The field {0} is required!")]
    [StringLength(100, ErrorMessage = "The field {0} is not a valid format!", MinimumLength = 3)]
    public string Password { get; set; } = null!;

    [Required, Compare("Password", ErrorMessage = "The passwords must be the same")]
    public string ConfirmPassword { get; set; } = null!;
    public string? Country { get; set; }
}
