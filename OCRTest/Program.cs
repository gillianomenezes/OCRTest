using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.ProjectOxford.Vision;
using System.IO;
using Microsoft.ProjectOxford.Vision.Contract;

namespace OCRTest
{
    class Program
    {
        private static string API_key = ConfigurationManager.AppSettings["skeys"];
        private static string API_location = "https://brazilsouth.api.cognitive.microsoft.com/vision/v1.0";

        public static void Main(string[] args)
        {
            try
            {
                //string imgToAnalyze = args[1];
                string imgToAnalyze = @"C:\Users\g.silva.de.menezes\Desktop\CNH.jpeg";

                TextExtraction(imgToAnalyze, false);

                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        public static void PrintResults(string[] res)
        {
            foreach (string r in res)
                Console.WriteLine(r);
        }

        public static void TextExtraction(string fname, bool wrds)
        {
            Task.Run( async () => {
                string[] res = await TextExtractionCore(fname, wrds);
                PrintResults(res);
            }).Wait();
        }

        public static async Task<string[]> TextExtractionCore(string fname, bool wrds)
        {
            VisionServiceClient client = new VisionServiceClient(API_key, API_location);
            string[] textres = null;

            if (File.Exists(fname))
            {
                using(Stream stream = File.OpenRead(fname))
                {
                    OcrResults res = await client.RecognizeTextAsync(stream, "pt", false);
                    textres = GetExtracted(res, wrds);
                }
            }

            return textres;
        }

        public static string[] GetExtracted(OcrResults res, bool wrds)
        {
            List<string> items = new List<string>();

            foreach(Region region in res.Regions)
            {
                foreach(Line line in region.Lines)
                {
                    if (wrds)
                        items.AddRange(GetWords(line));
                    else
                        items.Add(GetLineAsString(line));
                }
            }

            return items.ToArray();
        }

        private static List<string> GetWords(Line line)
        {
            List<string> words = new List<string>();

            foreach(Word word in line.Words)
                words.Add(word.Text);

            return words;
        }

        private static string GetLineAsString(Line line)
        {
            List<string> words = GetWords(line);
            return words.Count > 0 ? string.Join(" ", words) : string.Empty;
        }
    }
}