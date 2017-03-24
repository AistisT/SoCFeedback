using System.ComponentModel.DataAnnotations;
using SoCFeedback.Enums;

namespace SoCFeedback.Models.ManageViewModels
{
    public class ChangeNameViewModel
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
    }
}