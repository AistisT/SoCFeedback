using System.ComponentModel.DataAnnotations;

namespace SoCFeedback.Enums
{
    public enum Roles
    {
        /// <summary>
        ///     Lecturer Role
        /// </summary>
        [Display(Name = "Head Lecturer")]
        Lecturer = 1,

        /// <summary>
        ///     Admin Role
        /// </summary>
        [Display(Name = "Administrator")]
        Admin = 2,

        /// <summary>
        ///     Limited lecturer Role
        /// </summary>
        [Display(Name = "Lecturer")]
        LecturerLimited = 0,

        /// <summary>
        ///     Teaching staff Role
        /// </summary>
        [Display(Name = "Teaching Staff")]
        TeachingStaff = 3
    }
}