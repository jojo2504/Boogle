namespace Boogle.Engines{
    class Player{
        string _name;
        int _score = 0;
        Dictionary<string, int> _wordsFound = new Dictionary<string, int>();

        public Player(string _name)
        {
            this._name = _name;
        }

        public void Gain(int points)
        {
            _score += points;
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
            string descriptionPlayer = string.Format("{0} a {1} points et a trouvé les mots suivants :\n", _name, _score);
            foreach (KeyValuePair<string,int> kvp in _wordsFound)
            {
                descriptionPlayer += string.Format("{0}, {1} fois",kvp.Key,kvp.Value);
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
        public Dictionary<string, int> WordsFound
        {
            get {return _wordsFound;}
        }
    }
}