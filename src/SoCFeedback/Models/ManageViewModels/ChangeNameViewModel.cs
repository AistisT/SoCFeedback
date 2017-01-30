using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Models.ManageViewModels
{
    public class ChangeNameViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
    MinimumLength = 2)]
        [Display(Name = "Forename")]
        public string Forename { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
    }
}