using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SoCFeedback.Enums;

namespace SoCFeedback.Models.ViewModels
{
    public class QuestionViewModel
    {
        public QuestionViewModel()
        {
            StandardAnswers = new List<string>();
            RateAnswers = new List<float>();
            LabelsList = new List<float>();
            RatingsList = new List<int>();
        }

        /// <summary>
        /// Question ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Question Type
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// Question text
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Question Order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// List of text answers to the question
        /// </summary>
        [Display(Name = "Answer")]
        public List<string> StandardAnswers { get; set; }

        /// <summary>
        /// List of rating answers to the question
        /// </summary>
        public List<float> RateAnswers { get; set; }

        /// <summary>
        /// Rating question average
        /// </summary>
        public float Average => (float)Math.Round(RateAnswers.Average(),1,MidpointRounding.AwayFromZero);

        /// <summary>
        ///  Label list for the chart
        /// </summary>
        public List<float> LabelsList { get; set; }

        /// <summary>
        /// Rating list for the chart
        /// </summary>
        public List<int> RatingsList { get; set; }
        
        /// <summary>
        /// Category view model object
        /// </summary>
        public CategoriesViewModel Category { get; set; }

        /// <summary>
        /// set label list the chart
        /// </summary>
        public void SetLabelsList()
        {
            if (this.Type == QuestionType.Rate)
                LabelsList = RateAnswers.Distinct().OrderBy(e => e).ToList();
        }

        /// <summary>
        /// set rating list for the chart
        /// </summary>
        public void SetRatingsList()
        {
            if (this.Type == QuestionType.Rate)
            {
                var t = RateAnswers.OrderBy(r => r).ToList();
                RatingsList = t.GroupBy(r => r).Select(r => r.Count()).ToList();
            }
        }
    }
}