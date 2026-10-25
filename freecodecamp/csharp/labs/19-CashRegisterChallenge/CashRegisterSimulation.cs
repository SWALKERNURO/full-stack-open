internal static class CashRegisterSimulation { public static void Run(bool random, int seed) {
/*
This application manages transactions at a store check-out line. The
check-out line has a cash register, and the register has a cash till
that is prepared with a number of bills each morning. The till includes
bills of four denominations: $1, $5, $10, and $20. The till is used
to provide the customer with change during the transaction. The item 
cost is a randomly generated number between 2 and 49. The customer 
offers payment based on an algorithm that determines a number of bills
in each denomination. 

Each day, the cash till is loaded at the start of the day. As transactions
occur, the cash till is managed in a method named MakeChange (customer 
payments go in and the change returned to the customer comes out). A 
separate "safety check" calculation that's used to verify the amount of
money in the till is performed in the "main program". This safety check
is used to ensure that logic in the MakeChange method is working as 
expected.
*/



bool useTestData = !random;



int[] cashTill = new int[] { 0, 0, 0, 0 };
int registerCheckTillTotal = 0;

// registerDailyStartingCash: $1 x 50, $5 x 20, $10 x 10, $20 x 5 => ($350 total)
int[,] registerDailyStartingCash = new int[,] { { 1, 50 }, { 5, 20 }, { 10, 10 }, { 20, 5 } };

int[] testData = new int[] { 6, 10, 17, 20, 31, 36, 40, 41 };
int testCounter = 0;

LoadTillEachMorning(registerDailyStartingCash, cashTill);

registerCheckTillTotal = registerDailyStartingCash[0, 0] * registerDailyStartingCash[0, 1] + registerDailyStartingCash[1, 0] * registerDailyStartingCash[1, 1] + registerDailyStartingCash[2, 0] * registerDailyStartingCash[2, 1] + registerDailyStartingCash[3, 0] * registerDailyStartingCash[3, 1];

// display the number of bills of each denomination currently in the till
LogTillStatus(cashTill);

// display a message showing the amount of cash in the till
Console.WriteLine(TillAmountSummary(cashTill));

// display the expected registerDailyStartingCash total
Console.WriteLine($"Expected till value: {registerCheckTillTotal}");
if (cashTill.Any(count => count < 0) || cashTill[0] + cashTill[1] * 5 + cashTill[2] * 10 + cashTill[3] * 20 != registerCheckTillTotal)
    throw new InvalidDataException("Cash register balance invariant failed.");
Console.WriteLine();

var valueGenerator = new Random(seed);

int transactions = 100;

if (useTestData)
{
    transactions = testData.Length;
}

while (transactions > 0)
{
    transactions -= 1;
    int itemCost = valueGenerator.Next(2, 50);

    if (useTestData)
    {
        itemCost = testData[testCounter];
        testCounter += 1;
    }

    int paymentOnes = itemCost % 2;                 // value is 1 when itemCost is odd, value is 0 when itemCost is even
    int paymentFives = (itemCost % 10 > 7) ? 1 : 0; // value is 1 when itemCost ends with 8 or 9, otherwise value is 0
    int paymentTens = (itemCost % 20 > 13) ? 1 : 0; // value is 1 when 13 < itemCost < 20 OR 33 < itemCost < 40, otherwise value is 0
    int paymentTwenties = (itemCost < 20) ? 1 : 2;  // value is 1 when itemCost < 20, otherwise value is 2

    // display messages describing the current transaction
    Console.WriteLine($"Customer is making a ${itemCost} purchase");
    Console.WriteLine($"\t Using {paymentTwenties} twenty dollar bills");
    Console.WriteLine($"\t Using {paymentTens} ten dollar bills");
    Console.WriteLine($"\t Using {paymentFives} five dollar bills");
    Console.WriteLine($"\t Using {paymentOnes} one dollar bills");

    try
    {
        // MakeChange manages the transaction and updates the till 
        MakeChange(itemCost, cashTill, paymentTwenties, paymentTens, paymentFives, paymentOnes);

        // Backup Calculation - each transaction adds current "itemCost" to the till
        registerCheckTillTotal += itemCost;
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine($"Could not make transaction: {e.Message}");
    }

    Console.WriteLine(TillAmountSummary(cashTill));
    Console.WriteLine($"Expected till value: {registerCheckTillTotal}");
if (cashTill.Any(count => count < 0) || cashTill[0] + cashTill[1] * 5 + cashTill[2] * 10 + cashTill[3] * 20 != registerCheckTillTotal)
    throw new InvalidDataException("Cash register balance invariant failed.");
    Console.WriteLine();
}

Console.WriteLine("Press the Enter key to exit");
if (!Console.IsInputRedirected) Console.ReadLine();



}
static void LoadTillEachMorning(int[,] registerDailyStartingCash, int[] cashTill)
{
    cashTill[0] = registerDailyStartingCash[0, 1];
    cashTill[1] = registerDailyStartingCash[1, 1];
    cashTill[2] = registerDailyStartingCash[2, 1];
    cashTill[3] = registerDailyStartingCash[3, 1];
}


internal static void MakeChange(int cost, int[] cashTill, int twenties, int tens = 0, int fives = 0, int ones = 0)
{
    ArgumentNullException.ThrowIfNull(cashTill);
    if (cashTill.Length != 4) throw new ArgumentException("The till must have four denominations.", nameof(cashTill));
    if (cost < 0) throw new ArgumentOutOfRangeException(nameof(cost));
    if (twenties < 0 || tens < 0 || fives < 0 || ones < 0)
        throw new ArgumentOutOfRangeException(nameof(twenties), "Payment bill counts cannot be negative.");
    if (cashTill.Any(count => count < 0))
        throw new ArgumentOutOfRangeException(nameof(cashTill), "Till bill counts cannot be negative.");

    int amountPaid = checked(twenties * 20 + tens * 10 + fives * 5 + ones);
    int changeNeeded = amountPaid - cost;
    if (changeNeeded < 0)
        throw new InvalidOperationException("InvalidOperationException: Not enough money provided to complete the transaction.");

    // Stage all payment and change updates; a canceled transaction must leave
    // every bill count unchanged, including failures after some change is found.
    int[] stagedTill = (int[])cashTill.Clone();
    int[] payment = { ones, fives, tens, twenties };
    int[] denominations = { 1, 5, 10, 20 };
    int[] returned = new int[4];
    for (int i = 0; i < stagedTill.Length; i++)
        stagedTill[i] = checked(stagedTill[i] + payment[i]);

    for (int i = denominations.Length - 1; i >= 0; i--)
    {
        int count = Math.Min(changeNeeded / denominations[i], stagedTill[i]);
        stagedTill[i] -= count;
        returned[i] = count;
        changeNeeded -= count * denominations[i];
    }
    if (changeNeeded != 0)
        throw new InvalidOperationException("InvalidOperationException: The till is unable to make change for the cash provided.");

    Array.Copy(stagedTill, cashTill, cashTill.Length);
    Console.WriteLine("Cashier prepares the following change:");
    string[] labels = { "one", "five", "ten", "twenty" };
    for (int i = returned.Length - 1; i >= 0; i--)
        for (int bill = 0; bill < returned[i]; bill++)
            Console.WriteLine($"\t A {labels[i]}");
}
static void LogTillStatus(int[] cashTill)
{
    Console.WriteLine("The till currently has:");
    Console.WriteLine($"{cashTill[3] * 20} in twenties");
    Console.WriteLine($"{cashTill[2] * 10} in tens");
    Console.WriteLine($"{cashTill[1] * 5} in fives");
    Console.WriteLine($"{cashTill[0]} in ones");
    Console.WriteLine();
}

static string TillAmountSummary(int[] cashTill)
{
    return $"The till has {cashTill[3] * 20 + cashTill[2] * 10 + cashTill[1] * 5 + cashTill[0]} dollars";

}

}