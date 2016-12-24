using SoCFeedback.Enums;
using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class Question
    {
        public Question()
        {
            ModuleQuestions = new HashSet<ModuleQuestions>();
            PossibleAnswer = new HashSet<PossibleAnswer>();
        }

        public Guid Id { get; set; }
        public string Question1 { get; set; }
        public QuestionType Type { get; set; }
        public Guid AnswerId { get; set; }
        public Guid CategoryId { get; set; }
        public bool Optional { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<ModuleQuestions> ModuleQuestions { get; set; }
        public virtual ICollection<PossibleAnswer> PossibleAnswer { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual Category Category { get; set; }
    }
}
