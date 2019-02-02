using System;
using System.IO;
using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
using Console = System.Console;
using File = System.IO.File;

namespace _06_ml_net_research2
{
    class Program
    {
        static void Main(string[] args)
        {
            var modelsRoot = @"../../stanford";
            var text = "I hate Helen Parkhurst. I also love the UN. I like koala's as well, all animals.";


            // Annotation pipeline configuration
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically

            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(modelsRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);


            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }


            while (true)
            {
                using (var stream2 = new ByteArrayOutputStream())
                {
                    Console.Write("Enter a sentence to NER: ");
                    var input = Console.ReadLine();
                    var annotation2 = new Annotation(input);    
                    pipeline.annotate(annotation2);

                    pipeline.prettyPrint(annotation, new PrintWriter(stream2));
                    Console.WriteLine(stream2.toString());
                    stream2.close();
                }
            } ;
        }
    }
}
