using System;
using System.Collections.Generic;
using System.IO;

namespace Boogle.Engines
{
    public class Dictionary
    {
        readonly List<string> _wordList = []; //store all words in a list 
        readonly string _filePath;
        Dictionary<int, List<string>> _prefixDictionary;

        TrieNode _root = new TrieNode();

        public Dictionary(List<string> languageIDs){
            foreach (string languageID in languageIDs){
                Console.WriteLine("" + languageID);
                if (languageID == "en"){
                    _filePath = Path.Combine("..", "Boogle", "Utils", "english_dictionary");
                    _filePath = Path.GetFullPath(_filePath);
                    InitWords(_filePath);
                }
                if (languageID == "fr"){
                    _filePath = Path.Combine("..", "Boogle", "Utils", "french_dictionary");
                    _filePath = Path.GetFullPath(_filePath);
                    InitWords(_filePath);
                }
                if (_wordList.Count > 0){
                    _wordList.Distinct().ToList();
                }
                _wordList.Sort();
            }
        }

        public void InitWords(string filePath){
            string words = File.ReadAllLines(filePath)[0];
            
            // Split the text into words (assuming they are separated by spaces)
            string[] wordArray = words.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in wordArray)
            {
                if (word.Contains("'")) continue;
                Trie.InsertKey(_root, word);
                _wordList.Add(word);
            }
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
            string chainDescribeDictionary = "";
            foreach (KeyValuePair<int, int> kvp in wordsByLength){
                chainDescribeDictionary += string.Format("There is {0} words of length {1}\n", kvp.Value, kvp.Key);
            }
            foreach (KeyValuePair<char, int> kvp in wordsByLetter){
                chainDescribeDictionary += string.Format("There is {0} words which starts with the letter {1}\n", kvp.Value, kvp.Key);
            }

            chainDescribeDictionary += string.Format("There is {0} words in the current dictionary", _wordList.Count);
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

        public string FilePath => _filePath;
        public Dictionary<int, List<string>> PrefixDictionary => _prefixDictionary;
        public TrieNode Root => _root;
    }
}