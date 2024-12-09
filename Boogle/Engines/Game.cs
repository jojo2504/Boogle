namespace Boogle.Engines
{
    internal class Game
    {
        Board _board;
        public List<string> _languages = [];
        Player _player1;
        Player? _player2;
        Clock _clock;
        AI _ai;

        //primary constructor
        public Game(Board board, List<string> languages){
            _board = board;
            _board.Dictionary = new Dictionary(languages);
            _clock = new Clock();
            _ai = new AI(_board);
            _languages = languages;
        }
        
        public Game(int boardWidth, int boardHeight, List<string> languages)
            : this(new Board(boardWidth, boardHeight), languages) // Calls the primary constructor after creating new board
        {
        }

        public Game(List<string> languages): this(new Board(), languages) // Constructor if nothing is specified except languages
        {
        }

        public void StartClock(){
            _clock.Start();
        }

        public void Start(){
            throw new NotImplementedException();
        }

        public void NextTurn(){
            throw new NotImplementedException();
        }

        public void CheckWin(){
            throw new NotImplementedException();
        }

        public int AddPoint(string word){
            throw new NotImplementedException();
        }

        public Player CheckWinner(Player player1, Player player2){
            return player1.Score > player2.Score ? player1 : player2;
        }

        public Board Board{
            get{return _board;}
        }

        public Clock Clock{
            get{return _clock;}
        }

        public AI Ai => _ai;
        public List<string> Languages => _languages;
    }
}