using System;
using System.Collections.Generic;
using System.IO;

namespace Boogle.Engines
{
    public class Dictionary
    {
        HashSet<string> _wordSet = new HashSet<string>();

        public Dictionary(string filePath)
        {
            // Read all lines from file
            string words = File.ReadAllLines(filePath)[0];
            
            // Split the text into words (assuming they are separated by spaces)
            string[] wordArray = words.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in wordArray)
            {
                _wordSet.Add(word);
            }
        }

        public void Print()
        {
            Console.WriteLine(_wordSet.Count);
        }
    }
}