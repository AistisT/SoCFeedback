using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class YearModules
    {
        public Guid YearId { get; set; }
        public Guid ModuleId { get; set; }

        public virtual Module ModuleCodeNavigation { get; set; }
        public virtual Year YearNavigation { get; set; }
    }
}
