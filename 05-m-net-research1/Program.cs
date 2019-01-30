using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using Microsoft.ML.Model;
using Microsoft.ML.Transforms.Conversions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _05_m_net_research1
{
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


        public class Metric
        {
            public object metrics;
            public DateTime dateTime;
        }
        
        // QuestionData is used to provide training data, and as
        // input for prediction operations
        public class QuestionData
        {
            [LoadColumn(0)] public string Question;

            [LoadColumn(1)] public string Label;
        }

        // QuestionPrediction is the result returned from prediction operations
        public class QuestionPrediction
        {
            [ColumnName("PredictedLabel")] public string PredictedLabel;
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
            var reader = mlContext.Data.CreateTextReader<QuestionData>(separatorChar: ',', hasHeader: false);
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
                    labelColumn: "Label", featureColumn: "Features"
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
            // TODO understand this.
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
            // TODO understand this.
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
             
        static void Main(string[] args)
        {
            // Create the ML object
            mlContext = CreateMachineLearningContext();


            // <== Comment this if loading model ==
//          //  Read data from a file and load into memory
//            trainingData = ReadData();
//
//            // Setup learning pipeline, selecting which algorithm to use and which columns correspond to what.
//            // See QuestionData & QuestionPrediction for more info
//            pipeline = SetupPipeline();
//
//            // Train from the dataset, generating a model from the training.
//            model = CreateModel();
//
//            // Saves the updated model to a .zip file for loading
//            SaveModel();
            // == End ==/>

            // Loads the model
            // Comment if the lines above are commented
            model = LoadModel();

            // Use the model for one-time prediction
            var predictor = model.CreatePredictionEngine<QuestionData, QuestionPrediction>(mlContext);

            // Save model metrics, to see how well it performs vs older models
            SaveMetrics();

            while (true)
            {
                Console.Write("Stel een vraag (AGILE, PHP): ");
                var input = Console.ReadLine();
                var stopwatch = Stopwatch.StartNew();
                Console.WriteLine("PREDICTING..");
                QuestionPrediction prediction = predictor.Predict(
                    new QuestionData()
                    {
                        Question = input,
                    });
                Console.WriteLine("PREDICTED IN: {0} tick(s) and {1} seconds", stopwatch.ElapsedTicks, stopwatch.Elapsed.TotalSeconds);
                LogObject(prediction);
            }
        }
    }
}