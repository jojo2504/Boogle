using Boogle.Engines;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Boogle;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        //Application.Run(new Form1());
        Game game = new Game(3, 3, ["en","fr"]);
        char[,] testBoard = new char[3,3] {
            {'J', 'I', 'R'},
            {'J', 'I', 'R'},
            {'Q', 'D', 'A'},
        };
        Console.WriteLine("{0}", game.Board.Dictionary.WordList.Count);
        SortedList<string, string> actual = game.Ai.Play(testBoard);
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

        Console.WriteLine(string.Join("\n", actual.ToArray()));
    }       

    /// <summary>
    /// Debugging purpose, do NOT touch if you don't know what it is
    /// dotnet run --property:OutputType=Exe
    /// </summary>
    #region
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AllocConsole();
    #endregion
}