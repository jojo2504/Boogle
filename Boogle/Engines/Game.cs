namespace Boogle.Engines
{
    public class Game
    {
        Board _board;
        Dictionary dictionary;  // default language : english "en" | possible other languages : {french: "fr"}

        Player player1;
        Player player2;

        public Game(Board board, string language = "en"){ 
            _board = board;
            dictionary = new Dictionary(language);
        }

        public Game(int boardWidth, int boardHeight, string language = "en"){
            _board = new Board(boardWidth, boardHeight);
            dictionary = new Dictionary(language);
        }

        public void NextTurn(){
            //template
        }

        public bool CheckIfWordExistInDictionary(string word){
            //template
            return false;
        }

        public string DictionaryPath{
            get{return dictionary.FilePath;}
        }

        public Dictionary GetDictionary{
            get{return dictionary;}
        }
    }
}