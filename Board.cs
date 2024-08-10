namespace Minesweeper;

internal class Board
{
    public List<List<char>> board;
    public List<List<char>> displayBoard;
    public int sizeX;
    public int sizeY;
    public bool gameOver;
    public bool cheatMode;

    public Board()
    {
        board = [];
        displayBoard = [];
        sizeX = 0;
        sizeY = 0;
        gameOver = true;
        cheatMode = false;
    }

    public  void NewGame(int sizeX, int sizeY, int mines)
    {
        Random r = new();

        Clear();
        gameOver = false;

        // Creating the boards of needed size

        this.sizeX = sizeX;
        this.sizeY = sizeY;

        for (int i = 0; i < sizeX; i++)
        {
            board.Add([]);
            displayBoard.Add([]);
            for (int j = 0; j < sizeY; j++)
            {
                board[i].Add('e');
                displayBoard[i].Add('u');
            }
        }

        // Placing mines. Repeats till all are placed

        for (int i = 0; i < mines; i++)
        {
            int X = r.Next(sizeX);
            int Y = r.Next(sizeY);

            if (board[X][Y] == 'e')
            {
                board[X][Y] = 'm';
            }
            else
            {
                i--;
            }
        }

        Draw();
    }

    private void Clear()
    {
        board.Clear();
        displayBoard.Clear();
        sizeX = 0;
        sizeY = 0;
    }

