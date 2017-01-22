using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}