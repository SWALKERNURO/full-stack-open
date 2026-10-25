internal static class FirstMethods
{
    public static void Run(string[] args)
    {
        switch (args.FirstOrDefault())
        {
            case "syntax":
                Console.WriteLine("Before calling a method");
                SayHello();
                Console.WriteLine("After calling a method");
                int[] a = { 1, 2, 3, 4, 5 };
                PrintArray();
                void SayHello() { Console.WriteLine("Hello World!"); }
                void PrintArray() { foreach (int x in a) Console.Write($"{x} "); Console.WriteLine(); }
                break;
            case "random":
                Console.WriteLine("Generating random numbers:");
                DisplayRandomNumbers();
                void DisplayRandomNumbers()
                {
                    Random random = new Random();
                    for (int i = 0; i < 5; i++) Console.Write($"{random.Next(1, 100)} ");
                    Console.WriteLine();
                }
                break;
            case "times":
                int[] times = { 800, 1200, 1600, 2000 };
                int diff = 0;
                Console.WriteLine("Enter current GMT");
                int currentGMT = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Current Medicine Schedule:");
                DisplayTimes();
                Console.WriteLine("Enter new GMT");
                int newGMT = Convert.ToInt32(Console.ReadLine());
                if (Math.Abs(newGMT) > 12 || Math.Abs(currentGMT) > 12) Console.WriteLine("Invalid GMT");
                else
                {
                    // Signed offsets also handle travel westward and negative GMT zones.
                    diff = 100 * (newGMT - currentGMT);
                    AdjustTimes();
                }
                Console.WriteLine("New Medicine Schedule:");
                DisplayTimes();
                void DisplayTimes()
                {
                    foreach (int val in times)
                    {
                        string time = val.ToString();
                        int len = time.Length;
                        if (len >= 3) time = time.Insert(len - 2, ":");
                        else if (len == 2) time = time.Insert(0, "0:");
                        else time = time.Insert(0, "0:0");
                        Console.Write($"{time} ");
                    }
                    Console.WriteLine();
                }
                void AdjustTimes()
                {
                    for (int i = 0; i < times.Length; i++) times[i] = ((times[i] + diff) % 2400 + 2400) % 2400;
                }
                break;
            case "ipv4":
                string[] inputs = args.Length > 1 ? args.Skip(1).ToArray() : new[] { "107.31.1.5", "255.0.0.255", "555..0.555", "255...255" };
                string[] address = Array.Empty<string>();
                bool validLength = false, validZeroes = false, validRange = false;
                foreach (string ip in inputs)
                {
                    // Keep empty segments so extra dots cannot turn an invalid address into a valid one.
                    address = ip.Split('.');
                    ValidateLength();
                    ValidateZeroes();
                    ValidateRange();
                    Console.WriteLine($"{ip} is {(validLength && validZeroes && validRange ? "a valid" : "an invalid")} IPv4 address");
                }
                void ValidateLength() { validLength = address.Length == 4; }
                void ValidateZeroes()
                {
                    foreach (string number in address)
                    {
                        if (number.Length > 1 && number.StartsWith("0")) { validZeroes = false; return; }
                    }
                    validZeroes = true;
                }
                void ValidateRange()
                {
                    foreach (string number in address)
                    {
                        if (number.Length == 0 || number.Any(c => c < '0' || c > '9') || !int.TryParse(number, out int value) || value > 255)
                        { validRange = false; return; }
                    }
                    validRange = true;
                }
                break;
            case "fortune":
                Random generator = new Random();
                int luck = args.Length > 1 ? int.Parse(args[1]) : generator.Next(100);
                string[] text = { "You have much to", "Today is a day to", "Whatever work you do", "This is an ideal time to" };
                string[] good = { "look forward to.", "try new things!", "is likely to succeed.", "accomplish your dreams!" };
                string[] bad = { "fear.", "avoid major decisions.", "may have unexpected outcomes.", "re-evaluate your life." };
                string[] neutral = { "appreciate.", "enjoy time with friends.", "should align with your values.", "get in tune with nature." };
                TellFortune();
                void TellFortune()
                {
                    Console.WriteLine("A fortune teller whispers the following words:");
                    string[] fortune = luck > 75 ? good : luck < 25 ? bad : neutral;
                    for (int i = 0; i < 4; i++) Console.Write($"{text[i]} {fortune[i]} ");
                    Console.WriteLine();
                }
                break;
        }
    }
}
