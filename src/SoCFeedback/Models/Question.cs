using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Question
    {
        public Question()
        {
            ModuleQuestions = new HashSet<ModuleQuestions>();
            Answer = new HashSet<Answer>();
            RateAnswer = new HashSet<RateAnswer>();
        }

        public Guid Id { get; set; }

        [Display(Name = "Question")]
        [Required]
        [StringLength(Constants.QuestionLength)]
        public string Question1 { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }

        [Display(Name = "Optional")]
        public bool Optional { get; set; } = true;

        public Status Status { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Display(Name = "Question Order in the Category")]
        public int QuestionNumber { get; set; } = 99;

        [NotMapped]
        public RunningStatus RunningStatus { get; set; }

        [NotMapped]
        public Answer AnswerToSave { get; set; }

        [NotMapped]
        public RateAnswer RateAnswerToSave { get; set; }

        [NotMapped]
        public bool YearPublished { get; set; }

        public virtual ICollection<ModuleQuestions> ModuleQuestions { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
        public virtual ICollection<RateAnswer> RateAnswer { get; set; }
        public virtual Category Category { get; set; }
    }
}