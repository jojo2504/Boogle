namespace Boogle.Engines{
    class AI{
        Board _board;
        public AI(Board board){
            _board = board;
        }
        public SortedList<string,string> Play(char[,]? board = null){
            return _board.getAllValidWordsOnBoard(_board.Dictionary.Root, board);
        }
    }
}