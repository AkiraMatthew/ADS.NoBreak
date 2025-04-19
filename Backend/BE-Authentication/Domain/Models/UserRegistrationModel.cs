using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class UserRegistrationModel
{
    [Required(ErrorMessage = "The field {0} is required!")]
    [EmailAddress(ErrorMessage = "The field {0} is not a valid format!")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "The field {0} is required!")]
    [StringLength(100, ErrorMessage = "The field {0} is not a valid format!", MinimumLength = 3)]
    public required string Password { get; set; }

    [Compare("Password", ErrorMessage = "The passwords must be the same")]
    public required string ConfirmPassword { get; set; }
}
