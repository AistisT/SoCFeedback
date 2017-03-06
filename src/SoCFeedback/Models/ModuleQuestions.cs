using System;

namespace SoCFeedback.Models
{
    public class ModuleQuestions
    {
        public Guid ModuleId { get; set; }
        public Guid QuestionId { get; set; }
        public virtual Module ModuleCodeNavigation { get; set; }
        public virtual Question Question { get; set; }
    }
}