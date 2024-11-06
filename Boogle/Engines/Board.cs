using System;

namespace Boogle.Engines
{
    public class Board
    {
        private int boardWidth;
        private int boardHeight;
        private char[,] board;

        public Board(int boardWidth, int boardHeight)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            
            this.board = new char[boardWidth, boardHeight];
        }

        public void BoardGenerator()
        {
            Random random = new Random();
            int num;
            char randomLetter;
            for (int row = 0; row < this.boardWidth; row++)
            {
                for (int col = 0; col < this.boardHeight; col++)
                {
                    num = random.Next(0, 26);
                    randomLetter = (char)('a' + num);
                    this.board[row, col] = randomLetter;
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