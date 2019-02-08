using System.Collections.Generic;

namespace _06_ml_dataset_scraper.DataStructures
{
    public class QuestionList
    {
        public List<Question> items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }
}