using System;

class TicTacToe
{
    static char[] board = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    static int currentPlayer = 1; // 1 - X, 2 - O
    static bool gameOver = false;

    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в игру Крестики-нолики!");
        Console.WriteLine("Игрок 1: X, Игрок 2: O\n");

        while (!gameOver)
        {
            DrawBoard();
            PlayerTurn();
            CheckGameStatus();
            if (!gameOver)
                currentPlayer = currentPlayer == 1 ? 2 : 1;
        }
    }

    static void DrawBoard()
    {
        Console.Clear();
        Console.WriteLine(" {0} | {1} | {2} ", board[0], board[1], board[2]);
        Console.WriteLine("-----------");
        Console.WriteLine(" {0} | {1} | {2} ", board[3], board[4], board[5]);
        Console.WriteLine("-----------");
        Console.WriteLine(" {0} | {1} | {2} ", board[6], board[7], board[8]);
    }

    static void PlayerTurn()
    {
        bool validInput = false;
        int choice = 0;

        while (!validInput)
        {
            Console.Write($"Игрок {currentPlayer}, введите номер клетки: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out choice) && choice >= 1 && choice <= 9)
            {
                if (board[choice - 1] != 'X' && board[choice - 1] != 'O')
                {
                    board[choice - 1] = currentPlayer == 1 ? 'X' : 'O';
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Эта клетка уже занята! Попробуйте другую.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод! Введите число от 1 до 9.");
            }
        }
    }

    static void CheckGameStatus()
    {
        if (CheckWin())
        {
            DrawBoard();
            Console.WriteLine($"Игрок {currentPlayer} победил!");
            gameOver = true;
            return;
        }

        if (CheckDraw())
        {
            DrawBoard();
            Console.WriteLine("Ничья!");
            gameOver = true;
            return;
        }
    }

    static bool CheckWin()
    {
        if (board[0] == board[1] && board[1] == board[2]) return true;
        if (board[3] == board[4] && board[4] == board[5]) return true;
        if (board[6] == board[7] && board[7] == board[8]) return true;

        if (board[0] == board[3] && board[3] == board[6]) return true;
        if (board[1] == board[4] && board[4] == board[7]) return true;
        if (board[2] == board[5] && board[5] == board[8]) return true;

        if (board[0] == board[4] && board[4] == board[8]) return true;
        if (board[2] == board[4] && board[4] == board[6]) return true;

        return false;
    }

    static bool CheckDraw()
    {
        foreach (char cell in board)
        {
            if (cell != 'X' && cell != 'O')
            {
                return false;
            }
        }
        return true;
    }
}