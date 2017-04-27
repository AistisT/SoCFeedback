using System;

namespace SoCFeedback.Models
{
    public class YearModules
    {
        /// <summary>
        /// Year ID
        /// </summary>
        public Guid YearId { get; set; }

        /// <summary>
        /// Module ID
        /// </summary>
        public Guid ModuleId { get; set; }

        /// <summary>
        /// Virtual Module object
        /// </summary>
        public virtual Module ModuleCodeNavigation { get; set; }

        /// <summary>
        /// Virtual year object
        /// </summary>
        public virtual Year YearNavigation { get; set; }
    }
}