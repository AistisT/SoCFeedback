using System;

namespace SoCFeedback.Models
{
    public class YearModules
    {
        public Guid YearId { get; set; }
        public Guid ModuleId { get; set; }
        public virtual Module ModuleCodeNavigation { get; set; }
        public virtual Year YearNavigation { get; set; }
    }
}