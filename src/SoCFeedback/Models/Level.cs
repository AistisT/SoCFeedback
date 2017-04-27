using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Level
    {
        public Level()
        {
            Module = new HashSet<Module>();
        }

        /// <summary>
        /// Level ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Level Title
        /// </summary>
        [MaxLength(Constants.LevelTitleLength)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Level Status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Level Order
        /// </summary>
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Display(Name = "Level Order In a Year")]
        public int OrderingNumber { get; set; }

        /// <summary>
        /// Level Description
        /// </summary>
        [MaxLength(Constants.CategoryDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// List of virtual modules belonging to the Level
        /// </summary>
        public virtual ICollection<Module> Module { get; set; }
    }
}