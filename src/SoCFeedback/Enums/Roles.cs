using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Enums
{
    public enum Roles
    {
        /// <summary>
        ///     Lecturer Role
        /// </summary>
        [Display(Name = "Head Lecturer (Can create standard questions)")]
        Lecturer = 1,

        /// <summary>
        ///     Admin Role
        /// </summary>
        [Display(Name = "Admin (Full Privileges)")]
        Admin = 2,

        /// <summary>
        ///     Limited lecturer Role
        /// </summary>
        [Display(Name = "Lecturer (Can create optional questions only)")]
        LecturerLimited = 0,

        /// <summary>
        ///     Teaching staff Role
        /// </summary>
        [Display(Name = "Teaching Staff (Feedback view only)")]
        TeachingStaff = 3
    }
}