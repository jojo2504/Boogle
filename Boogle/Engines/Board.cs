using System;

namespace Boogle.Engines
{
    public class Board
    {
        private int boardWidth;
        private int boardHeight;
        private int caseNumbers;
        private char[,] board;

        public Board(int boardWidth, int boardHeight)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            caseNumbers = boardWidth * boardHeight;
            board = new char[boardWidth, boardHeight];
        }
        
        /// <summary>
        /// Generate a random capital letter in each case of the board 
        /// [TODO] Adding weight to randomness to have more vowels than some other consonants
        /// </summary>
        public void BoardGenerator()
        {
            Random random = new Random();
            int num;
            char randomLetter;
            for (int row = 0; row < boardWidth; row++)
            {
                for (int col = 0; col < boardHeight; col++)
                {
                    num = random.Next(0, 26);
                    randomLetter = (char)('A' + num);
                    board[row, col] = randomLetter;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void PrintBoard()
        {
            int index = 0;
            foreach (char letter in board)
            {
                Console.Write(letter);
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