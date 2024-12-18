namespace Boogle.Engines{
    public class Player{
        string _name;
        int _score = 0;

        Clock _clock;
        
        Dictionary<string, int> _wordsFound = new Dictionary<string, int>();

        List<string> _wordsPlayedTurn = new();

        public Player(string _name)
        {
            this._name = _name;
        }
        /// <summary>
        /// Verify if the word is contain in the dictionary as a key
        /// </summary>
        /// <param name="mot"></param>
        /// <returns></returns>
        public bool Contain(string mot)
        {
            bool isContained = false;
            foreach (KeyValuePair<string,int> kvp in _wordsFound)
            {
                if (kvp.Key == mot)
                {
                    isContained = true;
                }
            }
            return isContained;
        }

        /// <summary>
        /// Add the word to the dictionary as a key or increase its value by 1
        /// </summary>
        /// <param name="mot"></param>
        public void Add_Word (string mot)
        {
            if (Contain(mot))
            {
                _wordsFound[mot]++;
            }
            else
            {
                _wordsFound.Add(mot,1);
            }

            if (!_wordsPlayedTurn.Contains(mot)){
                _wordsPlayedTurn.Add(mot);
            }
        }
        /// <summary>
        /// Clear the list wordsPlayerTurn
        /// </summary>
        public void WordsReset(){
            _wordsPlayedTurn.Clear();
        }

        /// <summary>
        /// Create a string to describe a Player
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string descriptionPlayer = string.Format("{0} has {1} points and has found those words :\n", _name, _score);
            foreach (KeyValuePair<string,int> kvp in _wordsFound)
            {
                descriptionPlayer += string.Format("{0}, {1} time",kvp.Key,kvp.Value);
            }
            return descriptionPlayer;
        }
        
        public string Name
        {
            get {return _name;}
        }
        public int Score
        {
            get {return _score;}
            set {_score = value;}
        }
        public Dictionary<string, int> WordsFound
        {
            get {return _wordsFound;}
        }

        public Clock Clock { get; internal set; }

        public List<string> WordsPlayedTurn => _wordsPlayedTurn;
    }
}