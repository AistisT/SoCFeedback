using SoCFeedback.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Models
{
    public partial class Question
    {
        public Question()
        {
            ModuleQuestions = new HashSet<ModuleQuestions>();
            PossibleAnswer = new HashSet<PossibleAnswer>();
            Answer = new HashSet<Answer>();
        }

        public Guid Id { get; set; }
        [Display(Name = "Question")]
        [Required]
        [StringLength(500)]
        public string Question1 { get; set; }
        public QuestionType Type { get; set; }
        public String CategoryId { get; set; }
        public bool Optional { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<ModuleQuestions> ModuleQuestions { get; set; }
        public virtual ICollection<PossibleAnswer> PossibleAnswer { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
        public virtual Category Category { get; set; }
    }
}
