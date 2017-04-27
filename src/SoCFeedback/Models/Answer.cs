using System;
using System.ComponentModel.DataAnnotations;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Answer
    {
        /// <summary>
        /// Answer ID
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Question ID that the answer belongs to
        /// </summary>
        [Required]
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Answer text
        /// </summary>
        [Display(Name = "Answer")]
        [StringLength(Constants.AnswerLength)]
        public string Answer1 { get; set; }

        /// <summary>
        /// Year answer belongs to
        /// </summary>
        [Required]
        public int Year { get; set; }

        /// <summary>
        /// Module id the answer belongs to
        /// </summary>
        [Required]
        public Guid ModuleId { get; set; }

        /// <summary>
        /// Virtual question object, not mapped to database
        /// </summary>
        public virtual Question Question { get; set; }
    }
}