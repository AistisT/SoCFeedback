using Microsoft.AspNetCore.Mvc;
using SoCFeedback.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Models
{
    public partial class Level
    {
        public Level()
        {
            Module = new HashSet<Module>();
        }
        public Guid Id { get; set; }
        [MaxLength(Constants.LevelTitleLength)]
        [Required]
       // [Remote(action: "CheckLevelExists", controller: "Levels")]
        public string Title { get; set; }
        public Status Status { get; set; }
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int OrderingNumber { get; set; }
        [MaxLength(Constants.CategoryDescriptionLength)]

        public string Description { get; set; }
        public virtual ICollection<Module> Module { get; set; }
    }
}
