using Boogle.Engines;

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
        
        Board board = new Board(4, 4);
        Game game = new Game(board);

        game.Board.BoardGenerator();
        game.Board.PrintBoard();
        game.Dictionary.Print();

        game.Dictionary.toString();

        Console.WriteLine(game.Dictionary.RechDichoRecursif("TERRACED"));
        Console.WriteLine(game.Dictionary.RechDichoRecursif("TERRACEDKJ"));
    }    

    /// <summary>
    /// Debugging purpose, do NOT touch if you don't know what it is
    /// </summary>
    #region
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AllocConsole();
    #endregion
}