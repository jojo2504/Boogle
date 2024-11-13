using Boogle.Engines;
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
        Application.Run(new Form1());

        Board board = new Board(10, 10);
        Game game = new Game(board, ["en", "fr"]);
        game.Board.BoardGenerator();

        Console.WriteLine(string.Join(',',game.Board[0,0].Faces));

        game.Board.RollAllDices();
        game.Board.PrintBoard();
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