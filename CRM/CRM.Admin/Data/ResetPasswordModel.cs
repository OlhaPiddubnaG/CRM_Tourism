using System.ComponentModel.DataAnnotations;

namespace CRM.Admin.Data;

public class ResetPasswordModel
{
    public string Token { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
    public string ConfirmPassword { get; set; }
}