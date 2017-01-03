using SoCFeedback.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCFeedback.Models
{
    public partial class Supervisor
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

        [NotMapped]
        public string FullName { get => $"{Title} {Forename} {Surname}"; }
    }
}
