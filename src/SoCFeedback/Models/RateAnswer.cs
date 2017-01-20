using System;
using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Models
{
    public class RateAnswer
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid QuestionId { get; set; }
        [Required]

        [Range(0.5, float.MaxValue, ErrorMessage = "Rating is required.")]
        public float Rating { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public Guid ModuleId { get; set; }
        public virtual Question Question { get; set; }
    }
}