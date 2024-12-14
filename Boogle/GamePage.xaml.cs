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

            // Initialize timers and start updating the UI
            StartUITimer();

            InitializeGameBoard();
 
            // Wire up event handlers
            SubmitWordButton.Click += SubmitWordButton_Click;
            EndTurnButton.Click += EndTurnButton_Click;

            //start turn (player 1)
            _game.Player1.Clock.Start();
        }

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
            //end logic
            WordInputTextBox.Clear();
        }

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
                if (_player2IsAI || _game.Player1.Score > _game.Player2.Score)
                {
                    playerCloud = WordCloudGenerator.CreateWordCloud(_game.Player1.WordsFound);
                }
                else
                {
                    playerCloud = WordCloudGenerator.CreateWordCloud(_game.Player2.WordsFound);
                }
                string filePath = System.IO.Path.Combine(DirectoryPath.GetSolutionRoot(), "assets", "word_cloud_player1.png");
                WordCloudGenerator.SaveWordCloud(playerCloud, filePath);
                NavigationService?.Navigate(new WordCloudPage());
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
                //MessageBox.Show(string.Join(" ", listOfWords) + sumPoints);
                Player2PointsTextBlock.Text = $"Points: {_game.Ai.Score}";    
            }
            else
            {
                ChangeTurn();
            }
        }

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
            CurrentPlayerTurnTextBlock.Text = _game.CurrentPlayer.Name;
            _game.CurrentPlayer.Clock.Start();
        }
        private void WordInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SubmitWordButton_Click(sender, e);
            }
        }

        private void ShakeTextBox(TextBox textBox)
        {
            const int shakeDelta = 5; // Amount to move left and right
            const int shakeDuration = 50; // Duration of each shake in milliseconds
            int shakes = 5; // Number of shakes (must be an odd number)

            Storyboard storyboard = new Storyboard();

            // Create the animation for horizontal shaking
            for (int i = 0; i < shakes; i++)
            {
                DoubleAnimation shakeAnimation = new DoubleAnimation
                {
                    From = (i % 2 == 0) ? 0 : -shakeDelta, // Alternating between left and right
                    To = (i % 2 == 0) ? shakeDelta : 0,
                    Duration = TimeSpan.FromMilliseconds(shakeDuration / 2),
                    AutoReverse = false
                };

                // Add the animation to the Storyboard targeting the TranslateTransform
                Storyboard.SetTarget(shakeAnimation, textBox);
                Storyboard.SetTargetProperty(shakeAnimation, new PropertyPath("(RenderTransform).(TranslateTransform.X)"));

                storyboard.Children.Add(shakeAnimation);
            }

            // Apply a TranslateTransform if not already present
            if (textBox.RenderTransform == null || !(textBox.RenderTransform is TranslateTransform))
            {
                textBox.RenderTransform = new TranslateTransform();
            }

            // Start the shake animation
            storyboard.Begin();
        }

        private void InitializeGameBoard()
        {
            // Clear any previous row and column definitions in the grid
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();

            // Define the size of each cell in the grid
            double cellSize = 80;

            // Add row and column definitions dynamically based on _boardSize
            for (int i = 0; i < _boardSize; i++)
            {
                // Add Row and Column definitions with fixed pixel sizes
                GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize) });
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize) });
            }

            // Initialize the _boardTextBlocks array to hold references to the TextBlock elements
            _boardTextBlocks = new TextBlock[_boardSize, _boardSize];

            // Clear existing children from the grid
            GameGrid.Children.Clear();

            // Add vertical grid lines
            for (int col = 0; col <= _boardSize; col++)
            {
                Line verticalLine = new Line
                {
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 63, 255, 255)),
                    StrokeThickness = 1,
                    X1 = col * cellSize,
                    Y1 = 0,
                    X2 = col * cellSize,
                    Y2 = _boardSize * cellSize
                };
                GameGrid.Children.Add(verticalLine);
            }

            // Add horizontal grid lines
            for (int row = 0; row <= _boardSize; row++)
            {
                Line horizontalLine = new Line
                {
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 63, 255, 255)),
                    StrokeThickness = 1,
                    X1 = 0,
                    Y1 = row * cellSize,
                    X2 = _boardSize * cellSize,
                    Y2 = row * cellSize
                };
                GameGrid.Children.Add(horizontalLine);
            }

            // Add TextBlock elements to the grid for each position
            for (int row = 0; row < _boardSize; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    // Create a new TextBlock for each cell
                    TextBlock textBlock = new TextBlock
                    {
                        Style = (Style)FindResource("GameBoardLetterStyle"),
                        Text = _game.Board[row, col].CurrentLetter.ToString()
                    };

                    // Set the row and column for the TextBlock
                    Grid.SetRow(textBlock, row);
                    Grid.SetColumn(textBlock, col);

                    // Add the TextBlock to the GameGrid
                    GameGrid.Children.Add(textBlock);

                    // Store the reference in _boardTextBlocks for future updates
                    _boardTextBlocks[row, col] = textBlock;
                }
            }
        }

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

        private void StartUITimer()
        {
            _uiTimer = new DispatcherTimer();
            _uiTimer.Interval = TimeSpan.FromSeconds(1);
            _uiTimer.Tick += (sender, args) =>
            {
                // Update the timer for each player
                Player1TimerTextBlock.Text = $"Time: {_game.Player1.Clock.GetFormattedTime()}";
                if (!_player2IsAI)
                {
                    Player2TimerTextBlock.Text = $"Time: {_game.Player2.Clock.GetFormattedTime()}";
                }

                // Check if Player 1's timer has reached 0
                if (_game.CurrentPlayer.Clock.GetTimeRemaining() == 0)
                {
                    SimulateEndTurn();
                }
            };
            _uiTimer.Start();
        }

        private void SimulateEndTurn()
        {
            // Simulate pressing the End Turn button
            EndTurnButton_Click(EndTurnButton, new RoutedEventArgs());
        }

        // Methods to update points for each player
        public void UpdatePlayer1Points(int points)
        {
            Player1PointsTextBlock.Text = $"Points: {points}";
        }

        public void UpdatePlayer2Points(int points)
        {
            Player2PointsTextBlock.Text = $"Points: {points}";
        }

        // Methods to update timers (placeholders for you to implement)
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