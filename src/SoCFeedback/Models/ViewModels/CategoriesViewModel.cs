using System;

namespace SoCFeedback.Models.ViewModels
{
    public class CategoriesViewModel
    {
        /// <summary>
        /// Category ID
        /// </summary>
        public Guid CategoryId { get; set; }
        
        /// <summary>
        /// Category Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Category Order
        /// </summary>
        public int Order { get; set; }
    }
}