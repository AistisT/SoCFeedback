using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Enums
{
    public enum CategoryType
    {
        /// <summary>
        ///    Category accept all type questions
        /// </summary>
        [Display(Name = "Any Questions")]
        Open = 0,

        /// <summary>
        ///     Question with one answer
        /// </summary>
        [Display(Name = "Mandatory Questions Only")]
        Mandatory = 1,

        /// <summary>
        ///     Category accepts optional questions only
        /// </summary>
        [Display(Name = "Optional Questions Only")]
        Optional = 2,
    }
}