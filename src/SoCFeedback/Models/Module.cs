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

        /// <summary>
        /// Module ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Module Code
        /// </summary>
        [Required]
        [StringLength(Constants.ModuleCodeLength)]
        public string Code { get; set; }

        /// <summary>
        /// Module Title
        /// </summary>
        [Required]
        [StringLength(Constants.ModuleTitleLength)]
        public string Title { get; set; }

        /// <summary>
        /// Module URL
        /// </summary>
        [StringLength(Constants.UrlLength)]
        [Url]
        [Display(Name = "Module URL")]
        public string Url { get; set; }

        /// <summary>
        /// Level ID the module belongs to
        /// </summary>
        [Required]
        public Guid LevelId { get; set; }

        /// <summary>
        /// Supervisor ID, that supervises the Module
        /// </summary>
        [Required]
        public Guid SupervisorId { get; set; }

        /// <summary>
        /// Module Description
        /// </summary>
        [StringLength(Constants.ModuleDescLength)]
        public string Description { get; set; }

        /// <summary>
        /// Module Status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Module running status within an academic year, not mapped to the database
        /// </summary>
        [NotMapped]
        public RunningStatus RunningStatus { get; set; }

        /// <summary>
        /// Module year ID, not mapped to the database
        /// </summary>
        [NotMapped]
        public Guid YearId { get; set; }

        /// <summary>
        /// List of module questions, not mapped to the database
        /// </summary>
        [NotMapped]
        public List<Question> Questions { get; set; }

        /// <summary>
        /// Module questions categories, not mapped to the database
        /// </summary>
        [NotMapped]
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Virtual list of the module questions
        /// </summary>
        public virtual List<ModuleQuestions> ModuleQuestions { get; set; }
        
        /// <summary>
        /// Virtual list of year modules
        /// </summary>
        public virtual ICollection<YearModules> YearModules { get; set; }

        /// <summary>
        /// Virtual module level object
        /// </summary>
        public virtual Level Level { get; set; }

        /// <summary>
        /// Virtual module supervisor object
        /// </summary>
        [Display(Name = "Coordinator")]
        public virtual Supervisor Supervisor { get; set; }

        /// <summary>
        /// Module code and title combined
        /// </summary>
        [Display(Name = "Module")]
        [NotMapped]
        public string ModuleName => $"{Code} {Title}";
    }
}