using Microsoft.ML.Data;

namespace _05_m_net_research1
{
    /// <summary>
    /// Used for making a "Q&A" combo
    /// </summary>
    public class QuestionData
    {
        public QuestionData(string _Question)
        {
            Question = _Question;
        }


        public QuestionData(string _Question, string _Label)
        {
            Question = _Question;
            Label = _Label;
        }


        [LoadColumn(1)] public string Question;

        [LoadColumn(2)] public string Label;

    }

    public class QuestionDataCategory : QuestionData
    {
        public string Category;

        public QuestionDataCategory(string _Question) : base(_Question)
        {
        }

        public QuestionDataCategory(string _Question, string _Label) : base(_Question, _Label)
        {
        }

        public QuestionDataCategory(string _Question, string _Label, string _Category) : base (_Question, _Label)
        {
            Question = _Question;
            Label = _Label;
            Category = _Category;
        }

    }

    // QuestionPrediction is the result returned from prediction operations
    public class QuestionPrediction
    {
        [ColumnName("PredictedLabel")] public string PredictedLabel;
        [ColumnName("Score")] public float[] Scores;
    }
}