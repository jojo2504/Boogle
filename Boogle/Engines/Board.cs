using System;

namespace Boogle.Engines
{
    internal class Board
    {
        int _boardWidth;
        int _boardHeight;
        int _caseNumbers;
        Dice[,] _board;

        public Board(int boardWidth = 4, int boardHeight = 4)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _caseNumbers = boardWidth * boardHeight;
            _board = new Dice[boardWidth, boardHeight];
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
            chainDescribeBoard += "\n";  
            return chainDescribeBoard;          
        }

        public string toString()
        {
            return string.Format("This board is a {0}*{1} with {3} dices.",_boardHeight,_boardWidth,_caseNumbers);
        }
        
        public void RollAllDices(){
            foreach (Dice dice in _board){
                dice.RollDice();
            }
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