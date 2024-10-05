using System;
using System.Drawing;
using System.Xml.Linq;

namespace MiniGameHub
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Mini-Game Hub!");
                Console.WriteLine("Choose a game to play:");
                Console.WriteLine("1. Retro Snake");
                Console.WriteLine("2. Rock Paper Scissors");
                Console.WriteLine("3. Tic Tac Toe");
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PlayRetroSnake();
                        break;
                    case "2":
                        PlayRockPaperScissors();
                        break;
                    case "3":
                        PlayTicTacToe();
                        break;
                    case "4":
                        Console.WriteLine("Thanks for playing! Goodbye.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static void PlayRetroSnake()
        {
            Console.Clear();
            Console.WriteLine("Starting Retro Snake...");
            // using Snake;
            Random random = new Random();
            Coord gridDimensions = new Coord(50, 20);

            Coord snakePos = new Coord(10, 1);
            Direction movementDirection = Direction.Down;
            List<Coord> snakePosHistory = new List<Coord>();
            int tailLength = 1;
            Coord applePos = new Coord(random.Next(1, gridDimensions.X - 1), random.Next(1, gridDimensions.Y - 1));
            int frameDelayMilli = 100;
            int score = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Score: " + score);
                snakePos.ApplyMovementDirection(movementDirection);

                // Render the game to the Console
                for (int y = 0; y < gridDimensions.Y; y++)
                {
                    for (int x = 0; x < gridDimensions.X; x++)
                    {
                        Coord currentCoord = new Coord(x, y);

                        if (snakePos.Equals(currentCoord) || snakePosHistory.Contains(currentCoord))
                            Console.Write("■");
                        else if (applePos.Equals(currentCoord))
                            Console.Write("a");
                        else if (x == 0 || y == 0 || x == gridDimensions.X - 1 || y == gridDimensions.Y - 1)
                            Console.Write("#");
                        else
                            Console.Write(" ");
                    }
                    Console.WriteLine();
                }

                // Check if snake has picked up apple
                if (snakePos.Equals(applePos))
                {
                    tailLength++;
                    score++;
                    applePos = new Coord(random.Next(1, gridDimensions.X - 1), random.Next(1, gridDimensions.Y - 1));
                }
                // Check for game over conditions - snake has hit wall or snake has hit tail
                else if (snakePos.X == 0 || snakePos.Y == 0 || snakePos.X == gridDimensions.X - 1 ||
                    snakePos.Y == gridDimensions.Y - 1 || snakePosHistory.Contains(snakePos))
                {
                    // Reset game
                    score = 0;
                    tailLength = 1;
                    snakePos = new Coord(10, 1);
                    snakePosHistory.Clear();
                    movementDirection = Direction.Down;
                    continue;
                }

                // Add the snake's current position to the snakePosHistory list
                snakePosHistory.Add(new Coord(snakePos.X, snakePos.Y));

                if (snakePosHistory.Count > tailLength)
                    snakePosHistory.RemoveAt(0);


                // Delay the rendering of next frame for frameDelayMilli milliseconds whilst checking for player input
                DateTime time = DateTime.Now;

                while ((DateTime.Now - time).Milliseconds < frameDelayMilli)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey().Key;

                        // Assign snake new direction to move in based on what key was pressed
                        switch (key)
                        {
                            case ConsoleKey.LeftArrow:
                                movementDirection = Direction.Left;
                                break;
                            case ConsoleKey.RightArrow:
                                movementDirection = Direction.Right;
                                break;
                            case ConsoleKey.UpArrow:
                                movementDirection = Direction.Up;
                                break;
                            case ConsoleKey.DownArrow:
                                movementDirection = Direction.Down;
                                break;
                        }
                    }
                }
                Console.WriteLine("Retro Snake is under construction!");
            }
        }

        static void PlayRockPaperScissors()
        {
            Console.Clear();
            Console.WriteLine("Starting Rock Paper Scissors...");

            string[] choices = { "Rock", "Paper", "Scissors" };
            Random random = new Random();
            bool playAgain = true;

            Console.WriteLine("Welcome to Rock, Paper, Scissors!");

            while (playAgain)
            {
                // Display the choices for the player
                Console.WriteLine("\nChoose one:");
                Console.WriteLine("1. Rock");
                Console.WriteLine("2. Paper");
                Console.WriteLine("3. Scissors");
                Console.WriteLine("Enter the number of your choice: ");

                // Get player choice
                string playerInput = Console.ReadLine();
                int playerChoice;

                // Validate input
                if (!int.TryParse(playerInput, out playerChoice) || playerChoice < 1 || playerChoice > 3)
                {
                    Console.WriteLine("Invalid choice, please select 1, 2, or 3.");
                    continue;
                }

                // Convert player input to index
                playerChoice--; // Convert to 0-based index

                // Get computer choice
                int computerChoice = random.Next(0, 3);

                // Display both choices
                Console.WriteLine($"You chose: {choices[playerChoice]}");
                Console.WriteLine($"Computer chose: {choices[computerChoice]}");

                // Determine winner
                if (playerChoice == computerChoice)
                {
                    Console.WriteLine("It's a tie!");
                }
                else if ((playerChoice == 0 && computerChoice == 2) || // Rock beats Scissors
                         (playerChoice == 1 && computerChoice == 0) || // Paper beats Rock
                         (playerChoice == 2 && computerChoice == 1))   // Scissors beats Paper
                {
                    Console.WriteLine("You win!");
                }
                else
                {
                    Console.WriteLine("Computer wins!");
                }

                // Ask if player wants to play again
                Console.WriteLine("Do you want to play again? (y/n)");
                string playAgainInput = Console.ReadLine().ToLower();

                if (playAgainInput != "y")
                {
                    playAgain = false;
                }
            }

            Console.WriteLine("Thanks for playing!");

            Console.WriteLine("Rock Paper Scissors is under construction!");
        }

        static void PlayTicTacToe()
        {
            Console.Clear();
            Console.WriteLine("Starting Tic Tac Toe...");

                string[] grid = new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                bool player1Turn = true;
                int numTurns = 0;
                int[] secretPattern = new int[] { 1, 5, 7, 4, 6, 2, 8, 9, 3 };
                int[] movesHistory = new int[9];

                while (!CheckVictory(grid) && numTurns != 9)
                {
                    PrintGrid(grid);

                    if (player1Turn)
                        Console.WriteLine("Player 1 Turn! Choose a position (1-9):");
                    else
                        Console.WriteLine("Player 2 Turn! Choose a position (1-9):");

                    string choice = Console.ReadLine();

                    // Check if input is valid
                    if (grid.Contains(choice) && choice != "X" && choice != "O")
                    {
                        int gridIndex = Convert.ToInt32(choice) - 1;

                        if (player1Turn)
                            grid[gridIndex] = "X";
                        else
                            grid[gridIndex] = "O";

                        // Record move in history
                        movesHistory[numTurns] = gridIndex + 1;
                        numTurns++;

                        // Check for secret pattern
                        if (numTurns == 9 && CheckSecretPattern(movesHistory, secretPattern))
                        {
                            PrintGrid(grid);
                            Console.WriteLine("Secret message: Congratulations! You cracked Addverb");
                            return;
                        }

                        player1Turn = !player1Turn;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                }

                PrintGrid(grid);

                if (CheckVictory(grid))
                    Console.WriteLine("You win!");
                else
                    Console.WriteLine("It's a tie!");
         

            // Function to check the secret pattern
            static bool CheckSecretPattern(int[] movesHistory, int[] secretPattern)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (movesHistory[i] != secretPattern[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            // Function to print the grid
            static void PrintGrid(string[] grid)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Console.Write(grid[i * 3 + j] + "|");
                    }
                    Console.WriteLine("");
                    Console.WriteLine("------");
                }
            }

        // Function to check for a winning condition
        bool CheckVictory(string[] grid)
        {
            bool row1 = grid[0] == grid[1] && grid[1] == grid[2];
            bool row2 = grid[3] == grid[4] && grid[4] == grid[5];
            bool row3 = grid[6] == grid[7] && grid[7] == grid[8];
            bool col1 = grid[0] == grid[3] && grid[3] == grid[6];
            bool col2 = grid[1] == grid[4] && grid[4] == grid[7];
            bool col3 = grid[2] == grid[5] && grid[5] == grid[8];
            bool diagDown = grid[0] == grid[4] && grid[4] == grid[8];
            bool diagUp = grid[6] == grid[4] && grid[4] == grid[2];

            return row1 || row2 || row3 || col1 || col2 || col3 || diagDown || diagUp;
        }
            Console.WriteLine("Tic Tac Toe is under construction!");
        
        }
    }
}
