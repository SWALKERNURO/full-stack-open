internal static class WorkflowChallenge
{
    public static void Run(string mode)
    {
        string[][] userEnteredValues = mode switch
        {
            "workflow-valid" => new[] { new[] { "1", "2", "3" }, new[] { "-1", "4", "8" } },
            "workflow-format" => new[] { new[] { "two" }, new[] { "1", "2", "3" } },
            "workflow-zero-first" => new[] { new[] { "0" }, new[] { "1", "2", "3" } },
            _ => new[] { new[] { "1", "2", "3" }, new[] { "1", "two", "3" }, new[] { "0", "1", "2" } }
        };
        try
        {
            Workflow1(userEnteredValues);
            Console.WriteLine("'Workflow1' completed successfully.");
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine("An error occurred during 'Workflow1'.");
            Console.WriteLine(ex.Message);
        }
    }

    static void Workflow1(string[][] userEnteredValues)
    {
        foreach (string[] userEntries in userEnteredValues)
        {
            try
            {
                Process1(userEntries);
                Console.WriteLine("'Process1' completed successfully.");
                Console.WriteLine();
            }
            catch (FormatException ex)
            {
                Console.WriteLine("'Process1' encountered an issue, process aborted.");
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
        }
    }

    static void Process1(string[] userEntries)
    {
        foreach (string userValue in userEntries)
        {
            if (!int.TryParse(userValue, out int valueEntered))
                throw new FormatException("Invalid data. User input values must be valid integers.");
            if (valueEntered == 0)
                throw new DivideByZeroException("Invalid data. User input values must be non-zero values.");
            _ = checked(4 / valueEntered);
        }
    }
}