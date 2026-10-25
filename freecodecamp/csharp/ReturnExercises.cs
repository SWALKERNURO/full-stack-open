internal static class ReturnExercises
{
    public static void Run(string[] args)
    {
        switch (args.FirstOrDefault())
        {
            case "checkout":
                double total = 0, minimumSpend = 30.00;
                double[] items = { 15.97, 3.50, 12.25, 22.99, 10.98 };
                double[] discounts = { .30, .00, .10, .20, .50 };
                for (int i = 0; i < items.Length; i++) total += GetDiscountedPrice(i);
                total -= TotalMeetsMinimum() ? 5.00 : 0.00;
                Console.WriteLine($"Total: ${FormatDecimal(total)}");
                double GetDiscountedPrice(int itemIndex) { return items[itemIndex] * (1 - discounts[itemIndex]); }
                bool TotalMeetsMinimum() { return total >= minimumSpend; }
                // This lesson demonstrates a string return using the supplied five-character truncation.
                string FormatDecimal(double input) { return input.ToString().Substring(0, 5); }
                break;
            case "currency":
                double usd = 23.73;
                int vnd = UsdToVnd(usd);
                Console.WriteLine($"${usd} USD = ${vnd} VND");
                Console.WriteLine($"${vnd} VND = ${VndToUsd(vnd)} USD");
                int UsdToVnd(double usd) { int rate = 23500; return (int)(rate * usd); }
                double VndToUsd(int vnd) { double rate = 23500; return vnd / rate; }
                break;
            case "words":
                Console.WriteLine("snake");
                Console.WriteLine(ReverseWord("snake"));
                string input = args.Length > 1 ? string.Join(" ", args.Skip(1)) : "there are snakes at the zoo";
                Console.WriteLine(input);
                Console.WriteLine(ReverseSentence(input));
                string ReverseWord(string word)
                {
                    string result = "";
                    for (int i = word.Length - 1; i >= 0; i--) result += word[i];
                    return result;
                }
                string ReverseSentence(string sentence)
                {
                    string result = "";
                    foreach (string word in sentence.Split(' ')) result += ReverseWord(word) + " ";
                    return result.Trim();
                }
                break;
            case "palindrome":
                string[] words = args.Length > 1 ? args.Skip(1).ToArray() : new[] { "racecar", "talented", "deified", "tent", "tenet" };
                Console.WriteLine("Is it a palindrome?");
                foreach (string word in words) Console.WriteLine($"{word}: {IsPalindrome(word)}");
                bool IsPalindrome(string word)
                {
                    int start = 0, end = word.Length - 1;
                    while (start < end)
                    {
                        if (word[start] != word[end]) return false;
                        start++;
                        end--;
                    }
                    return true;
                }
                break;
            case "coins-one":
                int[] firstCoins = { 5, 5, 50, 25, 25, 10, 5 };
                int firstTarget = int.Parse(args.ElementAtOrDefault(1) ?? "60");
                int[] pair = TwoCoinsFirst(firstCoins, firstTarget);
                Console.WriteLine(pair.Length == 0 ? "No two coins make change" : $"Change found at positions {pair[0]} and {pair[1]}");
                break;
            case "coins":
                int target = int.Parse(args.ElementAtOrDefault(1) ?? "30");
                int[] coins = args.Length > 2 ? args.Skip(2).Select(int.Parse).ToArray() : new[] { 5, 5, 50, 25, 25, 10, 5 };
                int[,] pairs = TwoCoins(coins, target);
                if (pairs.Length == 0) Console.WriteLine("No two coins make change");
                else
                {
                    Console.WriteLine("Change found at positions:");
                    for (int i = 0; i < pairs.GetLength(0); i++)
                    {
                        if (pairs[i, 0] == -1) break;
                        Console.WriteLine($"{pairs[i, 0]},{pairs[i, 1]}");
                    }
                }
                break;
            case "game":
                Random random = new Random();
                Queue<int> supplied = new Queue<int>(args.Skip(1).Select(int.Parse));
                Console.WriteLine("Would you like to play? (Y/N)");
                if (ShouldPlay()) PlayGame();
                bool ShouldPlay() { return Console.ReadLine()?.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase) == true; }
                int GetTarget() { return supplied.Count > 0 ? supplied.Dequeue() : random.Next(1, 6); }
                int RollDice() { return supplied.Count > 0 ? supplied.Dequeue() : random.Next(1, 7); }
                string WinOrLose(int roll, int target) { return roll > target ? "You win!" : "You lose!"; }
                void PlayGame()
                {
                    bool play = true;
                    while (play)
                    {
                        int target = GetTarget(), roll = RollDice();
                        Console.WriteLine($"Roll a number greater than {target} to win!");
                        Console.WriteLine($"You rolled a {roll}");
                        Console.WriteLine(WinOrLose(roll, target));
                        Console.WriteLine("\nPlay again? (Y/N)");
                        play = ShouldPlay();
                    }
                }
                break;
        }
    }

    private static int[] TwoCoinsFirst(int[] coins, int target)
    {
        for (int curr = 0; curr < coins.Length; curr++)
            for (int next = curr + 1; next < coins.Length; next++)
                if (coins[curr] + coins[next] == target) return new[] { curr, next };
        return Array.Empty<int>();
    }

    private static int[,] TwoCoins(int[] coins, int target)
    {
        int[,] result = { { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 } };
        int count = 0;
        for (int curr = 0; curr < coins.Length; curr++)
        {
            for (int next = curr + 1; next < coins.Length; next++)
            {
                if (coins[curr] + coins[next] == target)
                {
                    result[count, 0] = curr;
                    result[count++, 1] = next;
                }
                if (count == result.GetLength(0)) return result;
            }
        }
        return count == 0 ? new int[0, 0] : result;
    }
}
