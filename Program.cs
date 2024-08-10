namespace Minesweeper;

internal class Program
{
    static void Main()
    {
        Help();

        while (true){
            ProcessInput();
        }
    }

    static int width = 5;
    static int height = 5;
    static int mines = 5;
    static readonly Board B = new();

    public static string ReadInput()
    {
        // This function is supposed to ensure there are no null values

        string input = "";

        while (input == "")
        {
            input = Console.ReadLine() + "";
        }

        return input;
    }

    public static void ProcessInput()
    {
        int a;
        int b;
        int c;

        // Interpreting commands

        switch (ReadInput())
        {
            case "reveal":
                if (B.gameOver)
                {
                    Console.WriteLine("\nError! New game must be started");
                    break;
                }

                Console.WriteLine("\nEnter x, then y");
                if(!Int32.TryParse(Console.ReadLine() + "", out a) || 
                   !Int32.TryParse(Console.ReadLine() + "", out b))
                {
                    Console.WriteLine("\nError! Input caused parsing error");
                    break;
                }

                if (a < 0 || a >= B.sizeX || b < 0 || b >= B.sizeY)
                {
                    Console.WriteLine("\nError! Input out of bounds");
                    break;
                }

                B.Reveal(a, b);
                break;

            case "flag":
                if (B.gameOver)
                {
                    Console.WriteLine("\nError! New game must be started");
                    break;
                }

                Console.WriteLine("\nEnter x, then y");

                if (!Int32.TryParse(Console.ReadLine() + "", out a) ||
                   !Int32.TryParse(Console.ReadLine() + "", out b))
                {
                    Console.WriteLine("\nError! Input caused parsing error");
                    break;
                }

                if (a < 0 || a >= B.sizeX || b < 0 || b >= B.sizeY)
                {
                    Console.WriteLine("\nError! Input out of bounds");
                    break;
                }

                B.Flag(a, b);
                break;

            case "new game":
                B.NewGame(width, height, mines);
                break;

            case "settings":
                Console.WriteLine("\nEnter width, then height, then amount of mines");
                if (!Int32.TryParse(Console.ReadLine() + "", out a) ||
                   !Int32.TryParse(Console.ReadLine() + "", out b) ||
                   !Int32.TryParse(Console.ReadLine() + "", out c))
                {
                    Console.WriteLine("\nError! Input caused parsing error");
                    break;
                }

                if (a < 1 || a > 65 || b < 1 || b > 100 || c < 1 || c >= a * b)
                {
                    Console.WriteLine("\nError! Input out of bounds");
                    break;
                }

                width = a;
                height = b;
                mines = c;

                B.gameOver = true;

                Console.WriteLine($"\nNow set to play on {width}x{height} board with {mines} mines");
                break;

            case "cheat":
                B.cheatMode = !B.cheatMode;
                Console.WriteLine($"\nCheat mode set to {B.cheatMode}");
                break;

            case "help":
                Help();
                break;

            default:
                Console.WriteLine("\nError! No such command exists");
                break;
        }
    }

    public static void Help()
    {
        Console.WriteLine("Enter \"help\" for help.");
        Console.WriteLine("Enter \"settings\" to change settings. Default is 5/5/5. Resets the current game!");
        Console.WriteLine("Enter \"cheat\" to change cheat mode.");
        Console.WriteLine("Enter \"new game\" to start new game.");
        Console.WriteLine("Enter \"reveal\" to reveal a tile. Selecting a number with eniugh flags around it reveal tiles nearby.");
        Console.WriteLine("Enter \"flag\" to add or remove a flag.");

        Console.WriteLine();

        Console.WriteLine("u - unknown tile");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("e");
        Console.ResetColor();
        Console.WriteLine(" - empty tile");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("1-8");
        Console.ResetColor();
        Console.WriteLine(" - tiles with 1-8 mines nearby");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("F");
        Console.ResetColor();
        Console.WriteLine(" - flagged tile");

        Console.WriteLine();
    } 
}
