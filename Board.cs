using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter20
{
    class Board
    {
        enum State { X = 1, O = 2 };
        private int[,] matrix;
        private int size;
        private bool winDetected = false;
        private bool drawDetected = false;

        /// <summary>
        /// Creates a (size) X (size) square board.
        /// A variable of type int[,] named matrix is initialized with zeroes.
        /// A board is printed to console.
        /// </summary>
        /// <param name="size">Minimum size is 3</param>
        public Board(int size)
        {
            if (size > 2)
            {
                this.size = size;
                matrix = new int[size, size];

                for (int row = 0; row < size; row++)
                {
                    for (int column = 0; column < size; column++)
                    {
                        matrix[row, column] = 0;
                    }
                }

                PrintBoard();
            }
        }

        public int GetSquareValue(int row, int column)
        {
            return matrix[row, column];
        }

        public void SetSquareValue(int row, int column, int value)
        {
            matrix[row, column] = value;
            Console.WriteLine($"[{row},{column} = {value}] ");
        }

        public void PrintBoard()
        {
            Console.WriteLine();
            Console.Write(" ");
            for (int column = 0; column < size; column++)
            {
                Console.Write($"  {column} ");
            }
            Console.WriteLine();

            Console.Write(" -");
            for (int column = 0; column < size; column++)
            {
                Console.Write("----");
            }
            Console.WriteLine();

            for (int row = 0; row < size; row++)
            {
                Console.Write($"{row}|");
                for (int column = 0; column < size; column++)
                {
                    Console.Write("");
                    if (matrix[row, column] == 0)
                    {
                        Console.Write("   |");
                    }
                    else
                    {
                        Console.Write($" {(State)matrix[row, column]} |");
                    }
                }
                Console.WriteLine();

                Console.Write(" -");
                for (int column = 0; column < size; column++)
                {
                    Console.Write("----");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Counts how many other consecutive elements in the row has the same value as the starting column.
        /// </summary>
        /// <param name="row">Row to check.</param>
        /// <param name="column">Starting column.</param>
        /// <param name="counter">Number of elements containing the same value as the starting index.</param>
        /// <param name="direction">Direction to check. Values more than 1 or less than -1 will skip elements.</param>
        /// <returns></returns>
        private int CheckRow(int row, int column, int counter, int direction)
        {
            if (column + direction >= 0 && column + direction < size)
            {
                if (matrix[row, column] == matrix[row, column + direction])
                {
                    counter++;
                    Console.WriteLine($"[{row},{column}]-[{row},{column + direction}] counter: {counter}");
                    return CheckRow(row, column + direction, counter, direction);
                }
                else
                {
                    return counter;
                }
            }
            else
            {
                return counter;
            }
        }

        /// <summary>
        /// Counts how many other consecutive elements in the column has the same value as the starting row.
        /// </summary>
        /// <param name="row">Starting row.</param>
        /// <param name="column">Column to check.</param>
        /// <param name="counter">Number of elements containing the same value as the starting index.</param>
        /// <param name="direction">Direction to check. Values more than 1 or less than -1 will skip elements.</param>
        /// <returns></returns>
        private int CheckColumn(int row, int column, int counter, int direction)
        {
            if (row + direction >= 0 && row + direction < size)
            {
                if (matrix[row, column] == matrix[row + direction, column])
                {
                    counter++;
                    Console.WriteLine($"[{row},{column}]-[{row + direction},{column}] counter: {counter}");
                    return CheckColumn(row + direction, column, counter, direction);
                }
                else
                {
                    return counter;
                }
            }
            else
            {
                return counter;
            }
        }

        /// <summary>
        /// Counts how many other consecutive elements are in diagonals in +column,-row and -column,+row directions.
        /// </summary>
        /// <param name="row">Starting row.</param>
        /// <param name="column">Starting column.</param>
        /// <param name="counter">Number of elements containing the same value as the starting index.</param>
        /// <param name="direction">Direction to check. Values more than 1 or less than -1 will skip elements.</param>
        /// <returns></returns>
        private int CheckDiagOne(int row, int column, int counter, int direction)
        {
            //ensure the next index is within bounds
            if (row - direction >= 0 && row - direction < size && column + direction >= 0 && column + direction < size)
            {
                //Console.WriteLine($"[{row},{column}]");
                //Console.WriteLine($"[{row-direction},{column+direction}]");
                if (matrix[row, column] == matrix[row - direction, column + direction])
                {
                    counter++;
                    Console.WriteLine($"[{row},{column}]-[{row - direction},{column + direction}] counter: {counter}");
                    return CheckDiagOne(row - direction, column + direction, counter, direction);
                }
                else
                {
                    return counter;
                }
            }
            else
            {
                return counter;
            }
        }

        /// <summary>
        /// Counts how many other consecutive elements are in diagonals in +column,+row and -column,-row directions
        /// </summary>
        /// <param name="row">Starting row.</param>
        /// <param name="column">Starting column.</param>
        /// <param name="counter">Number of elements containing the same value as the starting index.</param>
        /// <param name="direction">Direction to check. Values more than 1 or less than -1 will skip elements.</param>
        /// <returns></returns>
        private int CheckDiagTwo(int row, int column, int counter, int direction)
        {
            //ensure the next index is within bounds
            if (row + direction >= 0 && row + direction < size && column + direction >= 0 && column + direction < size)
            {
                //Console.WriteLine($"[{row},{column}]");
                //Console.WriteLine($"[{row-direction},{column+direction}]");
                if (matrix[row, column] == matrix[row + direction, column + direction])
                {
                    counter++;
                    Console.WriteLine($"[{row},{column}]-[{row + direction},{column + direction}] counter: {counter}");
                    return CheckDiagTwo(row + direction, column + direction, counter, direction);
                }
                else
                {
                    return counter;
                }
            }
            else
            {
                return counter;
            }
        }

        /// <summary>
        /// Checks through each of the win conditions.
        /// </summary>
        /// <param name="row">Starting row.</param>
        /// <param name="column">Starting column.</param>
        /// <returns></returns>
        public bool CheckWin(int row, int column)
        {
            int horizontalStreak;
            int verticalStreak;
            int diagonalOneStreak;
            int diagonalTwoStreak;

            //check for streaks towards the right
            horizontalStreak = CheckRow(row, column, 0, 1) + CheckRow(row, column, 0, -1);
            verticalStreak = CheckColumn(row, column, 0, 1) + CheckColumn(row, column, 0, -1);
            diagonalOneStreak = CheckDiagOne(row, column, 0, 1) + CheckDiagOne(row, column, 0, -1);
            diagonalTwoStreak = CheckDiagTwo(row, column, 0, 1) + CheckDiagTwo(row, column, 0, -1);

            if (horizontalStreak >= size - 1 || verticalStreak >= size - 1 || diagonalOneStreak >= size - 1 || diagonalTwoStreak >= size - 1)
            {
                winDetected = true;
            }
            else
            {
                winDetected = false;
            }

            return winDetected;
        }

        public bool CheckDraw()
        {
            int boardSum = 0;

            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    if (matrix[row, column] == 1 || matrix[row, column] == 2)
                    {
                        boardSum++;
                    }
                }
            }

            if (boardSum == size*size && !winDetected)
            {
                drawDetected = true;
            }
            else
            {
                drawDetected = false;
            }

            return drawDetected;
        }
    }
}
