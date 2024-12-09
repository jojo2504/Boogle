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
    public void Test_AddPoint_banane(){
        Assert.Equal(8, game.AddPoint("BANANE"));
    }

    [Fact]
    public void Test_AddPoint_null(){
        Assert.Equal(0, game.AddPoint(""));
    }

    [Fact]
    public void Test_AddPoint_xzy(){
        Assert.Equal(30, game.AddPoint("XYZ"));
    }

    [Fact]
    public void Test_AddPoint_NeedToUpper(){
        int result = game.AddPoint("xyz");
        if (result != 30){
            Logger.WriteLine("Need to Upper each caracter in inputted word");
        }
        Assert.Equal(30, result);
    }

    [Fact]
    public void Test_CheckWinner(){
        Player player1 = new Player("");
        Player player2 = new Player("");
        player1.Score = 5;
        player2.Score = 6;
        Assert.Equal(player2, game.CheckWinner(player1, player2));
    }
}