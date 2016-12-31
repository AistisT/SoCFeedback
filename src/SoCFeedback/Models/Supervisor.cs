using SoCFeedback.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Models
{
    public class Supervisor
    {
        public Supervisor()
        {
            Module = new HashSet<Module>();
        }
        public Guid Id { get; set; }
        [Required]
        public Title Title { get; set; }
        [Required]
        [StringLength(Constants.NameLength)]
        public string Forename { get; set; }
        [Required]
        [StringLength(Constants.NameLength)]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(Constants.EmailLength)]
        public string Email { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Module> Module { get; set; }
    }
}
