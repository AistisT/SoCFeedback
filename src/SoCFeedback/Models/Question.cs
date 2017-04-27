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

        /// <summary>
        /// Question ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Question Text
        /// </summary>
        [Display(Name = "Question")]
        [Required]
        [StringLength(Constants.QuestionLength)]
        public string Question1 { get; set; }

        /// <summary>
        /// Question Type
        /// </summary>
        [Required]
        public QuestionType Type { get; set; }

        /// <summary>
        /// Category ID the question belongs to
        /// </summary>
        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Question optionality status
        /// </summary>
        [Display(Name = "Optional")]
        public bool Optional { get; set; } = true;

        /// <summary>
        /// Question status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Question Order
        /// </summary>
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Display(Name = "Question Order in the Category")]
        public int QuestionNumber { get; set; } = 99;

        [NotMapped]
        public RunningStatus RunningStatus { get; set; }

        /// <summary>
        /// Question answer to save, not mapped to database
        /// </summary>
        [NotMapped]
        public Answer AnswerToSave { get; set; }

        /// <summary>
        /// Rating question answer to save, not mapped to database
        /// </summary>
        [NotMapped]
        public RateAnswer RateAnswerToSave { get; set; }

        /// <summary>
        /// Year status, not mapped to database
        /// </summary>
        [NotMapped]
        public bool YearPublished { get; set; }

        /// <summary>
        /// Virtual list of Module questions
        /// </summary>
        public virtual ICollection<ModuleQuestions> ModuleQuestions { get; set; }

        /// <summary>
        /// Virtual list of question answers
        /// </summary>
        public virtual ICollection<Answer> Answer { get; set; }

        /// <summary>
        /// Virtual list of rating type question answers
        /// </summary>
        public virtual ICollection<RateAnswer> RateAnswer { get; set; }

        /// <summary>
        /// Virtual category object
        /// </summary>
        public virtual Category Category { get; set; }
    }
}