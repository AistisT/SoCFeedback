using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoCFeedback.Enums;

namespace SoCFeedback.Models
{
    public class Supervisor
    {
        public Supervisor()
        {
            Module = new HashSet<Module>();
        }

        /// <summary>
        /// Supervisor ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Supervisor Title
        /// </summary>
        [Required]
        public Title Title { get; set; }

        /// <summary>
        /// Supervisor's Forename
        /// </summary>
        [Required]
        [StringLength(Constants.NameLength)]
        public string Forename { get; set; }

        /// <summary>
        /// Supervisor's surname
        /// </summary>
        [Required]
        [StringLength(Constants.NameLength)]
        public string Surname { get; set; }

        /// <summary>
        /// Supervisor's Email
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(Constants.EmailLength)]
        public string Email { get; set; }

        /// <summary>
        /// Supervisor's status
        /// </summary>
        public Status Status { get; set; }
        public virtual ICollection<Module> Module { get; set; }

        /// <summary>
        /// Supervisor's full name, not mapped to database
        /// </summary>
        [NotMapped]
        public string FullName => $"{Title} {Forename} {Surname}";
    }
}