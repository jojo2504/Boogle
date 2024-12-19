using System.Text;

namespace Boogle.Engines{
    public class MatrixGraphBuilder<T>
    {
        readonly Dictionary<Node<T>, HashSet<Node<T>>> _graph = new Dictionary<Node<T>, HashSet<Node<T>>>();
        readonly bool _includeDiagonals;

        public MatrixGraphBuilder(T[,] matrix, bool includeDiagonals = false){
            _includeDiagonals = includeDiagonals;
            ConvertMatrixToGraph(matrix);
        }

        public MatrixGraphBuilder(bool includeDiagonals = false){
            _includeDiagonals = includeDiagonals;
        }

        /// <summary>
        /// Convert given 2D matrix into its graph
        /// </summary>
        /// <param name="matrix"></param>
        private void ConvertMatrixToGraph(T[,] matrix)
        {
            if (matrix == null){
                throw new ArgumentNullException("");
            }
            for (int r = 0; r < matrix.GetLength(0); r++)
            {
                for (int c = 0; c < matrix.GetLength(1); c++)
                {
                    CreateNeighbor(r, c, matrix);
                }
            }
        }

        /// <summary>
        /// Create neighbors of a given node in the matrix as store it in a asjacency list
        /// Excludes itself, outside matrix's positions and diagonals (with paramaters)
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <param name="matrix2"></param>
        private void CreateNeighbor(int r, int c, T[,] matrix2) 
        {   
            Node<T> currentChar = new Node<T>(matrix2[r,c], r, c);
            for (int row = -1; row <= 1; row++) 
            {
                for (int col = -1; col <= 1; col++) 
                {
                    if (row == 0 && col == 0){
                        continue;
                    }
                    if ((0 > c + col) || (c + col >= matrix2.GetLength(1)) || 
                        (0 > r + row) || (r + row >= matrix2.GetLength(0))) {
                        continue;
                    }
                    if (!_includeDiagonals){
                        if (Math.Abs(row) == 1 && Math.Abs(col) == 1) {
                            continue;
                        }
                    }
                    Node<T> neighborChar = new Node<T>(matrix2[r + row,c + col], r + row, c + col);
                    Add(currentChar, neighborChar);
                }
            }
        }

        /// <summary>
        /// Add a node into the adjacency list of another node
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void Add(Node<T> from, Node<T> to)
        {
            if (!_graph!.ContainsKey(from)) 
                _graph[from] = new HashSet<Node<T>>();
            _graph[from].Add(to);
        }

        /// <summary>
        /// Clear the graph
        /// </summary>
        public void Clear(){
            if (_graph == null) return;
            _graph.Clear();
        }

        /// <summary>
        /// Method to change graph with another matrix
        /// </summary>
        /// <param name="matrix"></param>
        public void SetMatrix(T[,] matrix){
            Clear();
            ConvertMatrixToGraph(matrix);
        }

        /// <summary>
        /// Create string to display the adjacency list
        /// </summary>
        /// <returns></returns>
        public override string ToString(){
            if (_graph == null) return "";

            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<Node<T>, HashSet<Node<T>>> kpv in _graph)
            {
                stringBuilder.Append($"{kpv.Key} => ");
                foreach (Node<T> node in kpv.Value)
                {
                    stringBuilder.Append($"{node}, ");
                }
                if (kpv.Value.Count > 0)
                {
                    stringBuilder.Length -= 2;
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString().TrimEnd();
        }

        public Dictionary<Node<T>, HashSet<Node<T>>> Graph => _graph;
        public bool IncludeDiagonals => _includeDiagonals;
    }

    /// <summary>
    /// Node structure for the graph
    /// </summary>
    public readonly struct Node<T>
    {
        internal readonly T Value;
        internal readonly int Row;
        internal readonly int Col;

        public Node(T value, int row, int col){
            Value = value;
            Row = row;
            Col = col;
        }

        public override string ToString()
        {
            return $"{Value}({Row},{Col})";
        }
    }
}