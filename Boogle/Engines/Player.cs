namespace Boogle.Engines{
    class Player{
        string _name;
        int _score = 0;
        int _lastGain = 0;
        Dictionary<char,int> _letterPoints = InitializeLetterPoints();
        Dictionary<string, int> _wordsFound = new Dictionary<string, int>();

        public Player(string _name)
        {
            this._name = _name;
        }
        private static Dictionary<char,int> InitializeLetterPoints()
        {
            Dictionary<char, int> letterPoints = new Dictionary<char, int>();

            string filePath = Path.Combine("..", "Boogle", "Utils", "letters");
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

        public void Gain(string word)
        {
            _lastGain = 0;
            foreach (char letter in word)
            {
                _score += _letterPoints[letter];
                _lastGain += _letterPoints[letter];  
            }
        }

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
        }

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
        }
        public int LastGain
        {
            get {return _lastGain;}
        }
        public Dictionary<string, int> WordsFound
        {
            get {return _wordsFound;}
        }
    }
}