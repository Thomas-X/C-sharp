using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _05_m_net_research1
{
    public class Generator
    {
        // psi character greek word
        public static char DatasetSeparator = 'ψ';

        public static string Agile = "Agile";

        public static Dictionary<string, string> Answers = new Dictionary<string, string>()
        {
            {"scrummaster", "scrummaster"},
            {"invest", "invest"},
            {"dod", "dod"},
            {"feature", "feature"},
            {"bug", "bug"},
            {
                "technischwerk",
                "technischwerk"
            },
            {
                "spike",
                "spike"
            },
            {
                "epic",
                "epic"
            },
            {
                "acceptatiecriteria",
                "acceptatiecriteria"
            },
            {
                "userstory",
                "userstory"
            },
            {
                "drievragenuserstory",
                "drievragenuserstory"
            },
            {"velocity", "velocity"},
            {
                "prioriteit",
                "prioriteit"
            },
            {
                "machtsinteressediagram",
                "machtsinteressediagram"
            },
        };


        // https://github.com/oprogramador/most-common-words-by-language/blob/master/src/resources/dutch.txt
        // We could shuffle a set of words with the top 500 words

        // Two types of questions:
        // We want to include non-grammatically correct matches in our searches as well to improve predictions
        // What? <== gives the definition of the searched term

        // Some question include:


        // Step 1: Remove stopwords
        // Step 2: 


        // TODO whyQuestions and whyAnswers
        public static string[] whatQuestions =
        {
            "_REPLACE_WITH_TERM_",
//            "What does _REPLACE_WITH_TERM_ mean",
//            "Give me the definition of _REPLACE_WITH_TERM_",
//            "Wat kan _REPLACE_WITH_TERM_ zijn",
//            "Geef me de definitie van _REPLACE_WITH_TERM_",
//            "Kan je me de betekenis van _REPLACE_WITH_TERM_ geven",
//            "Wat betekent _REPLACE_WITH_TERM_",
//            "Wat is _REPLACE_WITH_TERM_ en wat houdt het in",
//            "Ik wil de definitie van _REPLACE_WITH_TERM_ weten",
//            "Weet je wat _REPLACE_WITH_TERM_ inhoudt",
//            "Hey hoi hi robot wat is _REPLACE_WITH_TERM_",
//            "Goededag middag avond robot wat is _REPLACE_WITH_TERM_",
//            "Weet je wat _REPLACE_WITH_TERM_ is",
//            "Wat _REPLACE_WITH_TERM_ is",
//            "Is _REPLACE_WITH_TERM_ wat",
//            "Welke _REPLACE_WITH_TERM_ is",
//            "Ik _REPLACE_WITH_TERM_ probleem",
//            "Wat is _REPLACE_WITH_TERM_ weet je dat",
//            "Zeg, wat is _REPLACE_WITH_TERM_",
//            "Wat voor _REPLACE_WITH_TERM_ is dat",
        };

        // FACTOID Give definition / NER to extract value
        // LIST Multiple/Single answer to a set i.e: 
        // https://github.com/dice-group/NLIWOD/tree/master/qa.datasets

        public static List<QuestionData> GenerateQuestionsFromTerm(string term, string answer, string category)
        {
            List<QuestionData> questionDatas = new List<QuestionData>();

            foreach (string whatQuestion in whatQuestions)
            {
                questionDatas.Add(
                    new QuestionData(
                        whatQuestion.Replace("_REPLACE_WITH_TERM_", term),
                        answer
                    ));
            }

            return questionDatas;
        }

        // Why? <== these are explaining questions, phrased very differently
        // TODO Why?-questions can be answered with "Omdat", in most cases

        public static List<List<QuestionData>> MachineLearningDataSet = new List<List<QuestionData>>()
        {
            GenerateQuestionsFromTerm("scrummaster", Answers["scrummaster"], Agile),
            GenerateQuestionsFromTerm("invest", Answers["invest"], Agile),
            GenerateQuestionsFromTerm("definitionofdone", Answers["dod"], Agile),
            GenerateQuestionsFromTerm("dod", Answers["dod"], Agile),
            GenerateQuestionsFromTerm("feature", Answers["feature"], Agile),
            GenerateQuestionsFromTerm("bug", Answers["bug"], Agile),
            GenerateQuestionsFromTerm("technischwerk", Answers["technischwerk"], Agile),
            GenerateQuestionsFromTerm("epic", Answers["epic"], Agile),
            GenerateQuestionsFromTerm("acceptatiecriteria", Answers["acceptatiecriteria"], Agile),
            GenerateQuestionsFromTerm("user story", Answers["userstory"], Agile),
            GenerateQuestionsFromTerm("drievragenuserstory", Answers["drievragenuserstory"], Agile),
            GenerateQuestionsFromTerm("velocity", Answers["velocity"], Agile),
            GenerateQuestionsFromTerm("prioriteit", Answers["prioriteit"], Agile),
            GenerateQuestionsFromTerm("machtsinteressediagram", Answers["machtsinteressediagram"], Agile),
            GenerateQuestionsFromTerm("spike", Answers["spike"], Agile),
        };


        public static void Generate()
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<QuestionData> questionDatas = MachineLearningDataSet.SelectMany(x => x).ToList();
            Program.Logger(new List<string>()
                {String.Format("The total amount of questions generated is: {0}", questionDatas.Count)});


            Program.LogObject(questionDatas);

            foreach (var questionData in questionDatas)
            {
                string[] strArr =
                {
                    questionData.Question,
                    questionData.Label
                };

                stringBuilder.AppendLine(String.Join(DatasetSeparator, strArr));
            }

            string path = $"{Directory.GetCurrentDirectory()}/ml-data.txt";
            File.WriteAllText(path, stringBuilder.ToString());
        }
    }
}