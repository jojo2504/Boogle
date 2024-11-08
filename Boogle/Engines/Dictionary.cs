using System;
using System.Collections.Generic;
using System.IO;

namespace Boogle.Engines
{
    public class Dictionary
    {
        HashSet<string> _wordSet = new HashSet<string>(); //store all words in a set to 
        string filePath;

        public Dictionary(string language)
        {
            switch (language){
                case "en":
                    filePath = "../Boogle/Utils/english_dictionary";
                    break;
                case "fr":
                    filePath = "../Boogle/Utils/french_dictionary";
                    break;
            }

            if (filePath == null) return;

            // Read all lines from file
            string words = File.ReadAllLines(filePath)[0];
            
            // Split the text into words (assuming they are separated by spaces)
            string[] wordArray = words.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in wordArray)
            {
                _wordSet.Add(word);
            }
        }

        public void Print()
        {
            Console.WriteLine(_wordSet.Count);
        }

        public string FilePath { get { return filePath; } }
    }
}