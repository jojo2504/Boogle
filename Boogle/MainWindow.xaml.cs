using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Boogle.Engines;

namespace MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AICheckBox_Click(object sender, RoutedEventArgs e)
        {
            // Disable or enable Player 2's Name TextBox based on the checkbox state
            Player2NameTextBox.IsEnabled = !(AICheckBox.IsChecked ?? false);
        }
        private void EnglishCheckBox_Click(object sender, RoutedEventArgs e)
        {
            // Disable or enable Player 2's Name TextBox based on the checkbox state
            EnglishCheckBox.IsEnabled = !(EnglishCheckBox.IsChecked ?? false);
        }
        private void FrenchCheckBox_Click(object sender, RoutedEventArgs e)
        {
            // Disable or enable Player 2's Name TextBox based on the checkbox state
            FrenchCheckBox.IsEnabled = !(FrenchCheckBox.IsChecked ?? false);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string player1Name = Player1NameTextBox.Text;
            string player2Name = Player2NameTextBox.IsEnabled ? Player2NameTextBox.Text : "AI";
            bool isAIEnabled = AICheckBox.IsChecked ?? false;
            bool isEnglishEnabled = EnglishCheckBox.IsChecked ?? false;
            bool isFrenchEnabled = FrenchCheckBox.IsChecked ?? false;

            // Get values from the sliders
            int boardSize = (int)BoardSizeSlider.Value;
            int numberOfTurns = (int)NumberOfTurnsSlider.Value;
            int timeMinutes = int.TryParse(MinutesTextBox.Text, out int minutes) ? minutes : 0;
            int timeSeconds = int.TryParse(SecondsTextBox.Text, out int seconds) ? seconds : 0;

            int finalTime = timeMinutes*60 + timeSeconds;

            // Initialize the game
            List<string> languages = new();
            if (isEnglishEnabled){
                languages.Add("en");
            }
            if (isFrenchEnabled){
                languages.Add("fr");
            }
            var game = new Game(boardSize, boardSize, languages, finalTime);
            game.Player1 = new Player(player1Name);
            game.Player2 = isAIEnabled ? null : new Player(player2Name);
            game.StartClock();

            MessageBox.Show($"{game.Languages}");
            MessageBox.Show($"Game initialized with:\nPlayer 1: {player1Name}\nPlayer 2: {player2Name}\nBoard Size: {boardSize}\nNumber of Turns: {numberOfTurns}\nTime per Turn: {finalTime}s");
            NavigationService.Navigate(new Page2());
        }
    }   
}
