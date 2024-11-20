using System;

namespace Boogle.Engines
{
    internal class Board
    {
        int _boardWidth;
        int _boardHeight;
        int _caseNumbers;
        List<string> validWords = [];
        Dice[,] _board;
        char[,] _matrix;
        MatrixGraphBuilder<char> _graph;

        public Board(int boardWidth = 4, int boardHeight = 4)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _caseNumbers = boardWidth * boardHeight;
            _board = new Dice[boardWidth, boardHeight];
            _matrix = new char[_boardWidth,_boardHeight];
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
            _graph = new MatrixGraphBuilder<char>(_matrix);
        }

        public void UpdateMatrix(){
            for (int row = 0; row < _boardWidth; row++)
            {
                for (int col = 0; col < _boardHeight; col++)
                {
                    _matrix[row, col] = _board[row, col].CurrentLetter;
                }
            }
        }

        /// <summary>
        /// Console Debugging Purpose
        /// </summary>
        public string PrintBoard()
        {
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
            return validWords.Contains(word);
        }

        /// <summary>
        /// Trying to find every valid words based on the current board.
        /// 
        /// This algorithm will use a Depth-First Search (DFS) approach in a graph theory by cutting impossible paths through the board.
        /// We will explore each Depth of the letter then follows path where it is possible.
        /// For this purpose, we will implement a new class called Graph which has a method to convert a 2 dimensional matrix into a graph.
        /// After everyone possible formed words has been found, we will then pass them through the dictionary to know if the word exists or not.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public List<string> getAllValidWordsInBoard(TrieNode trieNode){
            List<string> allFormedString = [];
            
            //loop through all the key node and perform dfs on every one of them
            foreach (Node<char> node in _graph.Graph.Keys){
                bool[,] visited = new bool[_boardWidth,_boardHeight];
                //every start of path starts as an empty string prefix
                string prefix = "";
                DFS(visited, node, prefix);
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
                    allFormedString.Add(dfsPrefix);
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
            return allFormedString.Distinct().ToList();
        }
        
        public void RollAllDices(){
            foreach (Dice dice in _board){
                dice.RollDice();
            }
            UpdateMatrix();
        }
        public int BoardWidth
        {
            get { return _boardWidth; }
            set { _boardWidth = value; }
        }

        public int BoardHeight
        {
            get { return _boardHeight; }
            set { _boardHeight = value; }
        }

        public Dice this[int x, int y]{
            get { return _board[x,y];}
        }
    }
}