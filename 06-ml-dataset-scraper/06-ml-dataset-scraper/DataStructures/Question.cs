using System.Collections.Generic;

namespace _06_ml_dataset_scraper.DataStructures
{
    public class Question
    {
        public List<string> tags { get; set; }
        public object owner { get; set; }
        public bool is_answered { get; set; }
        public int view_count { get; set; }
        public int answer_count { get; set; }
        public int score { get; set; }
        public int last_activity_date { get; set; }
        public int last_edit_date { get; set; }
        public int question_id { get; set; }
        public string link { get; set; }
        public string title { get; set; }

    }
}