using System;

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }
    public int RemainingTurns { get; set; }

    public Player(string name, Position position, int remainingTurns)
    {
        Name = name;
        Position = position;
        GemCount = 0;
        RemainingTurns = remainingTurns;
    }

    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U':
                Position.Y -= 1;
                break;
            case 'D':
                Position.Y += 1;
                break;
            case 'L':
                Position.X -= 1;
                break;
            case 'R':
                Position.X += 1;
                break;
        }
    }
}

class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant = "-")
    {
        Occupant = occupant;
    }
}

class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        InitializeBoard();
    }

    private void PlaceObstacles()
    {
        Random rand = new Random();
        for (int i = 0; i < 5; i++)
        {
            int obstacleX, obstacleY;
            do
            {
                obstacleX = rand.Next(6);
                obstacleY = rand.Next(6);
            } while (Grid[obstacleY, obstacleX].Occupant != "-");

            Grid[obstacleY, obstacleX].Occupant = "O";
        }
    }

    private void PlaceGems()
    {
        Random rand = new Random();
        for (int i = 0; i < 3; i++)
        {
            int gemX, gemY;
            do
            {
                gemX = rand.Next(6);
                gemY = rand.Next(6);
            } while (Grid[gemY, gemX].Occupant != "-");

            Grid[gemY, gemX].Occupant = "G";
        }
    }

    public void InitializeBoard()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell();
            }
        }

        // To place the players in their position in the beginning

        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        // Place obstacles randomly 
        PlaceObstacles();

        // Place gems at random positions
        PlaceGems();
    }



    //To display the main board and players
    public void Display()
    {
        Console.Clear(); // Clear the console screen

        Console.WriteLine("Gem Hunters - Assignment 2:\n");
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write($"{Grid[i, j].Occupant}\t");
            }
            Console.WriteLine();  // Add a line break for better board display
        }
        Console.WriteLine();
    }

    private string GetCellSymbol(int x, int y)
    {
        foreach (var player in new[] { "P1", "P2" })
        {
            if (Grid[y, x].Occupant == player)
                return player;
        }

        return Grid[y, x].Occupant;
    }

    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U':
                newY -= 1;
                break;
            case 'D':
                newY += 1;
                break;
            case 'L':
                newX -= 1;
                break;
            case 'R':
                newX += 1;
                break;
        }

        if (newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && Grid[newY, newX].Occupant != "O")
        {
            return true;
        }
        return false;
    }

    public void CollectGem(Player player)
    {
        Cell currentCell = Grid[player.Position.Y, player.Position.X];
        if (currentCell.Occupant == "G")
        {
            player.GemCount++;
            currentCell.Occupant = "-";
        }
    }

    public void UpdatePlayerPosition(Player player)
    {
        Grid[player.Position.Y, player.Position.X].Occupant = player.Name;
    }

    public void ClearPreviousPosition(Player player)
    {
        Grid[player.Position.Y, player.Position.X].Occupant = "-";
    }
}

class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; set; }
    public int TotalMoves { get; set; }
    public int MaxMoves { get; }

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0), 15);
        Player2 = new Player("P2", new Position(5, 5), 15);
        CurrentTurn = Player1;
        TotalMoves = 0;
        MaxMoves = 30;
    }

    public void Start()
    {
        Console.WriteLine("Gem Hunters Game\n");

        while (!IsGameOver())
        {
            // To show the ccurrent board
            Board.Display();
            PlayerTurn();
            SwitchTurn();
        }

        // To show the board at the end of the game
        Board.Display();
        AnnounceWinner();
    }


    // To alternate the turn between the players
    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    //To show the remaining turn and the direction inputted by the players

    public void PlayerTurn()
    {
        Console.WriteLine($"{CurrentTurn.Name}'s turn - Remaining Turns: {CurrentTurn.RemainingTurns}");
        Console.Write("Enter direction (U/D/L/R): ");
        char direction = char.ToUpper(Console.ReadKey().KeyChar);
        Console.WriteLine();

        if (Board.IsValidMove(CurrentTurn, direction))
        {
            Board.ClearPreviousPosition(CurrentTurn);
            CurrentTurn.Move(direction);
            Board.CollectGem(CurrentTurn);
            Board.UpdatePlayerPosition(CurrentTurn);
            TotalMoves++;
            CurrentTurn.RemainingTurns--;
        }
        else
        {
            Console.WriteLine("Invalid move. Try again.");
        }
    }

    public bool IsGameOver()
    {
        return TotalMoves >= MaxMoves || (Player1.RemainingTurns == 0 && Player2.RemainingTurns == 0);
    }


    // To show the gem taken by the players and show the player who has the most gem as a winner
    public void AnnounceWinner()
    {
        Console.WriteLine("\nGame Over!");
        Console.WriteLine($"Player 1 Gems: {Player1.GemCount}");
        Console.WriteLine($"Player 2 Gems: {Player2.GemCount}");

        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine("Player 1 wins!");
        }
        else if (Player1.GemCount < Player2.GemCount)
        {
            Console.WriteLine("Player 2 wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}

class Program
{
    static void Main()
    {
        Game G = new Game();
        G.Start();
    }
}
