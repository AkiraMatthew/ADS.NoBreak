using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class UserLoginModel
{
    [Required(ErrorMessage = "The field {0} is required!")]
    [EmailAddress(ErrorMessage = "The field {0} is not a valid format!")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "The field {0} is required!")]
    [StringLength(100, ErrorMessage = "The field {0} is not a valid format!", MinimumLength = 3)]
    public required string Password { get; set; }
}
