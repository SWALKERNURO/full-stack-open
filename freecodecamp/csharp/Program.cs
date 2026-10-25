using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;
switch (args.FirstOrDefault())
{
    case "greeting": Greeting(); break;
    case "profile": Profile(); break;
    case "variables": Variables(); break;
    case "tip": Tip(); break;
    case "temperature": Temperature(); break;
    case "dice": Dice(args.Skip(1).ToArray()); break;
    case "subscription": Subscription(args.Skip(1).FirstOrDefault()); break;
    case "logic": LogicExercises.Run(args.Skip(1).ToArray()); break;
    case "scope": ScopeExercises.Run(args.Skip(1).ToArray()); break;
    case "switch": SwitchExercises.Run(args.Skip(1).ToArray()); break;
    case "for": ForExercises.Run(args.Skip(1).FirstOrDefault()); break;
    case "while": WhileExercises.Run(args.Skip(1).ToArray()); break;
    case "types": TypeExercises.Run(args.Skip(1).FirstOrDefault()); break;
    case "convert": ConversionExercises.Run(args.Skip(1).ToArray()); break;
    case "arrays": ArrayExercises.Run(args.Skip(1).ToArray()); break;
    case "format": FormattingExercises.Run(args.Skip(1).FirstOrDefault()); break;
    case "strings": StringExercises.Run(args.Skip(1).FirstOrDefault()); break;
    case "methods": FirstMethods.Run(args.Skip(1).ToArray()); break;
    case "parameters": ParameterExercises.Run(args.Skip(1).FirstOrDefault()); break;
    case "returns": ReturnExercises.Run(args.Skip(1).ToArray()); break;
    case "zoo": ZooVisit.Run(args.Skip(1).FirstOrDefault()); break;
    default: Console.WriteLine("Choose greeting, profile, variables, tip, temperature, dice, subscription, logic, scope, switch, for, while, types, convert, arrays, format, strings, methods, parameters, returns, or zoo."); break;
}

static void Greeting()
{
    Console.WriteLine("Welcome to the greeting program!");
    Console.Write("What is your name? ");
    string name = Console.ReadLine() ?? "";
    Console.WriteLine($"Hello, {name}! It's great to meet you.");
    Console.WriteLine($"Uppercase: {name.ToUpper()}");
    Console.WriteLine($"Lowercase: {name.ToLower()}");
}

static void Profile()
{
    Console.Write("What is your name? ");
    string name = Console.ReadLine() ?? "";
    Console.Write("How old are you? ");
    string age = Console.ReadLine() ?? "";
    Console.WriteLine($"Hello, {name}! You are {age} years old.");
    Console.WriteLine(new string('-', 20));
    Console.WriteLine($"Name: {name}\nAge: {age}");
    Console.WriteLine(new string('-', 20));
}

static void Variables()
{
    string favoriteFood = "pizza";
    int slicesEaten = 3;
    bool isHungry = false;
    var favoriteColor = "blue";
    Console.WriteLine($"Favorite food: {favoriteFood}");
    Console.WriteLine($"Slices eaten: {slicesEaten}");
    Console.WriteLine($"Is hungry: {isHungry}");
    Console.WriteLine($"Favorite color: {favoriteColor}");
    Console.WriteLine($"Slices eaten before dessert: {slicesEaten}");
    slicesEaten = 5;
    Console.WriteLine($"Slices eaten after dessert: {slicesEaten}");
}

static void Tip()
{
    Console.WriteLine("Enter your bill details:\n");
    Console.Write("Bill amount:    ");
    decimal billAmount = Convert.ToDecimal(Console.ReadLine());
    Console.Write("Tip percentage: ");
    decimal tipPercentage = Convert.ToDecimal(Console.ReadLine());
    Console.WriteLine("\nBill details:");
    Console.WriteLine($"  Bill amount:    ${billAmount}");
    Console.WriteLine($"  Tip percentage: {tipPercentage}%");
    decimal tipAmount = billAmount * (tipPercentage / 100);
    decimal totalAmount = billAmount + tipAmount;
    Console.WriteLine($"\nTip amount:   ${tipAmount}");
    Console.WriteLine($"Total amount: ${totalAmount}");
    Console.WriteLine($"Rounded tip: ${decimal.Round(tipAmount, 2):F2}");
    Console.WriteLine($"Formatted total: ${totalAmount:F2}");
}

static void Temperature()
{
    Console.Write("Fahrenheit temperature: ");
    decimal fahrenheit = Convert.ToDecimal(Console.ReadLine());
    decimal celsius = (fahrenheit - 32) * 5 / 9;
    Console.WriteLine($"{fahrenheit}�F is {celsius}�C");
    Console.WriteLine($"Rounded: {celsius:F1}�C");
    Console.WriteLine($"Converted back: {celsius * 9 / 5 + 32:F1}�F");
}

static void Dice(string[] suppliedRolls)
{
    Random dice = new Random();
    int roll1 = suppliedRolls.Length == 3 ? int.Parse(suppliedRolls[0]) : dice.Next(1, 7);
    int roll2 = suppliedRolls.Length == 3 ? int.Parse(suppliedRolls[1]) : dice.Next(1, 7);
    int roll3 = suppliedRolls.Length == 3 ? int.Parse(suppliedRolls[2]) : dice.Next(1, 7);
    int total = roll1 + roll2 + roll3;
    Console.WriteLine($"Dice roll: {roll1} + {roll2} + {roll3} = {total}");
    if (roll1 == roll2 || roll2 == roll3 || roll1 == roll3)
    {
        if (roll1 == roll2 && roll2 == roll3)
        {
            Console.WriteLine("You rolled triples! +6 bonus to total!");
            total += 6;
        }
        else
        {
            Console.WriteLine("You rolled doubles! +2 bonus to total!");
            total += 2;
        }
    }
    Console.WriteLine($"Final total: {total}");
    if (total >= 16) Console.WriteLine("You win a new car!");
    else if (total >= 10) Console.WriteLine("You win a new laptop!");
    else if (total == 7) Console.WriteLine("You win a trip for two!");
    else Console.WriteLine("You win a kitten!");
}

static void Subscription(string? suppliedDays)
{
    Random random = new Random();
    int daysUntilExpiration = suppliedDays is null ? random.Next(12) : int.Parse(suppliedDays);
    int discountPercentage = 0;
    if (daysUntilExpiration == 0)
        Console.WriteLine("Your subscription has expired.");
    else if (daysUntilExpiration == 1)
    {
        Console.WriteLine("Your subscription expires within a day!");
        discountPercentage = 20;
    }
    else if (daysUntilExpiration > 1 && daysUntilExpiration <= 5)
    {
        Console.WriteLine($"Your subscription expires in {daysUntilExpiration} days.");
        discountPercentage = 10;
    }
    else if (daysUntilExpiration <= 10)
        Console.WriteLine("Your subscription will expire soon. Renew now!");
    if (discountPercentage > 0)
        Console.WriteLine($"Renew now and save {discountPercentage}%.");
}
