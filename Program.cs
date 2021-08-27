using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter20
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //board setup
                string userInputString;
                int userInput;
                int boardSize;
                Board board;

                Console.Write("Enter board size (minimum size is 3): ");
                userInputString = Console.ReadLine();

                if (!int.TryParse(userInputString, out userInput) || userInput < 3)
                {
                    Console.WriteLine("Invalid input. Enter a number equal to or greater than 3.");
                    continue;
                }

                userInput = Convert.ToInt32(userInputString);
                boardSize = userInput;
                board = new Board(boardSize);

                //players turns
                int squareValue;
                bool XToMove = true;

                while (true)
                {
                    string moveString;
                    string[] moveStringSplit;
                    int rowInput;
                    int columnInput;

                    if (XToMove)
                    {
                        Console.WriteLine("X to move.");
                        squareValue = 1;
                    }
                    else
                    {
                        Console.WriteLine("O to move.");
                        squareValue = 2;
                    }

                    Console.WriteLine("Enter 'row,column' to move. For example, '1,2'.");
                    moveString = Console.ReadLine();
                    moveStringSplit = moveString.Split(',');

                    //validate move
                    if (!int.TryParse(moveStringSplit[0], out rowInput) || !int.TryParse(moveStringSplit[1], out columnInput))
                    {
                        Console.WriteLine("Invalid input.");
                        continue;
                    }

                    if (board.GetSquareValue(rowInput, columnInput) > 0)
                    {
                        Console.WriteLine($"Invalid move, square {rowInput},{columnInput} is not empty.");
                        continue;
                    }
                    board.SetSquareValue(rowInput, columnInput, squareValue);
                    board.PrintBoard();

                    //check win conditions
                    if (board.CheckWin(rowInput, columnInput))
                    {
                        Console.WriteLine("Win detected.\n");
                        break;
                    }

                    //check draw conditions (full board)
                    if (board.CheckDraw())
                    {
                        Console.WriteLine("Draw detected.\n");
                        break;
                    }

                    //invert bool for next move
                    XToMove = !XToMove;
                }
            }
        }
    }
}
