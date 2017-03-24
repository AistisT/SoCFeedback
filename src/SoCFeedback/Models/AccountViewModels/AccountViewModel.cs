using System.ComponentModel.DataAnnotations;
using SoCFeedback.Enums;

namespace SoCFeedback.Models.AccountViewModels
{
    public class AccountViewModel
    {
        [Required]
        [StringLength(Constants.NameLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = Constants.NameMinLength)]
        [Display(Name = "Forename")]
        public string Forename { get; set; }

        [Required]
        [StringLength(Constants.NameLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = Constants.NameMinLength)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(Constants.EmailLength)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public Roles Role { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }
    }
}