using SoCFeedback.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Models
{
    public partial class Category
    {
        public Category()
        {
            Question = new HashSet<Question>();
        }
        public Guid Id { get; set; }
        [MaxLength(Constants.CategoryTitleLength)] 
        [Required]      
        public string Title { get; set; }
        [MaxLength(Constants.CategoryDescriptionLength)]
        [Required]
        public string Description { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
