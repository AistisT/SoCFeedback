using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class YearModules
    {
        public int Year { get; set; }
        public string ModuleCode { get; set; }

        public virtual Module ModuleCodeNavigation { get; set; }
        public virtual Year YearNavigation { get; set; }
    }
}
