namespace Boogle.Engines{
    public class TrieNode{
        public TrieNode[] Child = new TrieNode[26];
        public bool WordEnd = false;
    }

    public class Trie{
        /// <summary>
        /// Insert a new key in the trie
        /// </summary>
        /// <param name="root"></param>
        /// <param name="key"></param>
        public static void InsertKey(TrieNode root, string key)
        {
            TrieNode curr = root;
            foreach(char c in key.ToUpper())
            {
                if (curr.Child[c - 'A'] == null) {
                    TrieNode newNode = new TrieNode();
                    curr.Child[c - 'A'] = newNode;
                }
                curr = curr.Child[c - 'A'];
            }
            curr.WordEnd = true;
        }

        /// <summary>
        /// Search an existing key in the trie
        /// </summary>
        /// <param name="root"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool SearchKey(TrieNode root, ReadOnlySpan<char> key)
        {
            TrieNode curr = root;
            foreach(char c in key)
            {
                if (curr.Child[c - 'A'] == null) return false;
                curr = curr.Child[c - 'A'];
            }
            return curr.WordEnd;
        }

        /// <summary>
        /// Search an existing prefix in the trie
        /// </summary>
        /// <param name="root"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool SearchPrefix(TrieNode root, ReadOnlySpan<char> key){
            TrieNode currentPointer = root;
            foreach(char currentChar in key)
            {
                if (currentPointer.Child[currentChar - 'A'] == null) return false;
                currentPointer = currentPointer.Child[currentChar - 'A'];
            }
            return true;
        }
    }
}