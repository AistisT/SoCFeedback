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
        [MaxLength(50)] 
        [Required]
        [Key]
        public string Title { get; set; }
        [MaxLength(200)]
        [Required]
        public string Description { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
