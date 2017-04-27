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

        /// <summary>
        /// Year ID
        /// </summary>
        public Guid YearId { get; set; }

        /// <summary>
        /// Module Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Module Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Module Level
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Module Coordinator
        /// </summary>
        public string Coordinator { get; set; }

        /// <summary>
        /// List of module questions
        /// </summary>
        public List<QuestionViewModel> Questions { get; set; }

        /// <summary>
        /// List of module question categories
        /// </summary>
        public List<CategoriesViewModel> Categories { get; set; }

        /// <summary>
        /// Average score for the module
        /// </summary>
        public float Average { get; set; }

        /// <summary>
        /// List of labels for the charts
        /// </summary>
        public List<String> LabelsList { get; set; }

        /// <summary>
        /// Rating lists for the charts
        /// </summary>
        public List<float> RatingsList { get; set; }

        /// <summary>
        /// Sets average score for the module
        /// </summary>
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

        /// <summary>
        /// sets label lists for the charts
        /// </summary>
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

        /// <summary>
        /// sets rating lists for the charts
        /// </summary>
        public void SetRatingsList()
        {
            foreach (var question in Questions.Where(q=>q.Type == QuestionType.Rate))
            {
                RatingsList.Add(question.Average);
            }
        }
    }
}