using SoCFeedback.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        [StringLength(Constants.ModuleCodeLength)]
        public string Code { get; set; }
        [Required]
        [StringLength(Constants.ModuleTitleLength)]
        public string Title { get; set; }
        [StringLength(Constants.UrlLength)]
        [Url]
        public string Url { get; set; }
        [Required]
        public Guid LevelId { get; set; }
        [Required]
        public Guid SupervisorId { get; set; }
        [StringLength(Constants.ModuleDescLength)]
        public string Description { get; set; }
        public Status Status { get; set; }
        [NotMapped]
        public ModuleStatus RunningStatus { get; set; }

        public virtual ICollection<ModuleQuestions> ModuleQuestions { get; set; }
        public virtual ICollection<YearModules> YearModules { get; set; }
        public virtual Level Level { get; set; }
        public virtual Supervisor Supervisor { get; set; }
    }
}
