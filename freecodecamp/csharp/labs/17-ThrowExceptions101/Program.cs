using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
if (args.FirstOrDefault() == "baseline") WorkflowBaseline.Run();
else if (args.FirstOrDefault()?.StartsWith("workflow", StringComparison.Ordinal) == true) WorkflowChallenge.Run(args[0]);
else RunAverage();

static void RunAverage()
{
    int? lowerInput = ReadInteger("Enter the lower bound: ");
    if (lowerInput is null) return;
    int lowerBound = lowerInput.Value;
    int? upperInput = ReadInteger("Enter the upper bound: ");
    if (upperInput is null) return;
    int upperBound = upperInput.Value;
    bool exit = false;
    do
    {
        try
        {
            decimal averageValue = AverageOfEvenNumbers(lowerBound, upperBound);
            Console.WriteLine($"The average of even numbers between {lowerBound} and {upperBound} is {averageValue}.");
            exit = true;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine("An error has occurred.");
            Console.WriteLine(ex.Message);
            Console.WriteLine($"The upper bound must be greater than {lowerBound}");
            int? retry = ReadInteger("Enter a new upper bound (or enter Exit to quit): ");
            if (retry is null) exit = true;
            else upperBound = retry.Value;
        }
    } while (!exit);
    Console.ReadLine();
}

static int? ReadInteger(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? response = Console.ReadLine();
        if (response is null || response.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
            return null;
        if (int.TryParse(response, out int value)) return value;
        Console.WriteLine("Please enter a valid integer.");
    }
}

static decimal AverageOfEvenNumbers(int lowerBound, int upperBound)
{
    if (lowerBound >= upperBound)
        throw new ArgumentOutOfRangeException(nameof(upperBound), "ArgumentOutOfRangeException: upper bound must be greater than lower bound.");

    // Consecutive even integers form an arithmetic progression. Using endpoints
    // gives the same average without overflowing a sum or looping billions of times.
    long firstEven = lowerBound % 2 == 0 ? lowerBound : (long)lowerBound + 1;
    long lastEven = upperBound % 2 == 0 ? upperBound : (long)upperBound - 1;
    return (firstEven + lastEven) / 2m;
}