using System;
using System.Collections.Generic;

namespace SoCFeedback.Models.ViewModels
{
    public class FeedbackViewModel
    {
        public FeedbackViewModel()
        {
            Questions=new List<QuestionViewModel>();
            Categories= new List<CategoriesViewModel>();
        }
        public Guid YearId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public String Level { get; set; }
        public string Coordinator { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public List<CategoriesViewModel> Categories { get; set; }
    }
}
