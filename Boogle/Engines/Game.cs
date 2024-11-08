namespace Boogle.Engines
{
    public class Game
    {
        Board _board;
        Dictionary _dictionary;  // default language : english "en" | possible other languages : {french: "fr"}

        Player _player1;
        Player _player2;

        // Primary constructor initializes everything
        public Game(Board board, string language = "en")
        {
            _board = board;
            _dictionary = new Dictionary(language);
            _player1 = new Player();
            _player2 = new Player();
        }

        // Secondary constructor chains to primary constructor
        public Game(int boardWidth, int boardHeight, string language = "en")
            : this(new Board(boardWidth, boardHeight), language) // Calls the primary constructor after creating new board
        {
        }

        public Game(string language = "en"): this(new Board(), language) // Constructor if nothing is specified
        {
        }

        public void Start(){

        }

        public void NextTurn(){
            
        }

        public bool CheckIfWordExistInDictionary(string word){
            //template
            return false;
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
    }
}