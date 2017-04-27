using System;

namespace SoCFeedback.Models
{
    public class ModuleQuestions
    {
        /// <summary>
        /// Module Id
        /// </summary>
        public Guid ModuleId { get; set; }

        /// <summary>
        /// Question ID
        /// </summary>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Virtual Module Object
        /// </summary>
        public virtual Module ModuleCodeNavigation { get; set; }

        /// <summary>
        /// Virtual Question Object
        /// </summary>
        public virtual Question Question { get; set; }
    }
}