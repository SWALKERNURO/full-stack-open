internal static class SpecificExamples
{
    public static void Run(string mode)
    {
        try
        {
            switch (mode)
            {
                case "specific-properties": InspectException(); break;
                case "specific-challenge": CatchChallenge.Run(); return;
                case "specific-divide":
                case "specific-overflow":
                case "specific-first": TypedCatch(mode); break;
                case "specific-separate": SeparateGuards(); break;
                case "specific-parse": ParseInputs(); break;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("An exception has occurred");
        }
        Console.WriteLine("Exit program");
    }

    private static void InspectException()
    {
        try
        {
            WriteDivision();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception caught in Process1: {ex.Message}");
            Console.WriteLine($"Type: {ex.GetType().FullName}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    private static void TypedCatch(string mode)
    {
        try
        {
            WriteMessage(mode);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Exception caught in Process1: {ex.Message}");
        }
    }

    private static void WriteMessage(string mode)
    {
        double float1 = 3000.0;
        double float2 = 0.0;
        int number1 = 3000;
        int number2 = 0;
        Console.WriteLine(float1 / float2);
        if (mode != "specific-overflow")
            Console.WriteLine(number1 / number2);
        if (mode != "specific-divide")
        {
            Console.WriteLine("Reached overflow conversion");
            checked
            {
                byte smallNumber = (byte)number1;
                Console.WriteLine(smallNumber);
            }
        }
    }

    private static void WriteDivision()
    {
        double float1 = 3000.0;
        double float2 = 0.0;
        int number1 = 3000;
        int number2 = 0;
        Console.WriteLine(float1 / float2);
        Console.WriteLine(number1 / number2);
    }

    private static void SeparateGuards()
    {
        int number1 = 3000;
        try
        {
            WriteDivision();
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Exception caught in WriteMessage: {ex.Message}");
        }
        checked
        {
            try
            {
                byte smallNumber = (byte)number1;
                Console.WriteLine(smallNumber);
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"Exception caught in WriteMessage: {ex.Message}");
            }
        }
    }

    private static void ParseInputs()
    {
        string[] inputValues = { "three", "9999999999", "0", "2" };
        foreach (string inputValue in inputValues)
        {
            int numValue = 0;
            try
            {
                numValue = int.Parse(inputValue);
                Console.WriteLine($"Parsed: {numValue}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid readResult. Please enter a valid number.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("The number you entered is too large or too small.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
