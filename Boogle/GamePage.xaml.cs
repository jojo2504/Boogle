using System;
using System.Drawing;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using Boogle.Engines;
using directoryPath;

namespace Boogle
{
    public partial class GamePage : Page
    {
        private int _boardSize;
        private Game _game;
        private TextBlock[,] _boardTextBlocks;
        bool _player2IsAI;
        private DispatcherTimer _uiTimer;

        public GamePage(Game game, int roundRemaining, int seconds, string player1Name = "Player 1", string player2Name = "Player 2", bool player2IsAI = false)
        {
            InitializeComponent();

            _game = game;
            _game.RoundRemaining = roundRemaining;

            _player2IsAI = player2IsAI;

            Player player1 = new Player(player1Name);
            _game.Player1 = player1;
            _game.Player1.Clock = new Engines.Clock(seconds);
            Player1NameTextBlock.Text = player1.Name;
            Player1TimerTextBlock.Text = $"Time: {seconds}";

            if (player2IsAI) {
                AI player2 = new AI(_game.Board);
                _game.Ai = player2;
                Player2NameTextBlock.Text = "AI";
                Player2TimerTextBlock.Text = $"AI Timer";
            }
            else { 
                Player player2 = new Player(player2Name);
                _game.Player2 = player2;
                Player2NameTextBlock.Text = player2.Name;
                _game.Player2.Clock = new Engines.Clock(seconds);
                Player2TimerTextBlock.Text = $"Time: {seconds}";
            }
            CurrentPlayerTurnTextBlock.Text = $"{_game.Player1.Name}'s turn";
            RoundsRemainingTextBlock.Text = $"Rounds Remaining: {_game.RoundRemaining}";
            _game.CurrentPlayer = _game.Player1;
            _boardSize = _game.Board.BoardWidth;

            StartUITimer();
            InitializeGameBoard();

            SubmitWordButton.Click += SubmitWordButton_Click;
            EndTurnButton.Click += EndTurnButton_Click;

            _game.Player1.Clock.Start();
        }

        /// <summary>
        /// Submit inputted word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitWordButton_Click(object sender, RoutedEventArgs e)
        {
            string word = WordInputTextBox.Text.Trim().ToUpper();
            if (_game.Board.checkValidWord(word))
            {
                if (!_game.CurrentPlayer.WordsPlayedTurn.Contains(word))
                {
                    _game.CurrentPlayer.Add_Word(word);
                    int lastGain = Game.CalculatePoints(word);
                    _game.CurrentPlayer.Score += lastGain;
                    if (_game.CurrentPlayer == _game.Player1)
                    {
                        Player1PointsTextBlock.Text = $"Points: {_game.CurrentPlayer.Score}";
                    }
                    else
                    {
                        Player2PointsTextBlock.Text = $"Points: {_game.CurrentPlayer.Score}";
                    }
                }
            }
            else
            {
                ShakeTextBox(WordInputTextBox);
            }
            WordInputTextBox.Clear();
        }
        
        /// <summary>
        /// End turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndTurnButton_Click(object sender, RoutedEventArgs e)
        {
            WordInputTextBox.Clear();
            if (_game.CurrentPlayer == _game.Player2 || _player2IsAI)
            {
                _game.RoundRemaining--;
                RoundsRemainingTextBlock.Text = $"Rounds Remaining: {_game.RoundRemaining}";
            }

            if (_game.RoundRemaining == 0)
            {
                Bitmap playerCloud;
                string filePath;
                if (_player2IsAI || _game.Player1.Score > _game.Player2.Score)
                {
                    playerCloud = Game.CreationWordCloud(_game.Player1);
                    filePath = System.IO.Path.Combine(DirectoryPath.GetSolutionRoot(), "assets", "word_cloud_player1.png");
                }
                else
                {
                    playerCloud = Game.CreationWordCloud(_game.Player2);
                    filePath = System.IO.Path.Combine(DirectoryPath.GetSolutionRoot(), "assets", "word_cloud_player2.png");
                }
                WordCloudGenerator.SaveWordCloud(playerCloud, filePath);
                NavigationService?.Navigate(new WordCloudPage(filePath));
                return;
            }
            UpdateGameBoard();
            
            if (_player2IsAI)
            {
                Thread.Sleep(1000);
                int sumPoints = 0;
                IList<string> listOfWords = _game.Ai.Play().Keys;
                foreach (string word in listOfWords) {
                    sumPoints += Game.CalculatePoints(word);
                }
                _game.Ai.Score += sumPoints;
                MessageBox.Show(string.Join(" ", listOfWords) + sumPoints);
                Player2PointsTextBlock.Text = $"Points: {_game.Ai.Score}";    
            }
            else
            {
                ChangeTurn();
            }
        }

