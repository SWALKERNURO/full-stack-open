public static class ScopeExercises
{
    public static void Run(string[] args)
    {
        string exercise = args.FirstOrDefault() ?? "challenge";
        if (exercise == "inside")
        {
            bool flag = true;
            if (flag)
            {
                int value = 10;
                Console.WriteLine($"Inside the code block: {value}");
            }
        }
        else if (exercise == "initialized")
        {
            bool flag = true;
            int value = 0;
            if (flag) Console.WriteLine($"Inside the code block: {value}");
            value = 10;
            Console.WriteLine($"Outside the code block: {value}");
        }
        else if (exercise == "constant")
        {
            int value;
            if (true)
            {
                value = 10;
                Console.WriteLine($"Inside the code block: {value}");
            }
            Console.WriteLine($"Outside the code block: {value}");
        }
        else if (exercise == "styles")
        {
            bool flag = true;
            if (flag) { Console.WriteLine(flag); }
            if (flag)
                Console.WriteLine(flag);
            if (flag) Console.WriteLine(flag);
        }
        else if (exercise == "names")
        {
            string name = args.ElementAtOrDefault(1) ?? "steve";
            if (name == "bob")
                Console.WriteLine("Found Bob");
            else if (name == "steve")
                Console.WriteLine("Found Steve");
            else
                Console.WriteLine("Found Chuck");
        }
        else if (exercise == "challenge")
        {
            int[] numbers = args.Length > 1 ? args.Skip(1).Select(int.Parse).ToArray() : [4, 8, 15, 16, 23, 42];
            int total = 0;
            bool found = false;
            foreach (int number in numbers)
            {
                total += number;
                if (number == 42) found = true;
            }
            if (found) Console.WriteLine("Set contains 42");
            Console.WriteLine($"Total: {total}");
        }
        else throw new ArgumentException("Unknown scope exercise.");
    }
}
