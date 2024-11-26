namespace Boogle.Engines{
    public class TrieNode{
        public TrieNode[] Child = new TrieNode[26];
        public bool WordEnd = false;
    }

    public class Trie{
        // Method to insert a key into the Trie
        public static void InsertKey(TrieNode root, string key)
        {
            // Initialize the curr pointer with the root node
            TrieNode curr = root;

            // Iterate across the length of the string
            foreach(char c in key.ToUpper())
            {
                // Check if the node exists for the current
                // character in the Trie
                if (curr.Child[c - 'A'] == null) {
                    // If node for current character does
                    // not exist then make a new node
                    TrieNode newNode = new TrieNode();

                    // Keep the reference for the newly created node
                    curr.Child[c - 'A'] = newNode;
                }

                // Move the curr pointer to the newly created node
                curr = curr.Child[c - 'A'];
            }

            // Mark the end of the word
            curr.WordEnd = true;
        }

        // Method to search a key in the Trie
        public static bool SearchKey(TrieNode root, ReadOnlySpan<char> key)
        {
            // Initialize the curr pointer with the root node
            TrieNode curr = root;

            // Iterate across the length of the string
            foreach(char c in key)
            {
                // Check if the node exists for the current
                // character in the Trie
                if (curr.Child[c - 'A'] == null)
                    return false;

                // Move the curr pointer to the already
                // existing node for the current character
                curr = curr.Child[c - 'A'];
            }

            // Return true if the word exists and
            // is marked as ending
            return curr.WordEnd;
        }

        public static bool SearchPrefix(TrieNode root, ReadOnlySpan<char> key){
            // Initialize the curr pointer with the root node
            TrieNode currentPointer = root;

            // Iterate across the length of the string
            foreach(char currentChar in key)
            {
                // Check if the node exists for the current
                // character in the Trie
                if (currentPointer.Child[currentChar - 'A'] == null)
                    return false;

                // Move the curr pointer to the already
                // existing node for the current character
                currentPointer = currentPointer.Child[currentChar - 'A'];
            }
            //end of the prefix without any problem, so the current passed prefix exist and we can continue
            return true;
        }
    }
}