using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using Microsoft.ML.EntryPoints;
using Microsoft.ML.Model;
using Microsoft.ML.Transforms.Conversions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTextSummarizer;
using OpenTextSummarizer.Interfaces;

namespace _05_m_net_research1
{
    // TODO :
    // -benchmarks (x10000 vragen) en een onderzoeksdocumentje vaststellen
    // -code cleanup
    // -grotere dataset
    // -dataset question generator
    // -gecategorizeerde antwoorden:
    //      -Answer["Agile"]["Scrummaster"]

    // TODO for generating data set:
    // 
    // Question templates: 
    // Open vragen:

    // Niet-context gebasseerde vragen:
    // Wie <term>?
    // Welke <term>?
    // Waar, waarheen, waarvandaan?

    // Context gebasseerde vragen (Vragen naar de reden, oorzaak, verklaring of doel):
    // Waarom, waardoor, hoezo, waartoe, hoe, wanneer, hoe laat, hoelang, hoe vaak
    // Wat voor?

    // Antwoord: Omdat Scrum het meeste tijd bespaart

    class QuestionGenerator
    {
        public QuestionGenerator()
        {
        }
    }


    class Program
    {
        private static MLContext mlContext;
        private static EstimatorChain<KeyToValueMappingTransformer> pipeline;
        private static IDataView trainingData;
        private static ITransformer model;
        private const string Divider = "******************************************************************";
        private const string SaveLocation = "./model-saved.zip";
        private const string DataLocation = "ml-data.txt";
        private const string MetricsLocation = "./metrics.json";

        private static IDataView testData; 
        // Max iterations the multiclass classification algorithm can do (less means less quality model, but quicker re-trains)

        // Switch from Int32.MaxValue to 20 or so for train times of 2-5 seconds
        private const int maxIterations = 50000; 
        private static PredictionEngine<QuestionData, QuestionPrediction> predictor; 

        public class Metric
        {
            public object metrics;
            public DateTime dateTime;
        }


        public static MLContext CreateMachineLearningContext()
        {
            Logger(new List<string>() {"Creating MachineLearning context.."});
            return new MLContext();
        }

        public static void Logger(List<string> list)
        {
            Console.WriteLine(Divider);
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(Divider);
        }

        /// <summary>
        /// Reads data from a .txt file
        /// If the .txt file can't be found make sure you have enabled the "Copy always" property of the .txt file
        /// 1) https://i.imgur.com/uGqIuCL.png
        /// 2) https://i.imgur.com/jBebTRx.png
        /// </summary>
        /// <returns></returns>
        public static IDataView ReadData()
        {
            var reader =
                mlContext.Data.CreateTextReader<QuestionData>(separatorChar: Generator.DatasetSeparator,
                    hasHeader: false);
            Logger(new List<string>() {"Reading data.."});
            return reader.Read($"{Directory.GetCurrentDirectory()}/{DataLocation}");
        }

        /// <summary>
        /// Created the pipeline used in the model, containing certain mappings
        /// In this case, the MulticlassClassification.Trainers.StochasticDualCoordinateAscent algorithm needs one set of values which corresponds to a certain "class", in our case a Question is
        /// the value that needs to be determined with a "class", in our case an Answer, resulting in the model connecting certain questions with certain answers
        /// </summary>
        /// <returns></returns>
        public static EstimatorChain<KeyToValueMappingTransformer> SetupPipeline()
        {
            Logger(new List<string>() {"Setting up learning pipeline.."});
            return mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(mlContext.Transforms.Text.FeaturizeText("Question", "Features"))
                .Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(
                    labelColumn: "Label", featureColumn: "Features"
                ))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
        }

