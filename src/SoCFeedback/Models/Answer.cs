using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Answer1 { get; set; }
        public DateTime Timestamp { get; set; }
        public String ModuleCode { get; set; }
        public virtual Question Question { get; set; }
    }
}
