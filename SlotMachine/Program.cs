using System;

namespace SlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int LINE_2 = 1, LINE_3 = 2, LINE_6 = 5, GAME_1 = 1, GAME_3 = 3, GAME_4 = 4, GAME_5 = 5;
            const int LINE_LENGTH = 3, MAX_VALUE = 3, NUM_LINE_CHECKS_2 = 2, NUM_LINE_CHECKS_3 = 3;
            const decimal A_CENT = 0.01M, HALF_BONUS = 0.50M, A_DOLLAR = 1.00M, PLAYER_MONEY_RESET = 20.00M;
            const string WIN = "WIN!", LOST = "LOST!";
            decimal playerMoney = 20.00M;
            Random rng = new Random();
            ConsoleKeyInfo userInput;
            
            Console.WriteLine("Welcome to Slot Machine!");

            while (true)
            {
                // Get Random Numbers for Slot Machine ./
                int[,] slotMachine = new int[LINE_LENGTH, LINE_LENGTH];
                for (int i = 0; i < LINE_LENGTH; i++)
                {
                    for (int j = 0; j < LINE_LENGTH; j++)
                    {
                        slotMachine[i, j] = rng.Next(MAX_VALUE); //
                    }
                }

                // Print Select Gameplay Menu
                Console.WriteLine("\nSelect gameplay: ");
                Console.WriteLine("1. Center line");
                Console.WriteLine("2. All horizontal lines");
                Console.WriteLine("3. All vertical lines");
                Console.WriteLine("4. All diagonal lines");
                Console.WriteLine("5. All lines (horizontal, vertical, and diagonal)\n");

                // Set Selection
                int userNumber;
                int userInputGameSelection = 0;

                // Take Player Game Selection
                Console.Write("Play Game ");
                if (int.TryParse(Console.ReadLine(), out userNumber))
                {
                    userInputGameSelection = userNumber;
                }

                // Repeat Player Game Selection if selection is out of bound.
                while (userInputGameSelection < GAME_1 || userInputGameSelection > GAME_5)
                {
                    Console.Write("Play Game ");
                    if (int.TryParse(Console.ReadLine(), out userNumber))
                    {
                        userInputGameSelection = userNumber;
                    }
                }

                // Set Amounts
                decimal userAmount;
                decimal userInputWager = 0;

                // Take Player Wager
                Console.WriteLine($"\nPlayer money: {playerMoney:F2}");
                Console.Write($"How much is your wager (cannot exceed amount of player money): ");

                if (decimal.TryParse(Console.ReadLine(), out userAmount))
                {
                    userInputWager = userAmount;
                }

                // Repeat Player Wager if wager is out of bound.
                while (userInputWager < A_CENT || userInputWager > playerMoney)
                {
                    Console.WriteLine($"\nPlayer money: {playerMoney:F2}");
                    Console.Write($"How much is your wager (cannot exceed amount of player money): ");
                    if (decimal.TryParse(Console.ReadLine(), out userAmount))
                    {
                        userInputWager = userAmount;
                    }
                }

                // Print Slot Machine Roll
                Console.WriteLine("\nSlot Machine Roll");
                for (int i = 0; i < LINE_LENGTH; i++)
                {
                    for (int j = 0; j < LINE_LENGTH; j++)
                    {
                        Console.Write($"{slotMachine[i, j]}  ");
                        if (j == LINE_LENGTH - 1) 
                        {
                            Console.Write("\n");
                        }
                    }
                }

                // Set Amuounts and Variables for calculations
                playerMoney -= userInputWager;
                decimal potentialWinAmount = (userInputGameSelection == GAME_1 ? (userInputWager + A_DOLLAR) : ((HALF_BONUS * userInputWager) + A_DOLLAR));
                decimal winAmount = 0;
                int[] aSingleLine = { 0, 1, 2 };

                // Determine where line starts and how many lines should be checked based on game selection
                int lineNumber = (userInputGameSelection == GAME_4 ? NUM_LINE_CHECKS_3 + NUM_LINE_CHECKS_3 : 0);
                int numLineChecks = (userInputGameSelection >= GAME_4 ? NUM_LINE_CHECKS_3 + NUM_LINE_CHECKS_3 + NUM_LINE_CHECKS_2 : 3);
                if (userInputGameSelection == GAME_3) 
                {
                    lineNumber = NUM_LINE_CHECKS_3;
                    numLineChecks = NUM_LINE_CHECKS_3 + NUM_LINE_CHECKS_3;
                }

                // DELETE IN FINAL CODE
                Console.WriteLine($"\nPotential Win Amount per line: {potentialWinAmount:F2}");

                // Check specific lines based on user inputted game selection
                while (lineNumber < numLineChecks) 
                {

                    // HORIZONTAL LINES - Obtain first three lines for games 1, 2, or 5, skip if game is 3 or 4
                    if (lineNumber <= LINE_3) 
                    {
                        // Check Horizontal Lines
                        for (int i = 0; i < LINE_LENGTH; i++)
                        {
                            aSingleLine[i] = slotMachine[lineNumber,i];
                        }
                    }

                    // VERTICAL LINES - Obtain second three lines for game 3 or 5, skip if game is 1, 2, or 4
                    if (lineNumber > LINE_3 && lineNumber <= LINE_6) 
                    { 
                        //Check Vertical Lines
                        for (int i = 0; i < LINE_LENGTH; i++) 
                        {
                            // lineNumber % NUM_LINE_CHECKS_3 ensures lineNumber is within line index of slot machine
                            aSingleLine[i] = slotMachine[i, lineNumber % NUM_LINE_CHECKS_3];
                        }
                    }

                    // Check Diagonal Lines - Skip if user inputted game selection is 1, 2, or 3
                    if (lineNumber > LINE_6) 
                    {
                        int j = LINE_3; 

                        for (int i = 0; i < LINE_LENGTH; i++)
                        {
                            // lineNumber % (NUM_LINE_CHECKS_3 + NUM_LINE_CHECKS_2) ensures modifiedLineNumber is within line index of slot machine
                            int modifiedLineNumber = lineNumber % (NUM_LINE_CHECKS_3 + NUM_LINE_CHECKS_3);

                            // Modified line number 0
                            if (modifiedLineNumber != LINE_2)
                            {
                                aSingleLine[i] = slotMachine[i, i];
                            }

                            // Modified Line number 1
                            if (modifiedLineNumber == LINE_2)
                            {
                                aSingleLine[i] = slotMachine[i, j];
                                j--;
                            }
                        }
                    }

                    // Set values for determining if a single line is a winner
                    int lineChecker = 0;
                    bool Win = false;

                    // Determine if the line has the same three numbers
                    if (!(userInputGameSelection == GAME_1 && lineNumber != LINE_2))
                    {
                        for (int i = 0; i < LINE_LENGTH; i++)//
                        {
                            if (aSingleLine[0] == aSingleLine[i])
                            {
                                lineChecker++;
                            }
                        }
                        if (lineChecker == LINE_LENGTH)
                        {
                            winAmount += potentialWinAmount;
                            Win = true;
                        }
                    }

                    // DELETE IN FINAL CODE
                    string counted = "";
                    if (userInputGameSelection == GAME_1 && lineNumber!= LINE_2)
                    {
                        counted = "(Not Tallied)";
                    }   
                    if (lineNumber == 0)
                    {
                        Console.Write("\n");
                    }
                    Console.Write($"Line {lineNumber + 1}: ");
                    foreach (int i in aSingleLine)
                    {
                        Console.Write($"{i} ");
                    }
                    Console.Write($"({Win}), Line Checker: {lineChecker} {counted}");
                    Console.Write("\n");

                    // Increment LineNumber
                    lineNumber++;
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
