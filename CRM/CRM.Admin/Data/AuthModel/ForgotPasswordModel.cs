using System.ComponentModel.DataAnnotations;

namespace CRM.Admin.Data;

public class ForgotPasswordModel
{
    [Required(ErrorMessage = "Email is required.")]
    [StringLength(40, MinimumLength = 5)]
    public string Email { get; set; }
}