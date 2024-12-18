namespace Boogle.Engines{
    public class AI{
        Board _board;
        int _score = 0;
        public AI(Board board){
            _board = board;
        }
        public SortedList<string,string> Play(char[,]? board = null){
            return _board.getAllValidWordsOnBoard(_board.Dictionary.Root, board);
        }
        public Board Board { get; internal set; }
        public int Score { get; internal set; }
    }
}