        /// <summary>
        /// Change turn and update everything
        /// </summary>
        private void ChangeTurn()
        {
            _game.CurrentPlayer.Clock.Stop();
            _game.CurrentPlayer.Clock.Reset();
            _game.CurrentPlayer.WordsReset();
            if (_game.CurrentPlayer == _game.Player1)
            {
                _game.CurrentPlayer = _game.Player2;
            }
            else if (_game.CurrentPlayer == _game.Player2)
            {
                _game.CurrentPlayer = _game.Player1;
            }
            CurrentPlayerTurnTextBlock.Text = $"{_game.CurrentPlayer.Name}'s turn";
            _game.CurrentPlayer.Clock.Start();
        }
        private void WordInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SubmitWordButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Shaking animation if failed input
        /// </summary>
        /// <param name="textBox"></param>
        private void ShakeTextBox(TextBox textBox)
        {
            const int shakeDelta = 5; 
            const int shakeDuration = 50; 
            int shakes = 5; 

            Storyboard storyboard = new Storyboard();

            for (int i = 0; i < shakes; i++)
            {
                DoubleAnimation shakeAnimation = new DoubleAnimation
                {
                    From = (i % 2 == 0) ? 0 : -shakeDelta,
                    To = (i % 2 == 0) ? shakeDelta : 0,
                    Duration = TimeSpan.FromMilliseconds(shakeDuration / 2),
                    AutoReverse = false
                };
                Storyboard.SetTarget(shakeAnimation, textBox);
                Storyboard.SetTargetProperty(shakeAnimation, new PropertyPath("(RenderTransform).(TranslateTransform.X)"));

                storyboard.Children.Add(shakeAnimation);
            }
            if (textBox.RenderTransform == null || !(textBox.RenderTransform is TranslateTransform))
            {
                textBox.RenderTransform = new TranslateTransform();
            }
            storyboard.Begin();
        }

        /// <summary>
        /// Initialize game board
        /// </summary>
        private void InitializeGameBoard()
        {
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();
            double cellSize = 80;

            for (int i = 0; i < _boardSize; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize) });
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize) });
            }
            _boardTextBlocks = new TextBlock[_boardSize, _boardSize];

            GameGrid.Children.Clear();

            for (int row = 0; row < _boardSize; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Style = (Style)FindResource("GameBoardLetterStyle"),
                        Text = _game.Board[row, col].CurrentLetter.ToString()
                    };

                    Grid.SetRow(textBlock, row);
                    Grid.SetColumn(textBlock, col);

                    GameGrid.Children.Add(textBlock);
                    _boardTextBlocks[row, col] = textBlock;
                }
            }
        }

        /// <summary>
        /// Update board letters
        /// </summary>
        public void UpdateGameBoard()
        {
            _game.Board.RollAllDices();
            for (int row = 0; row < _boardSize; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    _boardTextBlocks[row, col].Text = _game.Board[row, col].CurrentLetter.ToString();
                }
            }
        }

        /// <summary>
        /// Timer implementation
        /// </summary>
        private void StartUITimer()
        {
            _uiTimer = new DispatcherTimer();
            _uiTimer.Interval = TimeSpan.FromSeconds(1);
            _uiTimer.Tick += (sender, args) =>
            {
                Player1TimerTextBlock.Text = $"Time: {_game.Player1.Clock.GetFormattedTime()}";
                if (!_player2IsAI)
                {
                    Player2TimerTextBlock.Text = $"Time: {_game.Player2.Clock.GetFormattedTime()}";
                }
                if (_game.CurrentPlayer.Clock.GetTimeRemaining() == 0)
                {
                    SimulateEndTurn();
                }
            };
            _uiTimer.Start();
        }

        /// <summary>
        /// Simulate end turn
        /// </summary>
        private void SimulateEndTurn()
        {
            EndTurnButton_Click(EndTurnButton, new RoutedEventArgs());
        }

        /// <summary>
        /// Methods to update each player attributes
        /// </summary>
        /// <param name="points"></param> <summary>
        public void UpdatePlayer1Points(int points)
        {
            Player1PointsTextBlock.Text = $"Points: {points}";
        }

        public void UpdatePlayer2Points(int points)
        {
            Player2PointsTextBlock.Text = $"Points: {points}";
        }

        public void UpdatePlayer1Timer(string time)
        {
            Player1TimerTextBlock.Text = $"Time: {time}";
        }

        public void UpdatePlayer2Timer(string time)
        {
            Player2TimerTextBlock.Text = $"Time: {time}";
        }

        public void UpdateCurrentPlayerTurn(string playerName)
        {
            CurrentPlayerTurnTextBlock.Text = $"{playerName}'s Turn";
        }

        public void UpdateRoundsRemaining(int rounds)
        {
            RoundsRemainingTextBlock.Text = $"Rounds Remaining: {rounds}";
        }
    }
}