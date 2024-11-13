using System.Windows.Forms;
using System.ComponentModel;
using Boogle.Engines;

namespace Boogle;

public partial class Form1 : Form
{
    Game _game;
    int _boardWidth;
    int _boardHeight;
    TextBox playerLeftTextBox;
    TextBox playerRightTextBox;

    CheckBox checkBoxFrench;
    CheckBox checkBoxEnglish;

    Panel mainPanel;
    Panel arrowOverlayPanel;

    public Form1()
    {
        InitializeComponent();
        Start();
    }

    private void Start(){
        MenuLayout();
    }

    private void InitializeGame(int boardWidth, int boardHeight, List<string> languages){
        _game = new Game(boardWidth, boardHeight, languages);
    }
    private void InitializeGame(List<string> languages){
        _game = new Game(languages);
    }

    private void MenuLayout(){
        // Create main container panel that will hold all other panels
        mainPanel = new Panel{
            Dock = DockStyle.Fill
        };

        // Create the base panels
        Panel textBoxPanel = CreateTextBoxPanel();
        
        // Create the overlay arrow panel - positioned in middle of screen
        Panel arrowOverlayPanel = CreateArrowPanel();

        Panel checkBoxPanel = CreateCheckBoxPanel();
        
        // Position the arrow panel in the middle of the form
        arrowOverlayPanel.Dock = DockStyle.None;
        arrowOverlayPanel.Height = 60; // Fixed height for the arrow panel
        arrowOverlayPanel.Width = ClientSize.Width;
        
        // Center vertically
        int middleY = (ClientSize.Height - arrowOverlayPanel.Height) / 2;
        arrowOverlayPanel.Location = new Point(0, middleY);
        
        // Set anchors to maintain position when form resizes
        arrowOverlayPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right;

        // Add panels to the form in order (bottom to top)
        Controls.Add(mainPanel);
        Controls.Add(arrowOverlayPanel);
        Controls.Add(checkBoxPanel);
        
        // Ensure arrow panel stays on top
        arrowOverlayPanel.BringToFront();
        //checkBoxPanel.BringToFront();

        // Add resize handler to keep panel centered
        this.Resize += (sender, e) => {
            arrowOverlayPanel.Location = new Point(0, (this.ClientSize.Height - arrowOverlayPanel.Height) / 2);
        };
    }

    private Panel CreateArrowPanel(){
        Panel arrowPanel = new Panel(){
            Dock = DockStyle.Top,
        };

        TableLayoutPanel arrowLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 1,
            ColumnCount = 3,
            Padding = new Padding(0, 0, 0, 0) // Add left padding to shift button from edge
        };

        // Adjust column proportions to position button more to the left
        arrowLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f)); // Fixed width for button column
        arrowLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));   // Rest of space split between other columns
        arrowLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
        
        Button arrowButtonLeft = new Button{
            Size = new Size(100, 30),
            Anchor = AnchorStyles.None // This ensures the button is centered in its cell
        };
        Image arrowImageLeft = Image.FromFile("assets/Arrow.png");
        arrowImageLeft.RotateFlip(RotateFlipType.Rotate90FlipNone);
        arrowButtonLeft.Image = arrowImageLeft;

        Button arrowButtonRight = new Button{
            Size = new Size(100, 30),
            Anchor = AnchorStyles.None
        };
        Image arrowImageRight = Image.FromFile("assets/Arrow.png");
        arrowImageRight.RotateFlip(RotateFlipType.Rotate270FlipNone);
        arrowButtonRight.Image = arrowImageRight;
        
        arrowLayout.Controls.Add(arrowButtonLeft, 0, 0);
        arrowLayout.Controls.Add(arrowButtonRight, 2, 0);

        arrowPanel.Controls.Add(arrowLayout);
        return arrowPanel;
    }

    private Panel CreateTextBoxPanel(){
        Panel textBoxPanel = new Panel
        {
            Dock = DockStyle.Fill
        };

        // Create a TableLayoutPanel for organizing the content
        TableLayoutPanel contentLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 3,
        };

        // Set column sizes (33% each)
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

        // Set row sizes
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

        Label titleLabel = new Label
        {
            Text = "Enter your names :",
            Font = new Font("Arial", 25, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline),
            AutoSize = true,
            Anchor = AnchorStyles.None,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.Red
        };

        playerLeftTextBox = new TextBox
        {
            Font = new Font("Arial", 12),
            Anchor = AnchorStyles.None,
            Size = new Size(150, 30),
            PlaceholderText = "Left player"
        };

        playerRightTextBox = new TextBox
        {
            Font = new Font("Arial", 12),
            Anchor = AnchorStyles.None,
            Size = new Size(150, 30),
            PlaceholderText = "Right player"
        };

        Button submitButton = new Button
        {
            Text = "Start Game",
            Anchor = AnchorStyles.None,
            Size = new Size(100, 30)
        };
        submitButton.Click += SubmitButton_Click;

        contentLayout.Controls.Add(titleLabel, 1, 0);
        contentLayout.Controls.Add(playerLeftTextBox, 0, 2);
        contentLayout.Controls.Add(playerRightTextBox, 2, 2);
        contentLayout.Controls.Add(submitButton, 1, 2);

        textBoxPanel.Controls.Add(contentLayout);
        mainPanel.Controls.Add(textBoxPanel);

        return textBoxPanel;
    }

    private Panel CreateCheckBoxPanel(){
        Panel checkBoxPanel = new Panel
        {
            Dock = DockStyle.Top
        };

        // Create a TableLayoutPanel for organizing the content
        TableLayoutPanel contentLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 3,
        };
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90F));
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        // Set row sizes
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));

        checkBoxFrench = new CheckBox{
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            Text = "English",
            AutoSize = true
        }; 
        //checkBoxFrench.CheckedChanged += SubmitCheckBox;
        contentLayout.Controls.Add(checkBoxFrench, 2, 2);

        checkBoxEnglish = new CheckBox{
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            Text = "French",
            AutoSize = true
        }; 
        //checkBoxEnglish.CheckedChanged += SubmitCheckBox;
        contentLayout.Controls.Add(checkBoxEnglish, 2, 1);

        checkBoxPanel.Controls.Add(contentLayout);
        return checkBoxPanel;
    }

    private void SubmitButton_Click(object? sender, EventArgs e)
    {
        string player1 = playerLeftTextBox.Text;
        string player2 = playerRightTextBox.Text;
        
        if (string.IsNullOrWhiteSpace(player1) || string.IsNullOrWhiteSpace(player2))
        {
            MessageBox.Show("Please enter names for both players!", "Warning");
            return;
        }
        
        List<string> languages = [];
        if (checkBoxFrench.Checked) languages.Add("fr");
        if (checkBoxEnglish.Checked) languages.Add("en");
        //Console.WriteLine(string.Join(", ", languages));
        InitializeGame(languages);
        _game.Dictionary.toString();

        Console.WriteLine($"Player 1: {player1}");
        Console.WriteLine($"Player 2: {player2}");

        // Clear and move to next phase
        Controls.Clear();

        CreateSecondPhase(player1, player2);
    }

    private void CreateSecondPhase(string player1, string player2)
    {
        Label welcomeLabel = new Label
        {
            Text = $"Welcome {player1} and {player2}!",
            Font = new Font("Arial", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(50, 50)
        };
        Controls.Add(welcomeLabel);
    }

    private void Button_Click(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            Console.WriteLine(button.Text);
        }
    }

    private void ResetLayout(){
        Controls.Clear();
    }

    private void GameLayout(){
    }

    private void CloudLayout(){
    }
}