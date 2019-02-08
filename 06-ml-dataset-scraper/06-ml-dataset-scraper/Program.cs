using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using _06_ml_dataset_scraper.DataStructures;

namespace _06_ml_dataset_scraper
{
    class Program
    {
        private static RestClient client = new RestClient("http://api.stackexchange.com");

        public static List<string> keys = new List<string>()
        {
            "0)mf4TMTBFaG9dcFyL6fjA((",
            "Mb2yZmEV6RsEouf0VmUfjw((",
            "o2*iclpIeV5zy*PrNLicWQ((",
            "bDYlnb8qS6dYRlsOpKPXiw((",
            "mFfrDnc5wzsVJjpH)hNMzg((",
            "ry4NvFDSKlwX7yE5K3C)zw((",
            "CdKtPMujuBzu8zdbxrouZw((",

            // Old keys
//            "gxJS*Wle6xPIyfNunHxJFg((",
//            "Leo*1RuvVm5e24hPGIEByQ((",
//            "u7IcmYUsDX3euw6rqiENCQ((",
//            "TkBD8qEw2LLWa)*ZazEfjw((",
//            "ChZNg9j6JB9ZX6fSSNzp5g((",
//            "K4ckBUjQWlJJqNPVA4ffuA((",
//            "9LtspeQ0sG*V9RwNaWwzFA(("
        };

        // Add links[0].Value + $"&page={page}"
        public static Dictionary<string, string> links = new Dictionary<string, string>()
        {
            {
                "stackoverflow_javascript",
                $"https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=javascript&site=stackoverflow&pagesize=100&key={keys[0]}"
            },
            {
                "stackoverflow_php",
                $"https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=php&site=stackoverflow&pagesize=100&key={keys[1]}"
            },
            {
                "stackoverflow_jquery",
                $"https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=jquery&site=stackoverflow&pagesize=100&key={keys[2]}"
            },
            {
                "stackoverflow_css",
                $"https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=css&site=stackoverflow&pagesize=100&key={keys[3]}"
            },
            {
                "stackoverflow_html",
                $"https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=html&site=stackoverflow&pagesize=100&key={keys[4]}"
            },
            {
                "stackoverflow_mysql",
                $"https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=mysql&site=stackoverflow&pagesize=100&key={keys[5]}"
            },
            {
                "stackoverflow_sql",
                $"https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=sql&site=stackoverflow&pagesize=100&key={keys[6]}"
            },
        };


        static void Main(string[] args)
        {
            Console.WriteLine("Hello world i am not a ddos script, promised");

            //            var response = client.Get<QuestionList>(new RestRequest("/2.2/questions?order=desc&sort=activity&tagged=scrum&site=softwareengineering&pagesize=100&page=1&key=gxJS*Wle6xPIyfNunHxJFg(("));
            //
            //
            //            var val = links["stackoverflow_javascript"];
            //
            //            Console.WriteLine(val);
            //            Console.WriteLine(JsonConvert.SerializeObject());
            //
            ////            var json = JsonConvert.SerializeObject(DoRequest(1, (page) => $"/2.2/questions?order=desc&sort=activity&tagged=scrum&site=softwareengineering&pagesize=100&key=gxJS*Wle6xPIyfNunHxJFg((&page={page}"), Formatting.Indented);
            //
            ////            File.WriteAllText("./ml-data.json", json);
            //
            //
            ////
            /// DoRequest(1,
            //                (page) => $"{links["stackoverflow_javascript"]}&page={page}")

            RunNetworkRequests();
          

            Console.WriteLine("done");
            // For when its done a nice beep :)
            SystemSounds.Beep.Play();
            while (true) ;
        }


        static void RunNetworkRequests()
        {
            Task<List<Question>>[] tasks = new Task<List<Question>>[links.Count];
            int counter = 0;
            foreach (var link in links)
            {
                Console.WriteLine($"starting network requester for link: {link.Key}");
                Task<List<Question>> task = Task.Factory.StartNew(() =>
                {
                    // var bla = []
                    // bool has_more = false;
                    // while (has_more) {
                    // has_more = DoRequest()..
                    // if (has_more) => DoRequest()
                    // }

                    List<Question> _questions = new List<Question>();
                    bool has_more = true;
                    int page = 1;
                    while (has_more)
                    {
                        List<Question> newQuestions;
                        (has_more, newQuestions) = DoRequest(page, (page_) => $"{link.Value}&page={page_}");
                        // Add new questions to current questions
                        
                        foreach (var q in newQuestions)
                        {
                            _questions.Add(q);
                        }

                        // something something RAM and throttling
                        if (page > 900)
                        {
                            has_more = false;
                        }
                        page++;
                    }
                    Console.WriteLine($"Task {link.Key} done! Expect one less number in network requests page logging.");
                    return _questions;
                });
                tasks.SetValue(task, counter);
                counter++;
            }

            Task.WaitAll(tasks);
            
            List<Question> allQuestions = new List<Question>();
            foreach (var task in tasks)
            {
                foreach (var question in task.Result)
                {
                    allQuestions.Add(question);
                }
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var question in allQuestions)
            {
                stringBuilder.AppendLine($"{question.question_id}ψ{question.title}ψ{question.question_id}");
            }

            File.WriteAllText("../../ml-data.json", JsonConvert.SerializeObject(allQuestions, Formatting.Indented));
            File.WriteAllText("../../ml-data.txt", stringBuilder.ToString());
        }

        static (bool has_more, List<Question> newQuestions) DoRequest(int page, Func<int, string> path)
        {
            Console.WriteLine($"Doing request {page}");
            var response = client.Get<QuestionList>(new RestRequest(path(page))).Data;
            // Otherwise this is a ddos script
            Thread.Sleep(2000);
            return (has_more: response.has_more, newQuestions: response.items);
        }
        
        static void LogObject(object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}