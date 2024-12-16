using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using Boogle.Engines;

public class TestClass
{
    private readonly ITestOutputHelper Logger;
    private static readonly Game game = new Game(["fr"]);
    private static readonly Game game2 = new Game(["en"]);
    private static readonly Game game3 = new Game(["fr", "en"]);

    public TestClass(ITestOutputHelper output)
    {
        Logger = output;
    }

    [Fact]
    public void Test_Language_french()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(["fr"], game.Languages);
        timer.Stop();
        Logger.WriteLine($"Test_Language_french executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_Language_english()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(["en"], game2.Languages);
        timer.Stop();
        Logger.WriteLine($"Test_Language_english executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_Language_french_english()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(["fr", "en"], game3.Languages);
        timer.Stop();
        Logger.WriteLine($"Test_Language_french_english executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_Dictionary_french()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(130557, game.Board.Dictionary.WordList.Count);
        timer.Stop();
        Logger.WriteLine($"Test_Dictionary_french executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_Dictionary_english()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(192591, game2.Board.Dictionary.WordList.Count);
        timer.Stop();
        Logger.WriteLine($"Test_Dictionary_english executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_Dictionary_french_english()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(323148, game3.Board.Dictionary.WordList.Count);
        timer.Stop();
        Logger.WriteLine($"Test_Dictionary_french_english executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_AI()
    {
        var timer = Stopwatch.StartNew();
        char[,] testBoard = new char[3, 3] {
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
        game.Ai = new AI(game.Board);
        Assert.Equal(expected, game.Ai.Play(testBoard));
        timer.Stop();
        Logger.WriteLine($"Test_AI executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_AI2()
    {
        var timer = Stopwatch.StartNew();
        char[,] testBoard = new char[3, 3] {
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
        int sum = 0;
        foreach (string word in expected.Keys)
        {
            sum += Game.CalculatePoints(word);
        }
        Assert.Equal(57, sum);
        timer.Stop();
        Logger.WriteLine($"Test_AI2 executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_DFS()
    {
        var timer = Stopwatch.StartNew();
        char[,] testBoard = new char[3, 3] {
            {'T', 'E', 'Z'},
            {'T', 'E', 'U'},
            {'I', 'D', 'A'},
        };
        Logger.WriteLine(string.Join(' ', game.Board.getAllValidWordsOnBoard(game.Board.Dictionary.Root, testBoard).Keys));
        timer.Stop();
        Logger.WriteLine($"Test_DFS executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_CalculatePoints_banane()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(8, Game.CalculatePoints("BANANE"));
        timer.Stop();
        Logger.WriteLine($"Test_CalculatePoints_banane executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_CalculatePoints_null()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(0, Game.CalculatePoints(""));
        timer.Stop();
        Logger.WriteLine($"Test_CalculatePoints_null executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_CalculatePoints_xzy()
    {
        var timer = Stopwatch.StartNew();
        Assert.Equal(30, Game.CalculatePoints("XYZ"));
        timer.Stop();
        Logger.WriteLine($"Test_CalculatePoints_xzy executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_ProcessWinner()
    {
        var timer = Stopwatch.StartNew();
        Player player1 = new Player("");
        Player player2 = new Player("");
        player1.Score = 5;
        player2.Score = 6;
        Assert.Equal($"The winner is: {player2.Name}", game.ProcessWinner(player1, player2));
        timer.Stop();
        Logger.WriteLine($"Test_ProcessWinner executed in {timer.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Test_ProcessWinner_Tie()
    {
        var timer = Stopwatch.StartNew();
        Player player1 = new Player("");
        Player player2 = new Player("");
        player1.Score = 5;
        player2.Score = 5;
        Assert.Equal("It's a tie!", game.ProcessWinner(player1, player2));
        timer.Stop();
        Logger.WriteLine($"Test_ProcessWinner_Tie executed in {timer.ElapsedMilliseconds} ms");
    }
    
    [Fact]
    public void TestMergeSort()
    {
        var timer = Stopwatch.StartNew();
        int[] input = { 38, 27, 43, 3, 9, 82, 10 };
        int[] expected = { 3, 9, 10, 27, 38, 43, 82 };
        int[] result = Dictionary.MergeSortArray(input);
        Assert.Equal(expected, result);
        timer.Stop();
        Logger.WriteLine($"TestMergeSort executed in {timer.ElapsedMilliseconds} ms");
    }
}