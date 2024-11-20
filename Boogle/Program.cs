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
        Game game = new Game(10, 10, ["fr"]);
        game.Board.BoardGenerator();
        Console.WriteLine(game.Board.PrintBoard());
        List<string> valid_words = [];

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        List<string> words = game.Board.getAllValidWordsInBoard(game.Dictionary.Root);
        foreach (string word in words){
            if (game.Dictionary.RechDichoRecursif(word)){
                valid_words.Add(word);
            };
        }
        stopwatch.Stop();
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