using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Category
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
        public string Description { get; set; }

        public Status Status { get; set; }

        [Required]
        public CategoryType Type { get; set; }

        [Display(Name = "Category Order", Description = "Order in which category will be displayed in feedback form.")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int CategoryOrder { get; set; } = 99;

        public virtual ICollection<Question> Question { get; set; }
    }
}