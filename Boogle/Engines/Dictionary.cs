using System;
using System.Collections.Generic;
using System.IO;

namespace Boogle.Engines
{
    public class Dictionary
    {
        readonly List<string> _wordList = []; //store all words in a list 
        readonly string _filePath;
        readonly string _languageName;

        public Dictionary(string languageID)
        {
            switch (languageID){
                case "en":
                    _filePath = Path.Combine("..", "Boogle", "Utils", "english_dictionary");
                    _languageName = "english";
                    break;
                case "fr":
                    _filePath = Path.Combine("..", "Boogle", "Utils", "french_dictionary");
                    _languageName = "french";
                    break;
                default:
                    throw new ArgumentException("Unsupported language.");
            } 

            _filePath = Path.GetFullPath(_filePath);
            InitWords();
        }

        public void InitWords(){
            string words = File.ReadAllLines(_filePath)[0];
            
            // Split the text into words (assuming they are separated by spaces)
            string[] wordArray = words.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in wordArray)
            {
                _wordList.Add(word);
            }

            _wordList.Sort();
        }

        public void toString(){
            Dictionary<int, int> wordsByLength = new Dictionary<int, int>();
            Dictionary<char, int> wordsByLetter = new Dictionary<char,int>();
            foreach (string word in _wordList){
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

            Console.WriteLine(string.Format("There is {0} words in {1}", _wordList.Count, _languageName));
        }

        public bool RechDichoRecursif(string word, int left = 0, int right = -1)
        {
            bool Helper(string word, int left, int right)
            {
                if (left > right){
                    return false;
                }

                int mid = (left + right) / 2;
                int comparaison = string.Compare(word, _wordList[mid]);

                if (comparaison == 0){
                    return true;
                }
                else if (comparaison < 0){
                    return Helper(word, left, mid-1);
                }
                else {
                    return Helper(word, mid+1, right);
                }
            }   
        
            // Set right to _wordList.Count - 1 if it’s the default value (-1)
            if (right == -1)
            {
                right = _wordList.Count - 1;
            }
            return Helper(word, left, right);
        } 
        public void Print()
        {
            Console.WriteLine(_wordList.Count);
        }

        public string FilePath { get { return _filePath; }}
    }
}