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
        [Key]
        public string Title { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<Module> Module { get; set; }
    }
}
