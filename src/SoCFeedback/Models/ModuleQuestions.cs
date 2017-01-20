using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class ModuleQuestions
    {
        public Guid ModuleId { get; set; }
        public Guid QuestionId { get; set; }

        public virtual Module ModuleCodeNavigation { get; set; }
        public virtual Question Question { get; set; }
    }
}
