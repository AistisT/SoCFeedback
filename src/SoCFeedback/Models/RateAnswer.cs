using System;
using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Models
{
    public class RateAnswer
    {
        /// <summary>
        /// Answer ID
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Question ID
        /// </summary>
        [Required]
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Rating (answer)
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Year answer belongs to
        /// </summary>
        [Required]
        public int Year { get; set; }

        /// <summary>
        /// Module id, to which answer belongs to
        /// </summary>
        [Required]
        public Guid ModuleId { get; set; }

        /// <summary>
        /// Virtual question object
        /// </summary>
        public virtual Question Question { get; set; }
    }
}