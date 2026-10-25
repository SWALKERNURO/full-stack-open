// Set up the game. An optional argument makes branch checks reproducible.
Random random = new Random();
int secretNumber = args.Length > 0 ? int.Parse(args[0]) : random.Next(1, 21);
int maxGuesses = 5;
int guessCount = 0;
bool hasWon = false;
Console.WriteLine("I'm thinking of a number between 1 and 20.");
Console.WriteLine($"You have {maxGuesses} guesses. Good luck!");

// Ask the player to guess. Invalid input does not consume a guess.
while (guessCount < maxGuesses)
{
    Console.Write($"\nGuess #{guessCount + 1}: ");
    string? input = Console.ReadLine();
    if (input is null) break;
    if (!int.TryParse(input, out int guess) || guess < 1 || guess > 20)
    {
        Console.WriteLine("Please enter a whole number between 1 and 20.");
        continue;
    }
    guessCount++;

    // Check the guess and give feedback.
    if (guess == secretNumber)
    {
        Console.WriteLine($"Correct! You got it in {guessCount} guesses.");
        hasWon = true;
        break;
    }
    else if (guess < secretNumber) Console.WriteLine("Too low.");
    else Console.WriteLine("Too high.");
    if (Math.Abs(guess - secretNumber) <= 2) Console.WriteLine("You're very close!");
}

// Announce the result.
if (!hasWon) Console.WriteLine($"\nOut of guesses! The number was {secretNumber}.");
