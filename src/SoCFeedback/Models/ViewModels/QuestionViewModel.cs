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

        public Guid Id { get; set; }
        public QuestionType Type { get; set; }
        public string Question { get; set; }
        public int Order { get; set; }

        [Display(Name = "Answer")]
        public List<string> StandardAnswers { get; set; }

        public List<float> RateAnswers { get; set; }
        public float Average => (float)Math.Round(RateAnswers.Average(),1,MidpointRounding.AwayFromZero);
        public List<float> LabelsList { get; set; }
        public List<int> RatingsList { get; set; }
        public CategoriesViewModel Category { get; set; }

        public void SetLabelsList()
        {
            if (this.Type == QuestionType.Rate)
                LabelsList = RateAnswers.Distinct().OrderBy(e => e).ToList();
        }

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