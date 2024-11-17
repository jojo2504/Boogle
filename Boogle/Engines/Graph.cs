using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace Boogle.Engines
{   
    public class Node<T>
    {
        public T Value { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public Node(T value, int row, int col)
        {
            Value = value;
            Row = row;
            Col = col;
        }

        // Override Equals and GetHashCode to consider position
        public override bool Equals(object obj)
        {
            if (obj is Node<T> other)
            {
                return Value.Equals(other.Value) && 
                    Row == other.Row && 
                    Col == other.Col;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Row, Col);
        }

        // Optional: Nice string representation
        public override string ToString()
        {
            return $"{Value}({Row},{Col})";
        }
    }

    public class Graph<T>{
        Dictionary<Node<T>, HashSet<Node<T>>> _edgeTo;

        public Graph(){
            _edgeTo = new Dictionary<Node<T>, HashSet<Node<T>>>();
        }

        public Dictionary<Node<T>, HashSet<Node<T>>> ConvertMatrixToGraph(T[,] matrix){
            for (int r = 0; r < matrix.GetLength(0); r++) {
                for (int c = 0; c < matrix.GetLength(1); c++) {
                    CreateNeighbor(r, c, matrix);
                }
            }
            return _edgeTo;
        }

        private void CreateNeighbor(int r, int c, T[,] matrix2) 
        {   
            Node<T> currentChar = new Node<T>(matrix2[r,c], r, c);  // Get the current character

            for (int row = -1; row <= 1; row++) 
            {
                for (int col = -1; col <= 1; col++) 
                {
                    // avoid the center cell
                    if (row == 0 && col == 0) {
                        continue;
                    }

                    // outside matrix
                    if ((0 > c + col) || (c + col >= matrix2.GetLength(1)) || 
                        (0 > r + row) || (r + row >= matrix2.GetLength(0))) {
                        continue;
                    }

                    Node<T> neighborChar = new Node<T>(matrix2[r + row,c + col], r + row, c + col);
                    // Add edge from current char to neighbor char
                    Add(currentChar, neighborChar);
                }
            }
        }

        public void Add(Node<T> from, Node<T> to)
        {
            if (!_edgeTo.ContainsKey(from))
            {
                _edgeTo[from] = new HashSet<Node<T>>();
            }
            _edgeTo[from].Add(to);
        }

        public HashSet<Node<T>> GetNeighbors(Node<T> vertex)
        {
            return _edgeTo.GetValueOrDefault(vertex, new HashSet<Node<T>>());
        }

        public string ToString(){
            foreach (KeyValuePair<Node<T>, HashSet<Node<T>>> kpv in _edgeTo){
                string line = "";
                foreach (Node<T> node in kpv.Value){
                    line += string.Format("{0}, ", node);
                }
                Console.WriteLine("{0} => {1}", kpv.Key, line);
            }
            return "";
        }
    }
}