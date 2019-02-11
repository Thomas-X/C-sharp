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
        public static string DatasetSeparator = "	";

        public static string Agile = "Agile";

        public static Dictionary<string, string> Answers = new Dictionary<string, string>()
        {
            {"scrummaster", "Een Scrum Master treedt meer op als 'Facilitator'; hij zorgt er voor dat het team niets in de weg staat om goed werk te kunnen verrichten. Ook begeleidt hij het team, Product owner en stakeholders tijdens de verschillende meetings en zorgt hij ervoor dat Scrum correct wordt geïmplementeerd."},
            {"invest", "Independent;Het moet onafhankelijk te implementeren zijn Negotiable;Het element moet onderhandelbaar zijn Valuable;Het moet mogelijk zijn de waarde van het backlog element te bepalenEstimable;Het moet mogelijk zijn om in te schatten hoe groot en/of hoe complex een element isSmall;Het element moet klein zijnTestable;Het eindresultaat moet testbaar zijn"},
            {"dod", "De Definition of Done is een lijst die een aantal eisen waaraan ieder backlogelement moet voldoen."},
            {"feature", "Feature Een functionele wens of eis. Meestal beschreven in de vorm van een user story"},
            {"bug", "Bug Een fout in de software. Gedefinieerd als een afwijking van de beschreven functionaliteit."},
            {
                "technischwerk",
                "Technisch werk Een beschrijving van een technische taak die moet worden uitgevoerd, maar geen directe functionele achtergrond heeft. Bijvoorbeeld het opzetten van een testomgeving, het inrichten van een database enzovoort."
            },
            {
                "spike",
                "Spike Het maken van een prototype om te onderzoeken hoe een bepaalde functionele wens of eis het best kan kan worden aangepakt"
            },
            {
                "epic",
                "Epic Een functionele wens die nog weinig in detail is uitgewerkt. Deze staat gewoonlijk onder aan de backlog en is dus nog ver in de toekomst. Naarmate een epic belangrijker wordt en dus naar boven schuift in de backlog, moet de epic worden opgesplitst in kleinere features die beter gedefinieerd."
            },
            {
                "acceptatiecriteria",
                "De acceptatiecriteria Bij ieder backlogelement wordt door de Product Owner in samenwerking met de klant een lijst met eisen opgesteld waaraan het specifieke element moet voldoen om geaccepteerd te worden."
            },
            {
                "userstory",
                "User story Een user story is een korte beschrijving in voor ‘normale’ mensen leesbare taal, van een functionele wens. Een story beslaat maximaal drie of vier zinnen waarin kort de wens wordt beschreven."
            },
            {
                "drie vragen userstory",
                "Drie vragen Wat heb ik gedaan sinds de vorige dagelijkse Scrum-vergadering waardoor het team dichter bij het Sprint-doel is gekomen? Wat ga ik doen tot de volgende dagelijkse Scrum-vergadering waardoor het team dichter bij het Sprint-doel komt? Zie ik blokkades die mij of het productieteam verhinderen het Sprintdoel te halen?"
            },
            {"velocity", "De teamsnelheid heet “velocity”. Een burndown chart houdt in hoelang het nog duurt voordat het budget op is met de huidige “velocity” van story points"},
            {
                "prioriteit",
                "Prioriteit: Dit is een getal dat aangeeft welke functionaliteit voor de klant het belangrijkst is . Soms wordt ook gebruiktgemaakt van de MoSCoW-reeks (Must have, Should have, Could have, Won’t have) of alleen de prioriteiten hoog, middel en laag"
            },
            {
                "machtsinteressediagram",
                "Machtsinteresse diagram houdt in dat linksboven de genen met het meeste macht en interesse zitten en links onder de mensen met het minste macht/interesse"
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
//            "_REPLACE_WITH_TERM_",
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
            GenerateQuestionsFromTerm("drie vragen userstory", Answers["drie vragen userstory"], Agile),
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

            stringBuilder.AppendLine("Question	Answer");

            foreach (var questionData in questionDatas)
            {
                string[] strArr =
                {
                    questionData.Question,
                    questionData.Label
                };

                stringBuilder.AppendLine(String.Join(DatasetSeparator, strArr));
            }

            string path = $"{Directory.GetCurrentDirectory()}/ml-data.tsv";
            File.WriteAllText(path, stringBuilder.ToString());
        }
    }
}