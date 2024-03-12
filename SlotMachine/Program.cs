using System;

namespace SlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int ROW_COLUMN_LENGTH = 3, NDX_0 = 0,  NDX_1 = 1, NDX_2 = 2, NDX_3 = 3, GAME_1 = 1, GAME_2 = 2, GAME_3 = 3, GAME_4 = 4, GAME_5 = 5;
            const int A_DOLLAR = 1, NUM_CHECKS_2 = 2, NUM_CHECKS_3 = 3, NUM_CHECKS_5 = 5, NUM_CHECKS_6 = 6, NUM_CHECKS_7 = 7, NUM_CHECKS_8 = 8;
            const decimal A_CENT = 0.01M, HALF_BONUS = 0.50M, PLAYER_MONEY_RESET = 20.00M;
            const string WIN = "WIN!", LOST = "LOST!";
            decimal playerMoney = 20.00M;
            Random rng = new Random();
            ConsoleKeyInfo userInput;
            
            Console.WriteLine("Welcome to Slot Machine!");

            while (true)
            {
                // Get Random Numbers for Slot Machine
                int[,] slotMachine = new int[NDX_3,NDX_3];
                for (int i = 0; i < ROW_COLUMN_LENGTH; i++)
                {
                    for (int j = 0; j < ROW_COLUMN_LENGTH; j++)
                    {
                        slotMachine[i, j] = rng.Next(NDX_3);
                    }
                }

                // Print Select Gameplay Menu
                Console.WriteLine("\nSelect gameplay: ");
                Console.WriteLine("1. Center line");
                Console.WriteLine("2. All horizontal lines");
                Console.WriteLine("3. All vertical lines");
                Console.WriteLine("4. All diagonal lines");
                Console.WriteLine("5. All lines (horizontal, vertical, and diagonal)\n");

                int number;
                int userInputGameSelection = 0;

                // Take Player Game Selection
                Console.Write("Play Game ");
                if (int.TryParse(Console.ReadLine(), out number))
                {
                    userInputGameSelection = number;
                }

                // Repeat Player Game Selection if selection is out of bound.
                while (userInputGameSelection < GAME_1 || userInputGameSelection > GAME_5)
                {
                    Console.Write("Play Game ");
                    if (int.TryParse(Console.ReadLine(), out number))
                    {
                        userInputGameSelection = number;
                    }
                }

                decimal amount;
                decimal userInputWager = 0;
                // Take Player Wager
                Console.WriteLine($"\nPlayer money: {playerMoney:F2}");
                Console.Write($"How much is your wager (cannot exceed amount of player money): ");
                
                if (decimal.TryParse(Console.ReadLine(), out amount)) 
                {
                    userInputWager = amount;
                }

                // Repeat Player Wager if wager is out of bound.
                while (userInputWager < A_CENT || userInputWager > playerMoney)
                {
                    Console.WriteLine($"\nPlayer money: {playerMoney:F2}");
                    Console.Write($"How much is your wager (cannot exceed amount of player money): ");
                    if (decimal.TryParse(Console.ReadLine(), out amount))
                    {
                        userInputWager = amount;
                    }
                }
                
                // Set Amounts
                playerMoney -= userInputWager;
                int ndxCounter = 0;
                decimal potentialWinAmount = (userInputGameSelection == GAME_1 ? (userInputWager + A_DOLLAR) : ((HALF_BONUS * userInputWager) + A_DOLLAR));
                int numChecks = (userInputGameSelection == GAME_5 ? NUM_CHECKS_8 : NUM_CHECKS_3);
                if (userInputGameSelection == GAME_3) 
                {
                    numChecks = NUM_CHECKS_6;
                    ndxCounter = NUM_CHECKS_3;
                }
                if (userInputGameSelection == GAME_4) 
                {
                    numChecks = NUM_CHECKS_8;
                    ndxCounter = NUM_CHECKS_6;

                }
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

                // Check Lines
                while (ndxCounter < numChecks) {

                    bool Win = false;

                    // Check Horizontal Lines
                    if (ndxCounter < NUM_CHECKS_3)
                    {
                        for (int i = 0; i < ROW_COLUMN_LENGTH; i++)
                        { 
                            if (userInputGameSelection == GAME_1 && ndxCounter != NDX_1) 
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
                            row_col_diag[i] = slotMachine[i, ndxCounter - NDX_3];
                        }
                    }

                    // Check Diagonal Lines
                    if (ndxCounter > NUM_CHECKS_5)
                    {
                        int j = NDX_2;
                        for (int i = 0; i < ROW_COLUMN_LENGTH; i++)
                        {
                            if (ndxCounter == NUM_CHECKS_6)
                            {
                                row_col_diag[i] = slotMachine[i, i];
                            }
                            
                            if (ndxCounter == NUM_CHECKS_7)
                            {
                                row_col_diag[i] = slotMachine[i, j];
                                j--;
                            }
                        }
                    }

                    // Determine if row is a win. //
                    int rowChecker = 0;
                    if (!(userInputGameSelection == GAME_1 && ndxCounter != NDX_1))
                    {
                        for (int i = NDX_1; i < ROW_COLUMN_LENGTH; i++)
                        {
                            if (row_col_diag[NDX_0] == row_col_diag[i])
                            {
                                rowChecker++;
                            }
                        }

                        if (rowChecker == NUM_CHECKS_2)
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
                    playerMoney = PLAYER_MONEY_RESET;
                }
            }
        }
    }
}
