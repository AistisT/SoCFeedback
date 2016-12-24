using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class Answer
    {
        public Answer()
        {
            Question = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public string Answer1 { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual ICollection<Question> Question { get; set; }
    }
}
