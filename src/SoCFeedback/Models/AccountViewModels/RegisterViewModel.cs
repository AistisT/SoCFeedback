using System.ComponentModel.DataAnnotations;
using SoCFeedback.Enums;

namespace SoCFeedback.Models.AccountViewModels
{
    public class RegisterViewModel
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
        [RegularExpression("(^.*@dundee.ac.uk$)", ErrorMessage = "Only @dundee.ac.uk emails are allowed.")]
        [Display(Name = "Email")]
        [StringLength(Constants.EmailLength)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public Roles Role { get; set; }
    }
}