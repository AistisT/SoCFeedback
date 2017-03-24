using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Level
    {
        public Level()
        {
            Module = new HashSet<Module>();
        }

        public Guid Id { get; set; }

        [MaxLength(Constants.LevelTitleLength)]
        [Required]
        public string Title { get; set; }

        public Status Status { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Display(Name = "Level Order In a Year")]
        public int OrderingNumber { get; set; }

        [MaxLength(Constants.CategoryDescriptionLength)]
        public string Description { get; set; }

        public virtual ICollection<Module> Module { get; set; }
    }
}