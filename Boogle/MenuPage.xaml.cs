using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Boogle
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void AICheckBox_Click(object sender, RoutedEventArgs e)
        {
            // Disable or enable Player 2's Name TextBox based on the checkbox state
            Player2NameTextBox.IsEnabled = !(AICheckBox.IsChecked ?? false);
        }

        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinutesTextBox.Text, out int minutes) && int.TryParse(SecondsTextBox.Text, out int seconds) && (minutes*60+seconds)>=10)
            {
                //MessageBox.Show($"ok");
                PlayButton.IsEnabled = true;
            }
            else
            {
                //MessageBox.Show($"pas ok");
                PlayButton.IsEnabled = false;
            }
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

            int finalTime = timeMinutes * 60 + timeSeconds;

            // Initialize the game
            List<string> languages = new();
            if (isEnglishEnabled)
            {
                languages.Add("en");
            }
            if (isFrenchEnabled)
            {
                languages.Add("fr");
            }
            Game game = new Game(boardSize, boardSize, languages);
            //MessageBox.Show($"Game initialized with:\nPlayer 1: {player1Name}\nPlayer 2: {player2Name}\nBoard Size: {boardSize}\nNumber of Turns: {numberOfTurns}\nTime per Turn: {finalTime}s");
            NavigateToGamePage(game, numberOfTurns, finalTime, player1Name, player2Name, isAIEnabled);
        }
        private void NavigateToGamePage(Game game, int numberOfTurns, int finalTime, string player1Name, string player2Name, bool isAIEnabled)
        {
            NavigationService?.Navigate(new GamePage(game, numberOfTurns, finalTime, player1Name, player2Name, isAIEnabled));
        }
    }
}
