using System;

namespace Boogle.Engines
{
    public class Board
    {
        private int boardWidth;
        private int boardHeight;
        private int caseNumbers;
        private Dice[,] board;

        public Board(int boardWidth, int boardHeight)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            caseNumbers = boardWidth * boardHeight;
            board = new Dice[boardWidth, boardHeight];
        }
        
        /// <summary>
        /// Generate a random capital letter in each case of the board 
        /// [TODO] Adding weight to randomness to have more vowels than some other consonants
        /// </summary>
        public void BoardGenerator()
        {
            for (int row = 0; row < boardWidth; row++)
            {
                for (int col = 0; col < boardHeight; col++)
                {
                    board[row, col] = new Dice();
                }
            }
        }
        /// <summary>
        /// Console Debugging Purpose
        /// </summary>
        public void PrintBoard()
        {
            int index = 0;
            foreach (Dice dice in board)
            {
                Console.Write("{0} ", dice.CurrentLetter);
                index++;
                if (index % boardWidth == 0 && index < caseNumbers)
                {
                    Console.WriteLine();
                }
            }
        }
        
        public int BoardWidth
        {
            get { return boardWidth; }
            set { boardWidth = value; }
        }

        public int BoardHeight
        {
            get { return boardHeight; }
            set { boardHeight = value; }
        }
    }
}