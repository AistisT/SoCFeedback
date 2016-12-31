using SoCFeedback.Enums;
using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public partial class Module
    {
        public Module()
        {
            ModuleQuestions = new HashSet<ModuleQuestions>();
            YearModules = new HashSet<YearModules>();
        }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid LevelId { get; set; }
        public Guid SupervisorId { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<ModuleQuestions> ModuleQuestions { get; set; }
        public virtual ICollection<YearModules> YearModules { get; set; }
        public virtual Level Level { get; set; }
        public virtual Supervisor Supervisor { get; set; }
    }
}
