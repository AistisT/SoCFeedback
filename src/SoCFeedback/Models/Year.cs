using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Year
    {
        public Year()
        {
            YearModules = new HashSet<YearModules>();
            Modules = new List<Module>();
            Levels = new List<Level>();
        }

        /// <summary>
        /// Year ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Year (eg. 2017)
        /// </summary>
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Display(Name = "Academic Year", Description = "Enter Start of Academic year only etc:2016")]
        public int Year1 { get; set; } = DateTime.Now.Year;
        
        /// <summary>
        /// Gets Academic year , read only
        /// </summary>
        [NotMapped]
        public string AcademicYear => $"{Year1}/{Year1+1}";

        /// <summary>
        /// Ending year of academic year, read only
        /// </summary>
        [NotMapped]
        public int EndingYear => Year1 + 1;

        /// <summary>
        /// Year status
        /// </summary>
        public YearStatus Status { get; set; }

        /// <summary>
        /// Virtual list of yearmodules
        /// </summary>
        public virtual ICollection<YearModules> YearModules { get; set; }

        /// <summary>
        /// Virtual list of Modules belong to a year
        /// </summary>
        [NotMapped]
        public List<Module> Modules { get; set; }

        /// <summary>
        /// Virtual list of levels belong to a year
        /// </summary>
        [NotMapped]
        public List<Level> Levels { get; set; }
    }
}