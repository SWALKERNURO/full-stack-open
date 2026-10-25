// Boolean comparison practice from the Part 3 Microsoft Learn module.
public static class LogicExercises
{
    public static void Run(string[] args)
    {
        switch (args.FirstOrDefault() ?? "equality")
        {
            case "equality":
                Console.WriteLine("a" == "a");
                Console.WriteLine("a" == "A");
                Console.WriteLine(1 == 2);
                string myValue = "a";
                Console.WriteLine(myValue == "a");
                break;
            case "normalize":
                string value1 = " a";
                string value2 = "A ";
                Console.WriteLine(value1.Trim().ToLower() == value2.Trim().ToLower());
                break;
            case "inequality":
                Console.WriteLine("a" != "a");
                Console.WriteLine("a" != "A");
                Console.WriteLine(1 != 2);
                string unequalValue = "a";
                Console.WriteLine(unequalValue != "a");
                break;
            case "comparison":
                Console.WriteLine(1 > 2);
                Console.WriteLine(1 < 2);
                Console.WriteLine(1 >= 1);
                Console.WriteLine(1 <= 1);
                break;
            case "contains":
                string pangram = "The quick brown fox jumps over the lazy dog.";
                Console.WriteLine(pangram.Contains("fox"));
                Console.WriteLine(pangram.Contains("cow"));
                break;
            case "negation":
                string sentence = "The quick brown fox jumps over the lazy dog.";
                Console.WriteLine(!sentence.Contains("fox"));
                Console.WriteLine(!sentence.Contains("cow"));
                break;
            case "discount":
                int saleAmount = args.Length > 1 ? int.Parse(args[1]) : 1001;
                int discount = saleAmount > 1000 ? 100 : 50;
                Console.WriteLine($"Discount: {discount}");
                Console.WriteLine($"Discount: {(saleAmount > 1000 ? 100 : 50)}");
                break;
            case "coin":
                Random coin = new Random();
                int flip = args.Length > 1 ? int.Parse(args[1]) : coin.Next(0, 2);
                Console.WriteLine(flip == 0 ? "heads" : "tails");
                break;
            case "permissions":
                string permission = args.ElementAtOrDefault(1) ?? "Admin|Manager";
                int level = args.Length > 2 ? int.Parse(args[2]) : 55;
                if (permission.Contains("Admin"))
                {
                    Console.WriteLine(level > 55 ? "Welcome, Super Admin user." : "Welcome, Admin user.");
                }
                else if (permission.Contains("Manager") && level >= 20)
                    Console.WriteLine("Contact an Admin for access.");
                else Console.WriteLine("You do not have sufficient privileges.");
                break;
            default: throw new ArgumentException("Unknown Boolean exercise.");
        }
    }
}
