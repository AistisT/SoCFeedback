using System;
using System.Collections.Generic;
using System.Linq;
using SoCFeedback.Enums;


namespace SoCFeedback.Models.ViewModels
{
    public class FeedbackViewModel
    {
        public FeedbackViewModel()
        {
            Questions = new List<QuestionViewModel>();
            Categories = new List<CategoriesViewModel>();
            LabelsList = new List<string>();
            RatingsList = new List<float>();
        }

        public Guid YearId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
        public string Coordinator { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public List<CategoriesViewModel> Categories { get; set; }
        public float Average { get; set; }
        public List<String> LabelsList { get; set; }
        public List<float> RatingsList { get; set; }

        public void SetAverage()
        {
            int number = 0;
            Average = 0;

            foreach (var question in Questions.Where(q => q.Type == QuestionType.Rate))
            {
                number++;
                Average += question.Average;
            }
             Average /= number;
        }

        public void SetLabelsList()
        {
            int questionNumber = 0;
            foreach (var cat in Categories)
            {
                foreach (var question in Questions.Where(q=>q.Category.Title==cat.Title))
                {
                    questionNumber++;
                    if (question.Type == QuestionType.Rate)
                    {
                        LabelsList.Add($"Q{questionNumber}");
                    }
                }
            }
        }

        public void SetRatingsList()
        {
            foreach (var question in Questions.Where(q=>q.Type == QuestionType.Rate))
            {
                RatingsList.Add(question.Average);
            }
        }
    }
}