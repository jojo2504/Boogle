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
            _board.Dictionary = new Dictionary(languages);
            _clock = new Clock();
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
            Console.WriteLine("Hello, welcome to the Boogle game :\n\nEnter the name of player 1 :");
            _player1 = new Player(Console.ReadLine());
            Console.WriteLine("Enter the name of player 2 :");
            _player2 = new Player(Console.ReadLine());
            Console.WriteLine("Select the language you want to play with (en or fr):");
            languages.Add(Console.ReadLine());
            _dictionary = new Dictionary(languages);
            _clock = new Clock();
            _board = new Board();
            Console.WriteLine("Enter the number of rounds you want to play : ");
            int remainingRounds = Convert.ToInt32(Console.ReadLine());
            Player currentplayer = _player1;
            while(remainingRounds != 0) 
            {
                Turn(currentplayer);
                remainingRounds--;
                if (currentplayer == _player1){
                    currentplayer = _player2;
                }
                else{
                    currentplayer = _player1;
                }
                if(remainingRounds == 0)
                {
                    Console.WriteLine("Do you want to play more rounds ? (n or y)");
                    if(Console.ReadLine() == "y")
                    {
                        Console.WriteLine("How many rounds ?");
                        remainingRounds = Convert.ToInt32(Console.ReadLine());
                    }
                }
            }
            Console.WriteLine("The game has come to an end. Let's found out who won !");
            Console.WriteLine("Here is a short recap : \n"+_player1.toString()+"\n"+_player2.toString());

            if (_player1.Score > _player2.Score)
            {
                Console.WriteLine("The winner is : "+_player1.Name);
            }
            else if (_player1.Score < _player2.Score)
            {
                Console.WriteLine("The winner is : "+_player2.Name);
            }
            else
            {
                Console.WriteLine("This game ends on a tie !");
            }
        }

        public void Turn(Player player){
            _board.PrintBoard();
            Console.WriteLine("Enter the words you found :");
            while(_clock.GetTimeRemaining() != 0)
            {
                string word = Console.ReadLine();
                if (_board.checkValidWord(word))
                {
                    player.Gain(word);
                    Console.WriteLine(string.Format("You have scored {0} with {1}.",player.LastGain, word));
                }
                else{
                    Console.WriteLine(word + "is not valid.");
                }
            }
        }

        public void CheckWin(){}

        public Board Board{
            get{return _board;}
        }

        public Clock Clock{
            get{return _clock;}
        }
    }
}