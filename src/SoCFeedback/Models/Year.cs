using SoCFeedback.Enums;
using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class Year
    {
        public Year()
        {
            YearModules = new HashSet<YearModules>();
        }
        public Guid Id { get; set; }
        public int Year1 { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<YearModules> YearModules { get; set; }
    }
}
