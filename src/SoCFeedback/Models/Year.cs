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

        public Guid Id { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Display(Name = "Academic Year", Description = "Enter Start of Academic year only etc:2016")]
        public int Year1 { get; set; } = DateTime.Now.Year;

        public YearStatus Status { get; set; }

        public virtual ICollection<YearModules> YearModules { get; set; }

        [NotMapped]
        public List<Module> Modules { get; set; }

        [NotMapped]
        public List<Level> Levels { get; set; }
    }
}