internal static class WorkflowBaseline
{
    public static void Run()
    {
        string[][] userEnteredValues = { new[] { "1", "2", "3" }, new[] { "1", "two", "3" }, new[] { "0", "1", "2" } };
        string overallStatusMessage = Workflow1(userEnteredValues);
        if (overallStatusMessage == "operating procedure complete")
            Console.WriteLine("'Workflow1' completed successfully.");
        else
        {
            Console.WriteLine("An error occurred during 'Workflow1'.");
            Console.WriteLine(overallStatusMessage);
        }
    }

    static string Workflow1(string[][] userEnteredValues)
    {
        string operationStatusMessage = "good";
        foreach (string[] userEntries in userEnteredValues)
        {
            string processStatusMessage = Process1(userEntries);
            if (processStatusMessage == "process complete")
            {
                Console.WriteLine("'Process1' completed successfully.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("'Process1' encountered an issue, process aborted.");
                Console.WriteLine(processStatusMessage);
                Console.WriteLine();
                operationStatusMessage = processStatusMessage;
            }
        }
        return operationStatusMessage == "good" ? "operating procedure complete" : operationStatusMessage;
    }

    static string Process1(string[] userEntries)
    {
        string processStatus = "clean";
        string returnMessage = "";
        foreach (string userValue in userEntries)
        {
            if (int.TryParse(userValue, out int valueEntered))
            {
                if (valueEntered != 0) _ = checked(4 / valueEntered);
                else
                {
                    returnMessage = "Invalid data. User input values must be non-zero values.";
                    processStatus = "error";
                }
            }
            else
            {
                returnMessage = "Invalid data. User input values must be valid integers.";
                processStatus = "error";
            }
        }
        return processStatus == "clean" ? "process complete" : returnMessage;
    }
}