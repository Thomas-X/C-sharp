using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using Microsoft.ML.Model;
using Microsoft.ML.Transforms.Conversions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        private const string DataLocation = "./ml-data.txt";
        private const string MetricsLocation = "./metrics.json";
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
            return reader.Read(DataLocation);
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
                    labelColumn: "Label", featureColumn: "Features", maxIterations: Program.maxIterations
                ))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
        }

        /// <summary>
        /// Trains the data set and creates a model from training.
        /// </summary>
        public static ITransformer CreateModel()
        {
            
            Logger(new List<string>() {"Training model.."});
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var _model = pipeline.Fit(trainingData);
            Logger(new List<string>()
            {
                $"Estimated model training time was: {watch.ElapsedMilliseconds}ms"
            });
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

        static void Main(string[] args)
        {
            // Create the ML object
            mlContext = CreateMachineLearningContext();



            // <== Comment this if loading model ==

            // Generate dataset from Generator
            Generator.Generate();

            // Read data from a file and load into memory
            trainingData = ReadData();

            // See QuestionData & QuestionPrediction for more info
            pipeline = SetupPipeline();

            // Train from the dataset, generating a model from the training.
            model = CreateModel();

            // Saves the updated model to a .zip file for loading
            SaveModel();
            // == End ==/>

            // Loads the model
            // Comment if the lines above are commented
//            model = LoadModel();

            // Use the model for one-time prediction
            predictor = model.CreatePredictionEngine<QuestionData, QuestionPrediction>(mlContext);

            // Save model metrics, to see how well it performs vs older models
            SaveMetrics();

            while (true)
            {
                

                Console.Write("Stel een vraag: ");
                var input = Console.ReadLine();
                var stopwatch = Stopwatch.StartNew();
                Console.WriteLine("PREDICTING..");
                QuestionPrediction prediction = predictor.Predict(
                    new QuestionData(input));

                List<ScoresAndClassNames> scoresAndClassNames = new List<ScoresAndClassNames>();
                for (int i = 0; i < Generator.Answers.Count; i++)
                {
                    scoresAndClassNames.Add(new ScoresAndClassNames()
                    {
                        className = Generator.Answers.ElementAt(i).Key,
                        Score = prediction.Scores[i]
                    });
                }

                var sortedScoresClassNames = scoresAndClassNames.OrderByDescending(s => s.Score)
                    .Take(3)
                    .ToList();
                
                Console.WriteLine(Divider);
                Console.WriteLine("I'm {0}% sure this is correct. \n{1}", sortedScoresClassNames[0].Score * 100, Generator.Answers[sortedScoresClassNames[0].className]);
                foreach (var sortedScoresClassName in sortedScoresClassNames)
                {
                    LogObject(sortedScoresClassName);
                }
                Console.WriteLine(Divider);


                Console.WriteLine("PREDICTED IN: {0} tick(s) and {1} seconds", stopwatch.ElapsedTicks,
                    stopwatch.Elapsed.TotalSeconds);
//                LogObject(classNames);
            }
        }
    }
}