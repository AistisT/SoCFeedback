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

        /// <summary>
        /// Category ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Category Title
        /// </summary>
        [MaxLength(Constants.CategoryTitleLength)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Category Description
        /// </summary>
        [MaxLength(Constants.CategoryDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Category status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Category Type
        /// </summary>
        [Required]
        public CategoryType Type { get; set; }

        /// <summary>
        /// Category Order
        /// </summary>
        [Display(Name = "Category Order", Description = "Order in which category will be displayed in feedback form.")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int CategoryOrder { get; set; } = 99;

        /// <summary>
        /// Questions belonging to category virtual collection
        /// </summary>
        public virtual ICollection<Question> Question { get; set; }
    }
}