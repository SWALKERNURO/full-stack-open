internal static class ConversionExercises
{
    public static void Run(string[] args)
    {
        switch (args.FirstOrDefault())
        {
            case "demos":
                int first = 2;
                string second = "4";
                Console.WriteLine($"Concatenation: {first + second}");
                int myInt = 3;
                decimal widened = myInt;
                Console.WriteLine($"int: {myInt}\ndecimal: {widened}");
                decimal narrowed = 3.14m;
                Console.WriteLine($"decimal: {narrowed}\nint: {(int)narrowed}");
                decimal precise = 1.23456789m;
                float approximate = (float)precise;
                Console.WriteLine($"Decimal: {precise}\nFloat  : {approximate}");
                Console.WriteLine($"ToString: {5.ToString() + 7.ToString()}");
                Console.WriteLine($"Parsed sum: {int.Parse("5") + int.Parse("7")}");
                Console.WriteLine($"Converted product: {Convert.ToInt32("5") * Convert.ToInt32("7")}");
                Console.WriteLine($"Truncated: {(int)1.5m}\nRounded: {Convert.ToInt32(1.5m)}");
                break;
            case "round":
                decimal number = decimal.Parse(args[1]);
                Console.WriteLine($"Truncated: {(int)number}\nRounded: {Convert.ToInt32(number)}");
                break;
            case "parse":
                string value = args.ElementAtOrDefault(1) ?? "102";
                int result = 0;
                if (int.TryParse(value, out result)) Console.WriteLine($"Measurement: {result}");
                else Console.WriteLine("Unable to report the measurement.");
                if (result > 0) Console.WriteLine($"Measurement (w/ offset): {50 + result}");
                break;
            case "format-error":
                try { Console.WriteLine(int.Parse("Bob")); }
                catch (FormatException) { Console.WriteLine("Expected FormatException when parsing Bob."); }
                break;
            case "mixed":
                string[] values = args.Length > 1 ? args.Skip(1).ToArray() : new[] { "12.3", "45", "ABC", "11", "DEF" };
                decimal total = 0m;
                string message = "";
                foreach (string item in values)
                {
                    if (decimal.TryParse(item, out decimal parsed)) total += parsed;
                    else message += item;
                }
                Console.WriteLine($"Message: {message}\nTotal: {total}");
                break;
            case "math":
                int value1 = 11;
                decimal value2 = 6.2m;
                float value3 = 4.3f;
                int result1 = Convert.ToInt32(value1 / value2);
                decimal result2 = value2 / (decimal)value3;
                float result3 = value3 / value1;
                Console.WriteLine($"Divide value1 by value2, display the result as an int: {result1}");
                Console.WriteLine($"Divide value2 by value3, display the result as a decimal: {result2}");
                Console.WriteLine($"Divide value3 by value1, display the result as a float: {result3}");
                break;
        }
    }
}
