using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab3_4
{
    class Program
    {
        static void Main(string[] args) { 
            string directoryPath = @"C:\TextFiles";
            Func<string, IEnumerable<string>> tokenizeDelegate = TokenizeText;
            Func<IEnumerable<string>, IDictionary<string, int>> wordFrequencyDelegate = CalculateWordFrequency;
            Action<IDictionary<string, int>> displayFrequencyDelegate = DisplayWordFrequency;
            List<string> allWords = new List<string>();

            foreach (string filePath in Directory.GetFiles(directoryPath, "*.txt")) { 
                string fileContent = File.ReadAllText(filePath);

                IEnumerable<string> words = tokenizeDelegate(fileContent);

                allWords.AddRange(words);
            }
            IDictionary<string, int> wordFrequencies = wordFrequencyDelegate(allWords);
            displayFrequencyDelegate(wordFrequencies);
        }

        static IEnumerable<string> TokenizeText(string text) { 
            return text.Split(new[] { ' ', '.', ',', '!', '?', ':', ';', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        static IDictionary<string, int> CalculateWordFrequency(IEnumerable<string> words)
        {
            return words
                .GroupBy(word => word, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        static void DisplayWordFrequency(IDictionary<string, int> wordFrequencies)
        {
            foreach (var pair in wordFrequencies.OrderByDescending(pair => pair.Value))
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }
    }
}