        /// <summary>
        /// Trains the data set and creates a model from training.
        /// </summary>
        public static ITransformer CreateModel()
        {
//            EnsembleCreator.CreateMultiClassPipelineEnsemble(null, pipeline);

            Logger(new List<string>() {"Training model.."});
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var (trainData, testData) = mlContext.MulticlassClassification.TrainTestSplit(trainingData, testFraction: 0.1);
            var _model = pipeline.Fit(trainData);
            Logger(new List<string>()
            {
                $"Estimated model training time was: {watch.ElapsedMilliseconds}ms"
            });
            // Now run the 5-fold cross-validation experiment, using the same pipeline.
            var cvResults = mlContext.MulticlassClassification.CrossValidate(testData, pipeline, numFolds: 5);

            // The results object is an array of 5 elements. For each of the 5 folds, we have metrics, model and scored test data.
            // Let's compute the average micro-accuracy.
            var microAccuracies = cvResults.Select(r => r.metrics.AccuracyMicro);
            Logger(new List<string>() { $"Avg microaccuracy from cross-validation: {microAccuracies.Average()}" });
            return _model;
        }

        /// <summary>
        /// Saves the model
        /// </summary>
        public static void SaveModel()
        {
            using (var stream = File.Create(SaveLocation))
            {
                mlContext.Model.Save(model, stream);
            }
        }

        /// <summary>
        /// Loads the model (could be used in something like a .. Clippy bot!)
        /// This model loading could be done in another process!
        /// </summary>
        /// <param name="args"></param>
        public static ITransformer LoadModel()
        {
            ITransformer loadedModel;
            using (var stream = File.Open(SaveLocation, FileMode.Open))
                loadedModel = mlContext.Model.Load(stream);
            return loadedModel;
        }

        /// <summary>
        /// Logs objects
        /// </summary>
        /// <param name="logObj"></param>
        public static void LogObject(object logObj)
        {
            string json = JsonConvert.SerializeObject(logObj, Formatting.Indented);
            Console.WriteLine(json);
        }

        /// <summary>
        /// Serializes objects to JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// Gets and saves metrics for the current model
        /// </summary>
        public static void SaveMetrics()
        {
            // Gets the metrics from the current model
            // Possible duplicate call if not loading from existing model
            IDataView testData = ReadData();
            var metrics = mlContext.MulticlassClassification.Evaluate(model.Transform(testData), label: "Label");

            Logger(new List<string>() { String.Format("The model is {0}% better than random guessing", metrics.LogLossReduction) });

            // Get existing metrics
            string savedMetrics = "[]";

            if (File.Exists(MetricsLocation))
            {
                savedMetrics = File.ReadAllText(MetricsLocation);
            }

            // Add metric to list
            var list = JsonConvert.DeserializeObject<List<object>>(savedMetrics);
            list.Add(new Metric()
            {
                metrics = metrics,
                dateTime = DateTime.UtcNow
            });
            // Serialize & save
            string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(MetricsLocation, updatedJson);
        }

        public static void PredictAndLog()
        {

        }

        public struct ScoresAndClassNames
        {
            public float Score;
            public string className;
        }

        public class MyClass : IContentProvider
        {
            public MyClass(string _content)
            {
                Content = _content;
            }

            public string Content { get; }
        }

