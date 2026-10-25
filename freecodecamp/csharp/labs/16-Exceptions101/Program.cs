Console.OutputEncoding = System.Text.Encoding.UTF8;
if (args.FirstOrDefault()?.StartsWith("specific-", StringComparison.Ordinal) == true)
{
    SpecificExamples.Run(args[0]);
    return;
}
switch (args.FirstOrDefault() ?? "handled")
{
    case "unhandled":
        WriteMessage();
        Console.WriteLine("Exit program");
        break;
    case "handled":
        try
        {
            WriteMessage();
        }
        catch
        {
            Console.WriteLine("An exception has been caught");
        }
        Console.WriteLine("Exit program");
        break;
    case "stack":
        try
        {
            Process1();
        }
        catch
        {
            Console.WriteLine("An exception has occurred");
        }
        Console.WriteLine("Exit program");
        break;
}

static void Process1()
{
    try
    {
        WriteMessage();
    }
    catch
    {
        Console.WriteLine("Exception caught in Process1");
    }
}

static void WriteMessage()
{
    double float1 = 3000.0;
    double float2 = 0.0;
    int number1 = 3000;
    int number2 = 0;
    Console.WriteLine(float1 / float2);
    Console.WriteLine(number1 / number2);
}