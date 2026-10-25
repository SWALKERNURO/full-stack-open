internal static class ForExercises
{
    public static void Run(string? mode)
    {
        switch (mode)
        {
            case "up":
                for (int i = 0; i < 10; i++) Console.WriteLine(i);
                break;
            case "down":
                for (int i = 10; i >= 0; i--) Console.WriteLine(i);
                break;
            case "step":
                for (int i = 0; i < 10; i += 3) Console.WriteLine(i);
                break;
            case "break":
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                    if (i == 7) break;
                }
                break;
            case "reverse":
                string[] reversedNames = { "Alex", "Eddie", "David", "Michael" };
                for (int i = reversedNames.Length - 1; i >= 0; i--) Console.WriteLine(reversedNames[i]);
                break;
            case "rename":
                string[] names = { "Alex", "Eddie", "David", "Michael" };
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == "David") names[i] = "Sammy";
                }
                foreach (string name in names) Console.WriteLine(name);
                break;
            default:
                for (int i = 1; i <= 100; i++)
                {
                    if (i % 3 == 0 && i % 5 == 0) Console.WriteLine($"{i} - FizzBuzz");
                    else if (i % 3 == 0) Console.WriteLine($"{i} - Fizz");
                    else if (i % 5 == 0) Console.WriteLine($"{i} - Buzz");
                    else Console.WriteLine(i);
                }
                break;
        }
    }
}
