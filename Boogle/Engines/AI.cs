namespace Boogle.Engines{
    public class AI{
        Board _board;
        int _score = 0;
        public AI(Board board){
            _board = board;
        }
        /// <summary>
        /// Create a sortedList with all valid words on the board
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public SortedList<string,string> Play(char[,]? board = null){
            return _board.getAllValidWordsOnBoard(_board.Dictionary.Root, board);
        }

        public Board Board { get; internal set; }
        public int Score { get; internal set; }
    }
}