using System;
using System.Collections.Generic;
using System.IO;

namespace Boogle.Engines
{
    public class Dictionary
    {
        readonly HashSet<string> _wordSet = []; //store all words in a set 
        readonly string _filePath;

        readonly string _language;

        public Dictionary(string language)
        {
            switch (language){
                case "en":
                    _filePath = Path.Combine("..", "Boogle", "Utils", "english_dictionary");
                    break;
                case "fr":
                    _filePath = Path.Combine("..", "Boogle", "Utils", "french_dictionary");
                    break;
                default:
                    throw new ArgumentException("Unsupported language.");
            } 

            _filePath = Path.GetFullPath(_filePath);
            _language = language;
            InitWords();
        }

        public void InitWords(){
            string words = File.ReadAllLines(_filePath)[0];
            
            // Split the text into words (assuming they are separated by spaces)
            string[] wordArray = words.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in wordArray)
            {
                _wordSet.Add(word);
            }
        }

        public void toString(){
            Dictionary<int, int> wordsByLength = new Dictionary<int, int>();
            Dictionary<char, int> wordsByLetter = new Dictionary<char,int>();
            foreach (string word in _wordSet){
                if (!wordsByLength.Keys.Contains(word.Length)){
                    wordsByLength.Add(word.Length, 1);
                }
                else{
                    wordsByLength[word.Length]++;
                }

                if (!wordsByLetter.Keys.Contains(word[0])){
                    wordsByLetter.Add(word[0], 1);
                }
                else{
                    wordsByLetter[word[0]]++;
                }
            }
            
            foreach (KeyValuePair<int, int> kvp in wordsByLength){
                Console.WriteLine(string.Format("There is {0} words of length {1}", kvp.Value, kvp.Key));
            }
            foreach (KeyValuePair<char, int> kvp in wordsByLetter){
                Console.WriteLine(string.Format("There is {0} words which starts with the letter {1}", kvp.Value, kvp.Key));
            }
            
            string currentLanguage;
            switch (_language){
                case "en":
                    currentLanguage = "english";
                    break;
                case "fr":
                    currentLanguage = "french";
                    break;
                default:
                    throw new ArgumentException("Unsupported language.");
            } 
            Console.WriteLine(string.Format("There is {0} words in {1}", _wordSet.Count, currentLanguage));
        }

        public void Print()
        {
            Console.WriteLine(_wordSet.Count);
        }

        public string FilePath { get { return _filePath; }}
    }
}