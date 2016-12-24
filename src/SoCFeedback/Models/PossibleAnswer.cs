using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class PossibleAnswer
    {
        public Guid Id { get; set; }
        public string Answer { get; set; }
        public Guid QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
