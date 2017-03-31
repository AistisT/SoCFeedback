using System;
using System.ComponentModel.DataAnnotations;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Answer
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        //[Required]
        [Display(Name = "Answer")]
        [StringLength(Constants.AnswerLength)]
        public string Answer1 { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public Guid ModuleId { get; set; }

        public virtual Question Question { get; set; }
    }
}