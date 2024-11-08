namespace Boogle.Engines{
    class Dice{
        Random _random = new Random();
        char[] _faces;
        int _currentFace;

        public Dice(int faces = 6){
            _faces = new char[faces]; // ${faces} faces
            GenerateFaces();
        }

        public void GenerateFaces(){
            int num;
            char randomLetter;
            for(int i=0; i<_faces.Length; i++){
                num = _random.Next(0, 26);
                randomLetter = (char)('A' + num);
                _faces[i] = randomLetter;
            }
        }

        public void RollDice(){
            _currentFace = _random.Next(0, _faces.Length-1);
        }

        public int CurrentFace{
            get{return _currentFace;}
        }

        public char CurrentLetter{
            get{return _faces[_currentFace];}
        }
    }
}