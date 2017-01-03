using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoCFeedback.Enums
{
    public enum YearStatus
    {
        /// <summary>
        /// Year is open for modifications and not visible to students
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Year is published, visible to students and closed to modications
        /// </summary>
        Published = 1,
    }
}