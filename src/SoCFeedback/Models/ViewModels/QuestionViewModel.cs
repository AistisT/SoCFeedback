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
            StandardAnswers=new List<string>();
            RateAnswers= new List<float>();
            LabelsList=new List<float>();
            RatingsList =new List<int>();
        }
        public Guid Id { get; set; }
        public QuestionType Type { get; set; }
        public string Question { get; set; }
        public int Order { get; set; }
        [Display(Name = "Answer")]
        public List<String> StandardAnswers { get; set; }
        public List<float> RateAnswers { get; set; }
        public float Average => RateAnswers.Average();
        public List<float> LabelsList { get; set; }
        public List<int> RatingsList { get; set; }
        public CategoriesViewModel Category { get; set; }

        public void SetLabelsList()
        {
            LabelsList= RateAnswers.Distinct().OrderBy(e=>e).ToList();
        }

        public void SetRatingsList()
        {
            RatingsList = RateAnswers.GroupBy(r => r).Select(r => r.Count()).OrderBy(r => r).ToList();
        }
    }
}