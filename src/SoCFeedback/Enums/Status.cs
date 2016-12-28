namespace SoCFeedback.Enums
{
    public enum Status
    {
        /// <summary>
        /// Item is available for selection (year published to  students)
        /// </summary>
        Active = 0,

        /// <summary>
        /// For w/e reason item is no approved (if feature required to be implemented)
        /// </summary>
        Inactive = 1,

        /// <summary>
        /// item is no longer in circulation, everything is archived , no deletion.
        /// </summary>
        Archived = 2,
    }
}