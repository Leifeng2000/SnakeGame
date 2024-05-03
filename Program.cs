using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static int screenWidth = 50;
    static int screenHeight = 20;
    static int score = 0;
    static bool gameOver = false;

    static int snakeHeadX;
    static int snakeHeadY;
    static List<int> snakeBodyX;
    static List<int> snakeBodyY;
    static int baitX;
    static int baitY;

    static Direction direction;

    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(screenWidth, screenHeight);

        InitializeGame();

        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.Right;
                        break;
                    case ConsoleKey.UpArrow:
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Direction.Down;
                        break;
                }
            }

            MoveSnake();
            CheckCollision();
            GenerateBait();

            if (!gameOver)
            {
                DrawGame();
                Thread.Sleep(100);
            }
        }

        Console.Clear();
        Console.WriteLine("Game Over! Your score is: " + score);
        Console.ReadKey();
    }

    static void InitializeGame()
    {
        snakeHeadX = screenWidth / 2;
        snakeHeadY = screenHeight / 2;

        snakeBodyX = new List<int> { snakeHeadX };
        snakeBodyY = new List<int> { snakeHeadY };

        baitX = 0;
        baitY = 0;

        direction = Direction.Right;
    }

    static void DrawGame()
    {
        Console.Clear();

        for (int i = 0; i < screenWidth; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("#");
            Console.SetCursorPosition(i, screenHeight - 1);
            Console.Write("#");
        }

        for (int i = 0; i < screenHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("#");
            Console.SetCursorPosition(screenWidth - 1, i);
            Console.Write("#");
        }

        Console.SetCursorPosition(baitX, baitY);
        Console.Write("O");

        for (int i = 0; i < snakeBodyX.Count; i++)
        {
            Console.SetCursorPosition(snakeBodyX[i], snakeBodyY[i]);
            Console.Write(i == 0 ? "@" : "#");
        }
    }

    static void MoveSnake()
    {
        int snakeTailX = snakeBodyX.Last();
        int snakeTailY = snakeBodyY.Last();

        for (int i = snakeBodyX.Count - 1; i > 0; i--)
        {
            snakeBodyX[i] = snakeBodyX[i - 1];
            snakeBodyY[i] = snakeBodyY[i - 1];
        }

        snakeBodyX[0] = snakeHeadX;
        snakeBodyY[0] = snakeHeadY;

        switch (direction)
        {
            case Direction.Left:
                snakeHeadX--;
                break;
            case Direction.Right:
                snakeHeadX++;
                break;
            case Direction.Up:
                snakeHeadY--;
                break;
            case Direction.Down:
                snakeHeadY++;
                break;
        }

        Console.SetCursorPosition(snakeTailX, snakeTailY);
        Console.Write(" ");

        Console.SetCursorPosition(snakeHeadX, snakeHeadY);
        Console.Write("@");
    }

    static void CheckCollision()
    {
        if (snakeHeadX == 0 || snakeHeadX == screenWidth - 1 || snakeHeadY == 0 || snakeHeadY == screenHeight - 1)
        {
            gameOver = true;
            return;
        }

        for (int i = 1; i < snakeBodyX.Count; i++)
        {
            if (snakeHeadX == snakeBodyX[i] && snakeHeadY == snakeBodyY[i])
            {
                gameOver = true;
                return;
            }
        }

        if (snakeHeadX == baitX && snakeHeadY == baitY)
        {
            score++;
            snakeBodyX.Add(snakeBodyX.Last());
            snakeBodyY.Add(snakeBodyY.Last());

            baitX = 0;
            baitY = 0;
        }
    }

    static void GenerateBait()
    {
        if (baitX == 0 && baitY == 0)
        {
            Random random = new Random();
            baitX = random.Next(1, screenWidth - 1);
            baitY = random.Next(1, screenHeight - 1);
        }

        Console.SetCursorPosition(baitX, baitY);
        Console.Write("O");

        if (snakeHeadX == baitX && snakeHeadY == baitY)
        {
            score++;
            snakeBodyX.Add(snakeBodyX.Last());
            snakeBodyY.Add(snakeBodyY.Last());

            baitX = 0;
            baitY = 0;
        }
    }
}

enum Direction
{
    Left,
    Right,
    Up,
    Down
}