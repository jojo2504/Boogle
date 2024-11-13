namespace Boogle.Engines
{
    internal class Game
    {
        Board _board;

        public List<string> languages = [];
        Dictionary _dictionary;  // default language : {english "en"} | possible other languages : {french: "fr"}

        Player _player1;
        Player _player2;

        Clock _clock;

        //primary constructor
        public Game(Board board, List<string> languages){
            _board = board;
            _dictionary = new Dictionary(languages);
            _clock = new Clock();
        }
        
        public Game(int boardWidth, int boardHeight, List<string> languages)
            : this(new Board(boardWidth, boardHeight), languages) // Calls the primary constructor after creating new board
        {
        }

        public Game(List<string> languages): this(new Board(), languages) // Constructor if nothing is specified
        {
        }

        public void StartClock(){
            _clock.Start();
        }

        public void Start(){
            
        }

        public void NextTurn(){
            
        }

        public void CheckWin(){}

        public string DictionaryPath{
            get{return _dictionary.FilePath;}
        }

        public Dictionary Dictionary{
            get{return _dictionary;}
        }

        public Board Board{
            get{return _board;}
        }

        public Clock Clock{
            get{return _clock;}
        }
    }
}