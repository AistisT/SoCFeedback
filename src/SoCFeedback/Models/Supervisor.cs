using SoCFeedback.Enums;
using System;
using System.Collections.Generic;

namespace SoCFeedback.Models
{
    public class Supervisor
    {
        public Supervisor()
        {
            Module = new HashSet<Module>();
        }

        public Title Title { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Module> Module { get; set; }
    }
}
