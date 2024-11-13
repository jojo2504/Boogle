using KaimiraGames;

namespace Boogle.Engines{
    class Dice{
        readonly WeightedList<char> _weightedList = InitializeWeight();

        readonly Random _random = new Random();
        char[] _faces;
        int _currentFace;

        public Dice(int faces = 6){
            _faces = new char[faces]; // ${faces} faces
            GenerateFaces();
        }

        private static WeightedList<char> InitializeWeight(){
            WeightedList<char> weightedList = new();
            
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
                            int.TryParse(parts[2], out int weight))
                        {
                            weightedList.Add(letter, weight);
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }

            return weightedList;
        }

        public void GenerateFaces(){
            for(int i=0; i<_faces.Length; i++){
                _faces[i] = _weightedList.Next(); //returns a random letter based on the weightedList
            }
        }

        public void RollDice(){
            _currentFace = _random.Next(0, _faces.Length-1);
        }

        public int CurrentFace => _currentFace;
        public char CurrentLetter => _faces[_currentFace];
        public char[] Faces => _faces;
    }
}