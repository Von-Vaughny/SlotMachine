using System;

namespace SlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int NDX_0 = 0, GAME_1 = 1, NDX_1 = 1, NDX_2 = 2, GAME_3 = 3, ROW_COLUMN_LENGTH = 3, NUM_CHECKS_2 = 2, NUM_CHECKS_3 = 3, NUM_CHECKS_5 = 5, NUM_CHECKS_6 = 6;
            const decimal HALF_BONUS = 0.50M;
            const string WIN = "WIN!", LOST = "LOST!";
            decimal playerMoney = 20.00M;
            Random rng = new Random();
            ConsoleKeyInfo userInput;
            
            Console.WriteLine("Welcome to Slot Machine!");

            while (true)
            {
                // Get Random Numbers for Slot Machine
                int[,] slotMachine = new int[3,3];
                for (int i = 0; i < ROW_COLUMN_LENGTH; i++)
                {
                    for (int j = 0; j < ROW_COLUMN_LENGTH; j++)
                    {
                        slotMachine[i, j] = rng.Next(3);
                    }
                }

                // Print Select Gameplay Menu
                Console.WriteLine("\nSelect gameplay: ");
                Console.WriteLine("1. Center line");
                Console.WriteLine("2. All horizontal lines");
                Console.WriteLine("3. All vertical and diagonal lines\n");

                // Take Player Game Selection
                Console.Write("Play Game ");
                int userInputNumLines = int.Parse(Console.ReadLine());

                // Repeat Player Game Selection if selection is out of bound.
                while (userInputNumLines < GAME_1 || userInputNumLines > GAME_3)
                {
                    Console.Write("Play Game ");
                    userInputNumLines = int.Parse(Console.ReadLine());
                }

                // Take Player Wager
                Console.WriteLine($"\nPlayer money: {playerMoney:F2}");
                Console.Write($"How much is your wager (cannot exceed amount of player money): ");
                decimal userInputWager = decimal.Parse(Console.ReadLine());

                // Repeat Player Wager if wager is out of bound.
                while (userInputWager < 0 || userInputWager > playerMoney)
                {
                    Console.WriteLine($"\nPlayer money: {playerMoney:F2}");
                    Console.Write($"How much is your wager (cannot exceed amount of player money): ");
                    userInputWager = decimal.Parse(Console.ReadLine());
                }

                // Set Amounts
                playerMoney -= userInputWager;
                decimal potentialWinAmount = HALF_BONUS * (userInputWager) + 1;
                int numChecks = (userInputNumLines == GAME_3) ? 8 : 3;
                potentialWinAmount += (userInputNumLines == GAME_3) ? (userInputWager / 5) - 1: (userInputWager / 3) - 1;
                Console.WriteLine($"Potential Win Amount per line: {potentialWinAmount:F2}"); // DELETE CODE

                // Print Slot Machine Roll
                Console.WriteLine("\nSlot Machine Roll");
                for (int i = 0; i < ROW_COLUMN_LENGTH; i++)
                {
                    for (int j = 0; j < ROW_COLUMN_LENGTH; j++)
                    {
                        Console.Write($"{slotMachine[i, j]}  ");
                        if (j == NDX_2)
                        {
                            Console.Write("\n");
                        }
                    }
                }

                // Assign variables for calculations
                decimal winAmount = 0;
                int[] row_col_diag = { 0, 0, 0 };
                int ndxCounter = 0;

                // Check Lines
                while (ndxCounter < numChecks) {

                    bool Win = false;

                    // Check Horizontal Lines
                    if (ndxCounter < NUM_CHECKS_3)
                    {
                        for (int i = 0; i < ROW_COLUMN_LENGTH; i++)
                        {
                            // ERROR - Rework    
                            if (userInputNumLines == GAME_1 && ndxCounter != NDX_1) 
                            {
                                continue;
                            }

                            row_col_diag[i] = slotMachine[ndxCounter, i];
                        }
                    }

                    // Check Vertical Lines
                    if (ndxCounter > NUM_CHECKS_2 && ndxCounter < NUM_CHECKS_6) 
                    {
                        for (int i = 0; i < ROW_COLUMN_LENGTH; i++)
                        {
                            row_col_diag[i] = slotMachine[i, ndxCounter - 3];
                        }
                    }
                   

                    // Check Diagonal Lines
                    if (ndxCounter > NUM_CHECKS_5)
                    {
                        for (int i = 0; i < ROW_COLUMN_LENGTH; i++)
                        {
                            if (ndxCounter == 6)
                            {
                                row_col_diag[i] = slotMachine[i, i];
                            }
                            
                            if (ndxCounter == 7)
                            {
                                for (int j = 2; j >= 0; j--)
                                {
                                    row_col_diag[i] = slotMachine[i, j];
                                }
                            }
                        }
                    }

                    // Determine if row is a win.
                    int rowChecker = 0;
                    if (!(userInputNumLines == GAME_1 && ndxCounter != NDX_1))
                    {
                        for (int i = 1; i < ROW_COLUMN_LENGTH; i++)
                        {
                            if (row_col_diag[NDX_0] == row_col_diag[i])
                            {
                                rowChecker++;
                            }
                        }

                        if (rowChecker == 2)
                        {
                            winAmount += potentialWinAmount;
                            Win = true;
                        }
                    }

                    // DELETE IN FINAL CODE
                    if (ndxCounter == 0)
                    {
                        Console.Write("\n");
                    }
                    Console.Write($"Row {ndxCounter + NDX_1}: ");
                    foreach (int i in row_col_diag)
                    {
                        Console.Write($"{i} ");
                    }
                    Console.Write($"({Win}), Row Checker: {rowChecker}");
                    Console.Write("\n");
                    ndxCounter++;
                }

                // Determine if Player won or lost
                string game_results = LOST;
                if (winAmount > 0)
                {
                    game_results = WIN;
                }
                playerMoney += winAmount;

                // Print out Game Results Status
                Console.WriteLine($"\nPlayer {game_results}");
                Console.WriteLine($"Won: ${winAmount:F2}");
                Console.WriteLine($"Remaining: ${playerMoney:F2}\n");

                // 
                Console.WriteLine($"Press <Enter> to continue...");
                userInput = Console.ReadKey();

                while (userInput.Key != ConsoleKey.Enter)
                {
                    userInput = Console.ReadKey();
                }

                // Refresh Player's Money.
                if (playerMoney == 0) 
                {
                    Console.WriteLine("\nPlayer has lost all money. Refreshing game...\n");
                    playerMoney = 20.00M;
                }
            }
        }
    }
}
