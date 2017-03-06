namespace SoCFeedback.Enums
{
    public enum YearStatus
    {
        /// <summary>
        /// Year is open for modifications and not visible to students
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Year is published, visible to students and closed to modifications
        /// </summary>
        Published = 1,

        /// <summary>
        /// Year is finished, no longer visible to students or editable
        /// </summary>
        Archived = 2
    }
}