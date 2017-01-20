using System;

namespace SoCFeedback.Models.ViewModels
{
    public class CategoriesViewModel
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}