    private void Draw()
    {
        // Drawing the board to console

        Console.WriteLine();

        Spaces((sizeY - 1).ToString().Length + 2);

        for (int X = 0; X < sizeX; X++)
        {
            Spaces((sizeX - 1).ToString().Length + 1 - X.ToString().Length);
            Console.Write(X);
        }

        Console.WriteLine("\n");

        // This function runs through all tiles and draws them

        for (int y = 0; y < sizeY; y++)
        {
            Console.Write(y);
            
            Spaces((sizeY - 1).ToString().Length + 2 - y.ToString().Length);
            
            for (int x = 0; x < sizeX; x++)
            {
                Spaces((sizeX - 1).ToString().Length);

                // This part mostly colors tiles

                if ((displayBoard[x][y] == 'x') || (displayBoard[x][y] == 'X'))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (!cheatMode) 
                {
                    if ((displayBoard[x][y] == 'm') && !gameOver)
                    {
                        displayBoard[x][y] = 'u';
                    }

                    if (displayBoard[x][y] == 'F')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (board[x][y] != 'm')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                } 
                else
                {
                    if ((board[x][y] == 'm') && (displayBoard[x][y] == 'u'))
                    {
                        displayBoard[x][y] = 'm';
                    }

                    if ((displayBoard[x][y] == 'F') && (board[x][y] == 'm'))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    } 
                    else if (((displayBoard[x][y] == 'F') && (board[x][y] == 'e'))
                        || ((board[x][y] == 'm') && (displayBoard[x][y] == 'm')))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (board[x][y] != 'm')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                }

                if (displayBoard[x][y] == 'u')
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (displayBoard[x][y] == 'e')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if ((displayBoard[x][y] == 'm') && gameOver)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                Console.Write(displayBoard[x][y]);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private char Check(int X, int Y)
    {
        // Safe tile lookup. Prevents errors when trying to look up neigbour values

        if (OutOfBounds(X, Y))
        {
            return 'e';
        }
        return board[X][Y];
    }

    public void Reveal(int X, int Y)
    {
        Reveal(X, Y, 0);
    }

    private void Reveal(int X, int Y, int depth)
    {
        // Function for revealing the tile.
        // Depth is used to prevent excessive drawing and such

        if (displayBoard[X][Y] == 'F')
        {
            if (depth == 0)
            {
                Console.WriteLine("\nError! Can't reveal a flag");
            }
            return;
        }

        if (board[X][Y] == 'm')
        {
            displayBoard[X][Y] = 'X';
            GameOver();
            return;
        }

        if (displayBoard[X][Y] == 'e')
        {
            Console.WriteLine("\nError! Can't reveal a revealed space");
            return;
        }

        // This part is for revealing a numbered tile

        if (Char.IsNumber(displayBoard[X][Y]))
        {
            int f = 0;

            // Counting flags

            for (int x = X - 1; x < X + 2; x++)
            {
                for (int y = Y - 1; y < Y + 2; y++)
                {
                    if (!SameTile(x, y, X, Y) && !OutOfBounds(x, y) && (displayBoard[x][y] == 'F'))
                    {
                        f++;
                    }
                }
            }

            // Revealing tiles if number matches ammount of flags

            if (Int32.Parse(displayBoard[X][Y].ToString()) == f)
            {
                for (int x = X - 1; x < X + 2; x++)
                {
                    for (int y = Y - 1; y < Y + 2; y++)
                    {
                        if (!SameTile(x, y, X, Y) && !OutOfBounds(x, y) && (displayBoard[x][y] == 'u'))
                        {
                            Reveal(x, y, depth + 1);
                        }
                    }
                }
            }

            if (Int32.Parse(displayBoard[X][Y].ToString()) > f)
            {
                Console.WriteLine("\nError! Not enough flags");
                return;
            }

            if (Int32.Parse(displayBoard[X][Y].ToString()) < f)
            {
                Console.WriteLine("\nError! Too much flags");
                return;
            }
        }

        int m = 0;

        // Counting mines around tile

        for (int x = X - 1; x < X + 2; x++)
        {
            for (int y = Y - 1; y < Y + 2; y++)
            {
                if (!SameTile(x, y, X, Y))
                {
                    if (Check(x, y) == 'm')
                    {
                        m++;
                    }
                }
            }
        }

        // Revealing an empty tile recursively

        if (m == 0)
        {
            displayBoard[X][Y] = 'e';

            for (int x = X - 1; x < X + 2; x++)
            {
                for (int y = Y - 1; y < Y + 2; y++)
                {
                    if (!SameTile(x, y, X, Y) && !OutOfBounds(x, y) && (displayBoard[x][y] == 'u'))
                    {
                        Reveal(x, y, depth + 1);
                    }
                }
            }
        } 
        else
        {
            displayBoard[X][Y] = m.ToString()[0];
        }

        if ((depth == 0) && !gameOver)
        {
            Draw();
            TryWinning();
        }
    }

    public void Flag(int X, int Y)
    {
        if ((displayBoard[X][Y] == 'u') || (displayBoard[X][Y] == 'm'))
        {
            displayBoard[X][Y] = 'F';
        }
        else if (displayBoard[X][Y] == 'F')
        {
            if (cheatMode && (board[X][Y] == 'm'))
            {
                displayBoard[X][Y] = 'm';
            }
            else
            {
                displayBoard[X][Y] = 'u';
            }
        }
        else
        {
            Console.WriteLine("\nError! Can't put a flag here");
            return;
        }

        Draw();
    }

    private void GameOver()
    {
        // Draws exploded mines

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if ((board[x][y] == 'm') && (displayBoard[x][y] != 'X'))
                {
                    displayBoard[x][y] = 'x';
                }
            }
        }

        gameOver = true;

        Draw();

        Console.WriteLine("Game over!");
    }

    private void Win()
    {
        // Shows mines

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (board[x][y] == 'm')
                {
                    displayBoard[x][y] = 'm';
                }
            }
        }

        gameOver = true;

        Draw();

        Console.WriteLine("You win!");
    }

    private void TryWinning()
    {
        int errors = 0;

        // Counting errors

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (((displayBoard[x][y] == 'F') || (displayBoard[x][y] == 'u')) && (board[x][y] == 'e'))
                {
                    errors++;
                }
            }
        }

        if (errors == 0)
        {
            Win();
        }
    }

    private bool OutOfBounds(int X, int Y)
    {
        // Checks if coords are outside of the board

        return (X >= sizeX) || (X < 0) || (Y >= sizeY) || (Y < 0);
    }

    private static bool SameTile(int x, int y, int X, int Y)
    {
        // Checks if it's the same tile

        return (x == X) && (y == Y);
    }

    private static void Spaces(int n)
    {
        // Function for printing a number of spaces

        for (int i = 0; i < n; i++)
        {
            Console.Write(" ");
        }
    }
}