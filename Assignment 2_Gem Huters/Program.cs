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




public void PlaceGemsAndObstacles()
{



}




//Creating the display method
public void Display()
{

}


//Creating the method for collectinggem
public void CollectGem()
{

}


