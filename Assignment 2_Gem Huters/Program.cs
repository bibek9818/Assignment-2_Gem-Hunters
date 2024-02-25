using System;
using System.Xml.Linq;





class Player
{


    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }



    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U':
                if (Position.Y > 0)
                {
                    Position.Y--;
                }
                break;
            case 'D':
                if (Position.Y < 5)
                {
                    Position.Y++;
                }
                break;
            case 'L':
                if (Position.X > 0)
                {
                    Position.X--;
                }
                break;
            case 'R':
                if (Position.X < 5)
                {
                    Position.X++;
                }
                break;
        }
    }





}



class game
{
    public void Start()
    {
        do
        {
            Console.Clear();
            Board.Display();
            Console.WriteLine($"{CurrentTurn.Name}'s turn. Enter move (U/D/L/R): ");
            char move = Console.ReadKey().KeyChar;

            if (Board.IsValidMove(CurrentTurn, move))
            {
                CurrentTurn.Move(move);
                Board.CollectGem(CurrentTurn);
                TotalTurns++;
                SwitchTurn();
            }

        } while (!IsGameOver());

        Console.Clear();
        Board.Display();
        AnnounceWinner();
    }
}

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







public void CheckGemCollection(Player player)
{
    if (board[player.Position.X, player.Position.Y] == 'G')
    {
        player.GemCount++;
        board[player.Position.X, player.Position.Y] = '-'; // Remove the gem from the board
        Console.WriteLine($"{player.Name} collected a gem!");
    }
}

public void DisplayResults()
{
    Player winner = GetWinner();
    Console.WriteLine("Gem Hunters - Game Over");
    Console.WriteLine($"Player 1's Gems: {players[0].GemCount}");
    Console.WriteLine($"Player 2's Gems: {players[1].GemCount}");

    if (players[0].GemCount == players[1].GemCount)
    {
        Console.WriteLine("It's a tie!");
    }
    else
    {
        Console.WriteLine($"{winner.Name} wins!");
    }
}

public Player GetWinner()
{
    if (players[0].GemCount > players[1].GemCount)
    {
        return players[0];
    }
    else if (players[1].GemCount > players[0].GemCount)
    {
        return players[1];
    }
    else
    {
        int winsP1 = Math.Sign(players[0].GemCount - players[1].GemCount);
        int winsP2 = -winsP1;
        return winsP1 > winsP2 ? players[0] : players[1];
    }
}

