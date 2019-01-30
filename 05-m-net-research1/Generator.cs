using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _05_m_net_research1
{
    public class Generator
    {
        public static char DatasetSeparator = '|';

        public static string Agile = "Agile";

        public static Dictionary<string, string> Answers = new Dictionary<string, string>()
        {
            {"scrummaster", "Een scrummaster is iemand die het ontwikkelteam begeleidt"},
            {"invest", "INVEST staat voor: Independent; Negotiable; Valuable;Estimable;Small;Testable"},
            {"dod", "De DoD (Definition of Done) is een lijst aan eisen waaraan ieder backlogelement moet voldoen"},
            {"feature", "Een functionele wens of eis. Meestal beschreven in de vorm van een user story"},
            {"bug", "Een fout in de software. Gedefinieerd als een afwijking van de beschreven functionaliteit."},
            {"technisch werk", "Een beschrijving van een technische taak die moet worden uitgevoerd, maar geen directe functionele achtergrond heeft. Bijvoorbeeld het opzetten van een testomgeving, het inrichten van een database enzovoort."},
            {"spike", "Het maken van een prototype om te onderzoeken hoe een bepaalde functionele wens of eis het best kan kan worden aangepakt"},
            {"epic", "Een functionele wens die nog weinig in detail is uitgewerkt. Deze staat gewoonlijk onder aan de backlog en is dus nog ver in de toekomst. Naarmate een epic belangrijker wordt en dus naar boven schuift in de backlog, moet de epic worden opgesplitst in kleinere features die beter gedefinieerd."},
            {"acceptatiecriteria", "Bij ieder backlogelement wordt door de Product Owner in samenwerking met de klant een lijst met eisen opgesteld waaraan het specifieke element moet voldoen om geaccepteerd te worden."},
            {"user story", "Een user story is een korte beschrijving in voor ‘normale’ mensen leesbare taal, van een functionele wens. Een story beslaat maximaal drie of vier zinnen waarin kort de wens wordt beschreven."},
            {"drie vragen user story", "Als een gebruiker met rol <X> wil ik <functionaliteit Y> om <reden Z>\r\nOmdat <reden Z> wil ik als gebruiker met rol <X>, <functionaliteit Y>\r\n"},
            {"velocity", "De teamsnelheid heet “velocity”."},
            {"prioriteit", " Dit is een getal dat aangeeft welke functionaliteit voor de klant het belangrijkst is . Soms wordt ook gebruiktgemaakt van de MoSCoW-reeks (Must have, Should have, Could have, Won’t have) of alleen de prioriteiten hoog, middel en laag"},
            {"machtsinteresse diagram", "Machtsinteresse diagram houdt in dat linksboven de genen met het meeste macht en interesse zitten en links onder de mensen met het minste macht/interesse"},
        };


        // https://github.com/oprogramador/most-common-words-by-language/blob/master/src/resources/dutch.txt
        // We could shuffle a set of words with the top 500 words

        // Two types of questions:
        // We want to include non-grammatically correct matches in our searches as well to improve predictions
        // What? <== gives the definition of the searched term

        // Some question include:


        // TODO whyQuestions and whyAnswers
        public static string[] whatQuestions =
        {
            "Wat is _REPLACE_WITH_TERM_",
            "Wat houdt _REPLACE_WITH_TERM_ in",
            "Wat kan _REPLACE_WITH_TERM_ zijn",
            "Geef me de definitie van _REPLACE_WITH_TERM_",
            "Kan je me de betekenis van _REPLACE_WITH_TERM_ geven",
            "Wat betekent _REPLACE_WITH_TERM_",
            "Wat is _REPLACE_WITH_TERM_ en wat houdt het in",
            "Ik wil de definitie van _REPLACE_WITH_TERM_ weten",
            "Weet je wat _REPLACE_WITH_TERM_ inhoudt",
            "Hey hoi hi robot wat is _REPLACE_WITH_TERM_",
            "Goededag middag avond robot wat is _REPLACE_WITH_TERM_",
            "Weet je wat _REPLACE_WITH_TERM_ is",
            "Wat _REPLACE_WITH_TERM_ is",
            "Is _REPLACE_WITH_TERM_ wat",
            "Welke _REPLACE_WITH_TERM_ is",
            "Ik _REPLACE_WITH_TERM_ probleem",
            "Wat is _REPLACE_WITH_TERM_ weet je dat",
            "Zeg, wat is _REPLACE_WITH_TERM_",
            "Wat voor _REPLACE_WITH_TERM_ is dat",
        };

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
            GenerateQuestionsFromTerm("definition of done", Answers["dod"], Agile),
            GenerateQuestionsFromTerm("dod", Answers["dod"], Agile),
            GenerateQuestionsFromTerm("feature", Answers["feature"], Agile),
            GenerateQuestionsFromTerm("bug", Answers["bug"], Agile),
            GenerateQuestionsFromTerm("technisch werk", Answers["technisch werk"], Agile),
            GenerateQuestionsFromTerm("epic", Answers["epic"], Agile),
            GenerateQuestionsFromTerm("acceptatiecriteria", Answers["acceptatiecriteria"], Agile),
            GenerateQuestionsFromTerm("user story", Answers["user story"], Agile),
            GenerateQuestionsFromTerm("drie vragen user story", Answers["drie vragen user story"], Agile),
            GenerateQuestionsFromTerm("velocity", Answers["velocity"], Agile),
            GenerateQuestionsFromTerm("prioriteit", Answers["prioriteit"], Agile),
            GenerateQuestionsFromTerm("machtsinteresse diagram", Answers["machtsinteresse diagram"], Agile),
            GenerateQuestionsFromTerm("spike", Answers["spike"], Agile),
        };


        public static void Generate()
        {
            StringBuilder stringBuilder = new StringBuilder();

            List<QuestionData> questionDatas = MachineLearningDataSet.SelectMany(x => x).ToList();

            Console.WriteLine(questionDatas.Count);

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