        static void Main(string[] args)
        {

            SummarizerArguments sumargs = new SummarizerArguments()
            {
                Language = "en",
            };
            SummarizedDocument doc = Summarizer.Summarize(
                    new MyClass("Iterative, incremental and evolutionary\r\nMost agile development methods break product development work into small increments that minimize the amount of up-front planning and design. Iterations, or sprints, are short time frames (timeboxes) that typically last from one to four weeks. Each iteration involves a cross-functional team working in all functions: planning, analysis, design, coding, unit testing, and acceptance testing. At the end of the iteration a working product is demonstrated to stakeholders. This minimizes overall risk and allows the product to adapt to changes quickly.[22] An iteration might not add enough functionality to warrant a market release, but the goal is to have an available release (with minimal bugs) at the end of each iteration.[23] Multiple iterations might be required to release a product or new features. Working software is the primary measure of progress.[21]\r\n\r\nEfficient and face-to-face communication\r\nThe principle of co-location is that co-workers on the same team should be situated together to better establish the identity as a team and to improve communication.[24] This enables face-to-face interaction, ideally in front of a whiteboard, that reduces the cycle time typically taken when questions and answers are mediated through phone, persistent chat, wiki, or email.[25]\r\n\r\nNo matter which development method is followed, every team should include a customer representative (\"Product Owner\" in Scrum). This person is agreed by stakeholders to act on their behalf and makes a personal commitment to being available for developers to answer questions throughout the iteration. At the end of each iteration, stakeholders and the customer representative review progress and re-evaluate priorities with a view to optimizing the return on investment (ROI) and ensuring alignment with customer needs and company goals.\r\n\r\nIn agile software development, an information radiator is a (normally large) physical display located prominently near the development team, where passers-by can see it. It presents an up-to-date summary of the product development status.[26][27] A build light indicator may also be used to inform a team about the current status of their product development.\r\n\r\nVery short feedback loop and adaptation cycle\r\nA common characteristic in agile software development is the daily stand-up (also known as the daily scrum). In a brief session, team members report to each other what they did the previous day toward their team's iteration goal, what they intend to do today toward the goal, and any roadblocks or impediments they can see to the goal.[28]\r\n\r\nQuality focus\r\nSpecific tools and techniques, such as continuous integration, automated unit testing, pair programming, test-driven development, design patterns, behavior-driven development, domain-driven design, code refactoring and other techniques are often used to improve quality and enhance product development agility.[29] This is predicated on designing and building quality in from the beginning and being able to demonstrate software for customers at any point, or at least at the end of every iteration.[30]"),
                    sumargs
                );
            string summary = string.Join("\r\n\r\n", doc.Sentences.ToArray());
            // do some stuff with summary. It is your result.
            Console.WriteLine(summary);

            while (true) ;

            Environment.Exit(0);

            
            // Create the ML object
            mlContext = CreateMachineLearningContext();


            for (int i = 0; i < 1; i++)
            {
                // <== Comment this if loading model ==

//                // Generate dataset from Generator
//                //            Generator.Generate();
//
//                // Read data from a file and load into memory
                trainingData = ReadData();

                // See QuestionData & QuestionPrediction for more info
                pipeline = SetupPipeline();


                // Train from the dataset, generating a model from the training.
                model = CreateModel();

                //            LogObject(model.Transform(trainingData));

                // Saves the updated model to a .zip file for loading
                SaveModel();
//                // == End ==/>
//
//                // Loads the model
//                // Comment if the lines above are commented
//                model = LoadModel();

                // Use the model for one-time prediction
                predictor = model.CreatePredictionEngine<QuestionData, QuestionPrediction>(mlContext);

                // Save model metrics, to see how well it performs vs older models
                SaveMetrics();
            }

//
            while (true)
            {
                

                Console.Write("Stel een vraag: ");
                var input = Console.ReadLine();
                var stopwatch = Stopwatch.StartNew();
                Console.WriteLine("PREDICTING..");
                QuestionPrediction prediction = predictor.Predict(
                    new QuestionData(input));

//                List<ScoresAndClassNames> scoresAndClassNames = new List<ScoresAndClassNames>();
//                for (int i = 0; i < prediction.Scores.Length; i++)
//                {
//                    scoresAndClassNames.Add(new ScoresAndClassNames()
//                    {
//                        className = Generator.Answers.ElementAt(i).Key,
//                        Score = prediction.Scores[i]
//                    });
//                }
//
//                var sortedScoresClassNames = scoresAndClassNames.OrderByDescending(s => s.Score)
//                    .Take(3)
//                    .ToList();
//                
//                Console.WriteLine(Divider);
//                Console.WriteLine("I'm {0}% sure this is correct. \n{1}", sortedScoresClassNames[0].Score * 100, Generator.Answers[sortedScoresClassNames[0].className]);
//                foreach (var sortedScoresClassName in sortedScoresClassNames)
//                {
//                    LogObject(sortedScoresClassName);
//                }
//                Console.WriteLine(Divider);


                Console.WriteLine("PREDICTED IN: {0} tick(s) and {1} seconds", stopwatch.ElapsedTicks,
                    stopwatch.Elapsed.TotalSeconds);
                LogObject(prediction);
            }
        }
    }
}