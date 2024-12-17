using System.IO;
using directoryPath;

namespace Boogle.Engines
{
    public class Dictionary
    {
        readonly List<string> _wordList = []; 
        TrieNode _root = new TrieNode();

        public Dictionary(List<string> languageIDs){
            string filePath;
            foreach (string languageID in languageIDs){
                if (languageID == "en"){
                    filePath = Path.Combine(DirectoryPath.GetSolutionRoot(), "Utils", "english_dictionary");
                    filePath = Path.GetFullPath(filePath);
                    InitWords(filePath);
                }
                if (languageID == "fr"){
                    filePath = Path.Combine(DirectoryPath.GetSolutionRoot(), "Utils", "french_dictionary");
                    filePath = Path.GetFullPath(filePath);
                    InitWords(filePath);
                }
                if (_wordList.Count > 0){
                    _wordList.Distinct().ToList();
                }
                _wordList.Sort();
            }
        }

        public void InitWords(string filePath){
            string words = File.ReadAllLines(filePath)[0];
            
            string[] wordArray = words.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in wordArray)
            {
                if (word.Contains("'")) continue;
                Trie.InsertKey(_root, word);
                _wordList.Add(word);
            }
        }

        public override string ToString(){
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
            return chainDescribeDictionary;
        }

        public bool RechDichoRecursif(SortedList<string, string> validWords, string word, int left = 0, int right = -1)
        {   
            bool Helper(string word, int left, int right)
            {
                if (left > right){
                    return false;
                }

                int mid = (left + right) / 2;
                int comparaison = string.Compare(word, validWords.Keys[mid]);

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
        
            if (right == -1)
            {
                right = validWords.Count - 1;
            }
            return Helper(word, left, right);
        } 

    public static int[] MergeSortArray(int[] arr)
    {
        if (arr.Length <= 1)
            return arr;

        int mid = arr.Length / 2;
        int[] left = new int[mid];
        int[] right = new int[arr.Length - mid];

        Array.Copy(arr, 0, left, 0, mid);
        Array.Copy(arr, mid, right, 0, arr.Length - mid);

        left = MergeSortArray(left);
        right = MergeSortArray(right);

        int[] result = new int[arr.Length];
        int i = 0, j = 0, k = 0;

        while (i < left.Length && j < right.Length)
        {
            if (left[i] < right[j])
                result[k++] = left[i++];
            else
                result[k++] = right[j++];
        }

        while (i < left.Length)
            result[k++] = left[i++];

        while (j < right.Length)
            result[k++] = right[j++];

        return result;
    }

        public List<string> WordList => _wordList;
        public TrieNode Root => _root;
    }
}