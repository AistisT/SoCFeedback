using SoCFeedback.Enums;
using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class Category
    {
        public Category()
        {
            Question = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
