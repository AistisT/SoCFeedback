using System.ComponentModel.DataAnnotations;
using SoCFeedback.Enums;


namespace SoCFeedback.Models.AccountViewModels
{
    public class ConfirmEmailViewModel
    {
        [Required]
        [StringLength(Constants.PasswordMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = Constants.PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
        public string PassCode { get; set; }
        public string UserId { get; set; }
    }
}