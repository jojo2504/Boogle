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
        Game game = Game.InitGame();
        game.Start();
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