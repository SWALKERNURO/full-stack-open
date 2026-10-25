internal static class WhileExercises
{
    public static void Run(string[] args)
    {
        string mode = args.FirstOrDefault() ?? "do";
        Random random = new Random();
        Queue<int> supplied = new Queue<int>(args.Skip(1).Select(int.Parse));
        int Next() => supplied.Count > 0 ? supplied.Dequeue() : random.Next(1, 11);
        switch (mode)
        {
            case "do":
            case "continue":
                int current;
                do
                {
                    current = Next();
                    if (mode == "continue" && current >= 8) continue;
                    Console.WriteLine(current);
                } while (current != 7);
                break;
            case "pretest":
                int value = Next();
                while (value >= 3)
                {
                    Console.WriteLine(value);
                    value = Next();
                }
                Console.WriteLine($"Last number: {value}");
                break;
            case "battle":
                int hero = 10;
                int monster = 10;
                do
                {
                    int roll = Next();
                    monster -= roll;
                    Console.WriteLine($"Monster was damaged and lost {roll} health and now has {monster} health.");
                    if (monster <= 0) continue;
                    roll = Next();
                    hero -= roll;
                    Console.WriteLine($"Hero was damaged and lost {roll} health and now has {hero} health.");
                } while (hero > 0 && monster > 0);
                Console.WriteLine(hero > 0 ? "Hero wins!" : "Monster wins!");
                break;
            case "integer":
                Console.WriteLine("Enter an integer value between 5 and 10");
                int number;
                while (true)
                {
                    string? entry = Console.ReadLine();
                    if (entry is null) return;
                    if (!int.TryParse(entry, out number))
                        Console.WriteLine("Sorry, you entered an invalid number, please try again");
                    else if (number <= 5 || number >= 10)
                        Console.WriteLine($"You entered {number}. Please enter a number between 5 and 10.");
                    else break;
                }
                Console.WriteLine($"Your input value ({number}) has been accepted.");
                break;
            case "role":
                Console.WriteLine("Enter your role name (Administrator, Manager, or User)");
                string role;
                do
                {
                    string? entry = Console.ReadLine();
                    if (entry is null) return;
                    role = entry.Trim();
                    string normalized = role.ToLower();
                    if (normalized == "administrator" || normalized == "manager" || normalized == "user") break;
                    Console.WriteLine($"The role name that you entered, \"{role}\" is not valid. Enter your role name (Administrator, Manager, or User)");
                } while (true);
                Console.WriteLine($"Your input value ({role}) has been accepted.");
                break;
            case "sentences":
                string[] myStrings = { "I like pizza. I like roast chicken. I like salad", "I like all three of the menu choices" };
                int periodLocation;
                foreach (string source in myStrings)
                {
                    string myString = source;
                    periodLocation = myString.IndexOf(".");
                    while (periodLocation != -1)
                    {
                        Console.WriteLine(myString.Substring(0, periodLocation));
                        myString = myString.Remove(0, periodLocation + 1).TrimStart();
                        periodLocation = myString.IndexOf(".");
                    }
                    Console.WriteLine(myString);
                }
                break;
        }
    }
}
