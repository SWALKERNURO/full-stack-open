// Set up the game. Optional scores make the exercise reproducible in checks.
Random random = new Random();
int roundsPlayed = args.Length > 0 ? args.Length : 8;
List<int> allScores = new List<int>();
Console.WriteLine($"Playing {roundsPlayed} rounds...\n");

// Simulate playing rounds and log every score.
for (int round = 1; round <= roundsPlayed; round++)
{
    int score = args.Length > 0 ? int.Parse(args[round - 1]) : random.Next(0, 101);
    allScores.Add(score);
    Console.WriteLine($"Round {round}: scored {score} points");
}
Console.WriteLine($"\nTotal rounds logged: {allScores.Count}");

// Build a fixed five-slot array, safely handling fewer than five rounds.
allScores.Sort();
allScores.Reverse();
int[] topScores = new int[5];
for (int i = 0; i < topScores.Length && i < allScores.Count; i++)
    topScores[i] = allScores[i];

// Print the leaderboard.
Console.WriteLine("\nTop 5 Leaderboard");
for (int i = 0; i < topScores.Length; i++)
    Console.WriteLine($"{i + 1}. {topScores[i]} points");
