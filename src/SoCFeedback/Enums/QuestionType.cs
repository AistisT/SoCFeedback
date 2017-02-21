using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Enums
{
    public enum QuestionType
    {
        /// <summary>
        ///     Question with one answer
        /// </summary>
        [Display(Name = "Free Form Text")]
        Standard = 0,

        /// <summary>
        ///     Rating scale question
        /// </summary>
        [Display(Name = "Star Rating")]
        Rate = 1
    }
}