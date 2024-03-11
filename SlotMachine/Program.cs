using System;

namespace SlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int NDX_0 = 0, GAME_1 = 1, NDX_1 = 1, GAME_2 = 2, NDX_2 = 2, GAME_3 = 3, ROW_COLUMN_LENGTH = 3;
            const double HALF_BONUS = 0.50;
            const string WIN = "WIN!", LOST = "LOST!";
            double playerMoney = 20.00;
            Random rng = new Random();
            ConsoleKeyInfo userInput;
            
            Console.WriteLine("Welcome to Slot Machine!");

            while (true)
            {
                // Get Random Numbers for Slot Machine
                int A0 = rng.Next(3), A1 = rng.Next(3), A2 = rng.Next(3); // First slot line
                int B0 = rng.Next(3), B1 = rng.Next(3), B2 = rng.Next(3); // Second slot line
                int C0 = rng.Next(3), C1 = rng.Next(3), C2 = rng.Next(3); // Third slot line
                int[,] slotMachine = { { A0, A1, A2 }, { B0, B1, B2 }, { C0, C1, C2 } };

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
                double userInputWager = double.Parse(Console.ReadLine());

                // Repeat Player Wager if wager is out of bound.
                while (userInputWager < 0 || userInputWager > playerMoney)
                { 
                    Console.WriteLine($"\nPlayer money: {playerMoney:F2}");
                    Console.Write($"How much is your wager (cannot exceed amount of player money): ");
                    userInputWager = double.Parse(Console.ReadLine());
                }

                // Set Money Amounts
                playerMoney -= userInputWager;
                double potentialWinAmount = HALF_BONUS * (userInputWager) + 1;
                int numOfChecks = 1;

                if (userInputNumLines == GAME_2)
                {
                    potentialWinAmount = HALF_BONUS * userInputWager + userInputWager / 3;
                    numOfChecks = 3;
                }

                if (userInputNumLines == GAME_3)
                {
                    potentialWinAmount = HALF_BONUS * userInputWager + userInputWager / 5;
                    numOfChecks = 8;
                }

                // Print Slot Machine Roll
                Console.WriteLine("\nSlot Machine Roll");
                for (int i =  0; i < ROW_COLUMN_LENGTH; i++) 
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
                double winAmount = 0;
                int[] row_col_diag = {0, 0, 0};
                int counter = 0;

                // Check all lines
                while (counter < numOfChecks) 
                {  
                    bool Win = false;

                    for (int i = 0; i < ROW_COLUMN_LENGTH; i++) 
                    {
                        // Center horizontal line
                        if (counter == NDX_0)
                        {
                            row_col_diag[i] = slotMachine[NDX_1, i];
                        }

                        // Top horizontal line
                        if (counter == NDX_1)
                        {
                            row_col_diag[i] = slotMachine[NDX_0, i];
                        }

                        // Bottom horizontal line
                        if (counter == NDX_2)
                        {
                            row_col_diag[i] = slotMachine[NDX_2, i];
                        }

                        // Left vertical line
                        if (counter == NDX_2 + NDX_1)
                        {
                            row_col_diag[i] = slotMachine[i, NDX_0];
                        }

                        // Center vertical line
                        if (counter == NDX_2 * NDX_2)
                        {
                            row_col_diag[i] = slotMachine[i, NDX_1];
                        }

                        // Right vertical line
                        if (counter == NDX_2 * NDX_2 + NDX_1)
                        {
                            row_col_diag[i] = slotMachine[i, NDX_2];
                        }

                        // Top left to bottom right diagonal line
                        if (counter == NDX_2 * NDX_2 + NDX_2)
                        {
                            row_col_diag[i] = slotMachine[i, i];
                        }
                        
                        // Bottom left to top right diagonal line
                        if (counter == NDX_2 * NDX_2 * NDX_2 - NDX_1)
                        {
                            int j = 0;
                            if (i == NDX_0)
                            {
                                j = NDX_2; 
                            }

                            if (i == NDX_1) 
                            {
                                j = NDX_1;
                            }

                            if (i == NDX_2)
                            {
                                j = NDX_0;
                            }

                            row_col_diag[i] = slotMachine[i, j];
                        } 
                    }

                    // (Array.TrueForAll(row, ele => ele.Equals(row[0])) {}
                    if (row_col_diag[NDX_0] == row_col_diag[NDX_1] && row_col_diag[NDX_1] == row_col_diag[NDX_2])
                    {
                        winAmount += potentialWinAmount;
                        Win = true;
                    }

                    // DELETE IN FINAL CODE
                    if (counter == 0)
                    {
                        Console.Write("\n");
                    }
                    Console.Write($"Row {counter + NDX_1}: ");
                    foreach (int i in row_col_diag)
                    {
                        Console.Write($"{i} ");
                    }
                    Console.Write($"{Win}");
                    Console.Write("\n");
                    counter++;           
                }

                // Determine if Player won or lost
                string game_results = LOST;
                if (winAmount > 0)
                {
                    game_results = WIN;
                }
                playerMoney += winAmount;

                // Print out Game Results Status
                Console.WriteLine($"\n{game_results}");
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
                    playerMoney = 20.00;
                }
            }
        }
    }
}
