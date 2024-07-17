using System.ComponentModel.DataAnnotations;

namespace CRM.Admin.Data.AuthModel;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required.")]
    [StringLength(40, MinimumLength = 5)]
    public string Email { get; set; } 

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, ErrorMessage = "Password must be at least 8 characters long.",
        MinimumLength = 8)]
    public string Password { get; set; } 
}