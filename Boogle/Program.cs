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
        Stopwatch stopwatch = new Stopwatch();
        char[,] board = new char[,]
        {
            { 'E', 'N', 'I', 'A' },
            { 'O', 'L', 'T', 'S' },
            { 'D', 'R', 'E', 'D' },
            { 'N', 'E', 'E', 'O' }
        };
        Game game = new Game(50, 50, ["fr","en"]);
        stopwatch.Start();
        game.Board.getAllValidWordsOnBoard(game.Board.Dictionary.Root);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);

        //Console.WriteLine(game.Dictionary.RechDichoRecursif("DE"));
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