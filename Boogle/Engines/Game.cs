using directoryPath;

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
        Dictionary<char,int> _letterPoints = InitializeLetterPoints();

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

        public static Game InitGame(){
            string userInput;
            List<string> languages = new();

            Console.WriteLine("Hello, welcome to the Boogle game :\n\nEnter the name of player 1 :");
            Player player1 = new Player(Console.ReadLine());
            Console.WriteLine("Enter the name of player 2 :");
            Player _player2 = new Player(Console.ReadLine());

            Console.WriteLine("Select the language you want to play with (en or fr):");
            do {
                userInput = Console.ReadLine()!;
                if (userInput != "fr" && userInput != "en") {
                    break;
                }
                languages.Add(userInput);
            } while (languages.Count < 2);

            Game game = new Game(languages);
            game._player1 = player1;
            game._player2 = player1;

            return game;
        }

        public void Start(){
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
            Console.WriteLine(ProcessWinner(_player1, _player2));
        }

        public void Turn(Player player){
            Console.WriteLine(_board);
            Console.WriteLine("Enter the words you found :");
            while(_clock.GetTimeRemaining() != 0)
            {
                string word = Console.ReadLine();
                if (_board.checkValidWord(word.ToUpper()))
                {
                    int lastGain = CalculatePoints(word);
                    player.Score += lastGain;
                    Console.WriteLine(string.Format("You have scored {0} with {1}.", lastGain, word));
                }
                else {
                    Console.WriteLine("{0} is not valid.", word);
                }
            }
        }

        public int CalculatePoints(string word){
            int currentWordGain = 0;
            foreach (char letter in word){
                currentWordGain += _letterPoints[char.ToUpper(letter)];
            }
            return currentWordGain;
        }

        public string ProcessWinner(Player player1, Player player2){
            if (player1.Score != player2.Score){
                string winner = player1.Score > player2.Score ? player1.Name : player2.Name;
                return $"The winner is: {winner}";
            }
            else{
                return "It's a tie!";
            }
        }

        private static Dictionary<char,int> InitializeLetterPoints()
        {
            Dictionary<char, int> letterPoints = new Dictionary<char, int>();

            string filePath = Path.Combine(DirectoryPath.GetSolutionRoot(), "Utils", "letters");
            try
            {
                // Create a StreamReader
                using (StreamReader streamReader = new StreamReader(Path.Combine(filePath))){
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length == 3 &&
                            char.TryParse(parts[0], out char letter) &&
                            int.TryParse(parts[1], out int points))
                        {
                            letterPoints[letter] = points;
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
            return letterPoints;
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