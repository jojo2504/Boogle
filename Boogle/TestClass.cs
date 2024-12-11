using Xunit;
using Xunit.Abstractions;
using Boogle.Engines;

public class TestClass{
    private readonly ITestOutputHelper Logger;
    private readonly Game game = new Game(["fr"]);
    private readonly Game game2 = new Game(["en"]);
    private readonly Game game3 = new Game(["fr", "en"]);

    public TestClass(ITestOutputHelper output)
    {
        Logger = output;
    }

    [Fact]
    public void Test_Language_french(){
        Assert.Equal(["fr"], game.Languages);
    }
    [Fact]
    public void Test_Language_english(){
        Assert.Equal(["en"], game2.Languages);
    }
    [Fact]
    public void Test_Language_french_english(){
        Assert.Equal(["fr", "en"], game3.Languages);
    }

    [Fact]
    public void Test_Dictionary_french(){
        Assert.Equal(130557, game.Board.Dictionary.WordList.Count);
    }

    [Fact]
    public void Test_Dictionary_english(){
        Assert.Equal(192591, game2.Board.Dictionary.WordList.Count);
    }

    [Fact]
    public void Test_Dictionary_french_english(){
        Assert.Equal(323148, game3.Board.Dictionary.WordList.Count);
    }

    [Fact]
    public void Test_AI(){
        char[,] testBoard = new char[3,3] {
            {'J', 'I', 'R'},
            {'J', 'I', 'R'},
            {'Q', 'D', 'A'},
        };
        SortedList<string, string> expected = new SortedList<string, string>
        {
            { "AI", "AI" },
            { "AIR", "AIR" },
            { "DA", "DA" },
            { "DIA", "DIA" },
            { "DIRA", "DIRA" },
            { "IRA", "IRA" },
            { "IRAI", "IRAI" },
            { "RA", "RA" },
            { "RAD", "RAD" },
            { "RAI", "RAI" },
            { "RAID", "RAID" },
            { "RI", "RI" },
            { "RIA", "RIA" },
            { "RIDA", "RIDA" },
            { "RIRA", "RIRA" },
            { "RIRAI", "RIRAI" }
        };
        Assert.Equal(expected, game.Ai.Play(testBoard));
    }

    [Fact]
    public void Test_CalculatePoints_banane(){
        Assert.Equal(8, game.CalculatePoints("BANANE"));
    }

    [Fact]
    public void Test_CalculatePoints_null(){
        Assert.Equal(0, game.CalculatePoints(""));
    }

    [Fact]
    public void Test_CalculatePoints_xzy(){
        Assert.Equal(30, game.CalculatePoints("XYZ"));
    }

    [Fact]
    public void Test_CalculatePoints_NeedToUpper(){
        int result = game.CalculatePoints("xyz");
        if (result != 30){
            Logger.WriteLine("Need to Upper each caracter in inputted word");
        }
        Assert.Equal(30, result);
    }

    [Fact]
    public void Test_ProcessWinner(){
        Player player1 = new Player("");
        Player player2 = new Player("");
        player1.Score = 5;
        player2.Score = 6;
        Assert.Equal($"The winner is: {player2.Name}", game.ProcessWinner(player1, player2));
    }

    [Fact]
    public void Test_ProcessWinner_Tie(){
        Player player1 = new Player("");
        Player player2 = new Player("");
        player1.Score = 5;
        player2.Score = 5;
        Assert.Equal("It's a tie!", game.ProcessWinner(player1, player2));
    }
}