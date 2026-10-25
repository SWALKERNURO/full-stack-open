internal static class CashChecks
{
    public static void Run()
    {
        Scenario("ten-dollar and one-dollar change", () => Success(new[] { 50, 20, 10, 5 }, 6, 1, 0, 0, 0, new[] { 46, 20, 9, 6 }));
        Scenario("five-dollar change updates the five-dollar count", () => Success(new[] { 50, 20, 10, 5 }, 15, 1, 0, 0, 0, new[] { 50, 19, 10, 6 }));
        Scenario("exact payment into an empty till", () => Success(new int[4], 20, 1, 0, 0, 0, new[] { 0, 0, 0, 1 }));
        Scenario("newly received bills are available for change", () => Success(new int[4], 20, 1, 0, 1, 0, new[] { 0, 0, 0, 1 }));
        Scenario("lower denomination fallback", () => Success(new[] { 0, 2, 0, 0 }, 10, 1, 0, 0, 0, new[] { 0, 0, 0, 1 }));
        Scenario("zero cost and zero payment", () => Success(new[] { 2, 3, 4, 5 }, 0, 0, 0, 0, 0, new[] { 2, 3, 4, 5 }));
        Scenario("single one-dollar payment", () => Success(new int[4], 1, 0, 0, 0, 1, new[] { 1, 0, 0, 0 }));
        Scenario("underpayment preserves every bill count", () => Failure<InvalidOperationException>(new[] { 50, 20, 10, 5 }, 42, 2, 0, 0, 0, "Not enough money"));
        Scenario("empty till failure is atomic", () => Failure<InvalidOperationException>(new int[4], 1, 1, 0, 0, 0, "unable to make change"));
        Scenario("failure after partial change is atomic", () => Failure<InvalidOperationException>(new[] { 0, 1, 1, 0 }, 3, 1, 0, 0, 0, "unable to make change"));
        Scenario("negative cost rejected without mutation", () => Failure<ArgumentOutOfRangeException>(new int[4], -1, 1, 0, 0, 0, "cost"));
        Scenario("negative payment rejected without mutation", () => Failure<ArgumentOutOfRangeException>(new int[4], 1, -1, 0, 0, 0, "negative"));
        Scenario("negative till count rejected without mutation", () => Failure<ArgumentOutOfRangeException>(new[] { -1, 0, 0, 0 }, 1, 1, 0, 0, 0, "negative"));
        Scenario("wrong till shape rejected", () => Failure<ArgumentException>(new[] { 1, 2 }, 1, 1, 0, 0, 0, "four denominations"));
        Scenario("null till rejected", () => Failure<ArgumentNullException>(null, 1, 1, 0, 0, 0, "cashTill"));
        Scenario("payment overflow preserves every bill count", () => Failure<OverflowException>(new int[4], 1, int.MaxValue, 0, 0, 0, "overflow"));
        Scenario("staged count overflow preserves every bill count", () => Failure<OverflowException>(new[] { int.MaxValue, 0, 0, 0 }, 1, 0, 0, 0, 1, "overflow"));
        Console.WriteLine("17 cash-register state scenarios passed");
    }

    static void Scenario(string label, Action operation)
    {
        TextWriter original = Console.Out;
        try { Console.SetOut(TextWriter.Null); operation(); }
        finally { Console.SetOut(original); }
        Console.WriteLine($"PASS {label}");
    }

    static void Success(int[] till, int cost, int twenties, int tens, int fives, int ones, int[] expected)
    {
        long initialTotal = Total(till);
        CashRegisterSimulation.MakeChange(cost, till, twenties, tens, fives, ones);
        Check(till.SequenceEqual(expected), "Unexpected denomination counts.");
        Check(till.All(count => count >= 0), "A bill count became negative.");
        Check(Total(till) == initialTotal + cost, "Successful transaction did not add exactly the item cost.");
    }

    static void Failure<T>(int[]? till, int cost, int twenties, int tens, int fives, int ones, string message) where T : Exception
    {
        int[]? before = till?.ToArray();
        bool caught = false;
        try { CashRegisterSimulation.MakeChange(cost, till!, twenties, tens, fives, ones); }
        catch (T ex)
        {
            Check(ex.Message.Contains(message, StringComparison.OrdinalIgnoreCase), "Unexpected exception message.");
            caught = true;
        }
        Check(caught, $"Expected {typeof(T).Name}.");
        if (before is not null) Check(till!.SequenceEqual(before), "Failed transaction mutated the till.");
    }

    static long Total(int[] till) => till[0] + till[1] * 5L + till[2] * 10L + till[3] * 20L;
    static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidDataException(message);
    }
}