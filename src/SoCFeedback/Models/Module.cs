using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Module
    {
        public Module()
        {
            ModuleQuestions = new List<ModuleQuestions>();
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
        [Display(Name = "Module URL")]
        public string Url { get; set; }

        [Required]
        public Guid LevelId { get; set; }

        [Required]
        public Guid SupervisorId { get; set; }

        [StringLength(Constants.ModuleDescLength)]
        public string Description { get; set; }

        public Status Status { get; set; }

        [NotMapped]
        public RunningStatus RunningStatus { get; set; }

        [NotMapped]
        public Guid YearId { get; set; }

        [NotMapped]
        public List<Question> Questions { get; set; }

        [NotMapped]
        public List<Category> Categories { get; set; }

        public virtual List<ModuleQuestions> ModuleQuestions { get; set; }
        public virtual ICollection<YearModules> YearModules { get; set; }
        public virtual Level Level { get; set; }

        [Display(Name = "Coordinator")]
        public virtual Supervisor Supervisor { get; set; }

        [Display(Name = "Module")]
        [NotMapped]
        public string ModuleName => $"{Code} {Title}";
    }
}