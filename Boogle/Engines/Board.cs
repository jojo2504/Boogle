using System;

namespace Boogle.Engines
{
    internal class Board
    {
        int _boardWidth;
        int _boardHeight;
        int _caseNumbers;
        SortedList<string, string> _validWords;
        Dice[,] _board;
        char[,] _matrix;
        Dictionary _dictionary;
        MatrixGraphBuilder<char> _graph = new MatrixGraphBuilder<char>(includeDiagonals: true);

        public Board(int boardWidth = 4, int boardHeight = 4)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _caseNumbers = boardWidth * boardHeight;
            _board = new Dice[boardWidth, boardHeight];
            _matrix = new char[_boardWidth,_boardHeight];

            BoardGenerator();
        }
        
        /// <summary>
        /// Generate a random capital letter in each case of the board 
        /// [TODO] Adding weight to randomness to have more vowels than some other consonants
        /// </summary>
        public void BoardGenerator()
        {
            for (int row = 0; row < _boardWidth; row++)
            {
                for (int col = 0; col < _boardHeight; col++)
                {
                    _board[row, col] = new Dice();
                    _matrix[row, col] = _board[row, col].CurrentLetter;
                }
            }
            _graph.SetMatrix(_matrix);
        }

        public void UpdateMatrix(){
            for (int row = 0; row < _boardWidth; row++)
            {
                for (int col = 0; col < _boardHeight; col++)
                {
                    _matrix[row, col] = _board[row, col].CurrentLetter;
                }
            }
            _graph.SetMatrix(_matrix);
        }

        /// <summary>
        /// Console Debugging Purpose
        /// </summary>

        public override string ToString(){
            int index = 0;
            string chainDescribeBoard = "";
            foreach (Dice dice in _board)
            {
                chainDescribeBoard += string.Format("{0} ", dice.CurrentLetter);
                index++;
                if (index % _boardWidth == 0 && index < _caseNumbers)
                {
                    chainDescribeBoard += "\n";
                }
            }
            return chainDescribeBoard;    
        }

        public string toString()
        {
            return string.Format("This board is a {0}*{1} with {3} dices.",_boardHeight,_boardWidth,_caseNumbers);
        }

        public bool checkValidWord(string word){
            return _dictionary.RechDichoRecursif(_validWords, word);
        }

        /// <summary>
        /// Trying to find every valid words based on the current board.
        /// 
        /// This algorithm will use a Depth-First Search (DFS) approach in graph theory by cutting impossible non existant prefixes
        /// through the board.
        /// 
        /// For this purpose, we will implement a new class called Graph which can convert a 2 dimensional matrix into a graph.
        /// and will implement a trie data structure to track prefixes of all words composed in the language dictionaries.
        /// 
        /// By implementing the Trie Class, RechDichoRecursif becomes irrelevent to use on the whole dictionary but can be used instead
        /// on the output of the 'getAllValidWordsInBoard()' method which contains every possible words playable on the board.
        /// </summary>
        /// <param name="trieNode"></param>
        /// <returns></returns>
        public SortedList<string,string> getAllValidWordsOnBoard(TrieNode trieNode, char[,]? testBoard = null){
            if (testBoard != null){
                _boardHeight = testBoard.GetLength(0);
                _boardWidth = testBoard.GetLength(1);
                _graph.SetMatrix(testBoard);
            }
            SortedList<string,string> sortedWordList = new();
            bool[,] visited;

            //loop through all the key node and perform dfs on every one of them
            foreach (Node<char> node in _graph.Graph.Keys){
                //Console.Write("{0} ", node.Value);
                //every start of path starts as an empty string prefix
                visited = new bool[_boardWidth,_boardHeight];
                DFS(visited, node, string.Empty); 
            }

            void DFS(bool[,] visited, Node<char> node, string currentPrefix){
                // Check if this node has already been visited
                if (visited[node.Row, node.Col]){
                    return;
                }
                // update the current visited nodes
                visited[node.Row, node.Col] = true;

                string dfsPrefix = currentPrefix + node.Value;

                // check if the prefix exists in the trie
                // if not, end the dfs at this node
                if (!Trie.SearchPrefix(trieNode, dfsPrefix)){
                    visited[node.Row, node.Col] = false;
                    return;
                }

                if (Trie.SearchKey(trieNode, dfsPrefix)){
                    // if the current prefix matches one of the final word in the trie, then add it to the list of possible words
                    sortedWordList[dfsPrefix] = dfsPrefix;
                }

                // Recursively visit all adjacent vertices
                // that are not visited yet
                if (_graph.Graph.TryGetValue(node, out HashSet<Node<char>> neighbors)){
                    foreach (Node<char> neighbor in neighbors){
                        // Create a copy of the visited array to pass to recursive calls
                        bool[,] newVisited = (bool[,])visited.Clone();
                        
                        // Recursively visit unvisited neighbors
                        DFS(newVisited, neighbor, dfsPrefix);
                    }
                }
            }
            return sortedWordList;
        }
        
        public void RollAllDices(){
            foreach (Dice dice in _board){
                dice.RollDice();
            }
            UpdateMatrix();
        }

        public int BoardWidth => _boardWidth;
        public int BoardHeight => _boardHeight;
        public Dice this[int x, int y] => _board[x,y];
        public Dictionary Dictionary{
            set { 
                _dictionary = value;
                _validWords = getAllValidWordsOnBoard(_dictionary.Root);
            }
            get { return _dictionary;}
        }
        public SortedList<string, string> ValidWords => _validWords;
    }
}