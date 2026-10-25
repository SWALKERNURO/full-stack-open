using System.Globalization;

internal static class FormattingExercises
{
    public static void Run(string? mode)
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        switch (mode)
        {
            case "basics":
                string first = "Hello";
                string second = "World";
                Console.WriteLine(string.Format("{0} {1}!", first, second));
                Console.WriteLine("{1} {0}!", first, second);
                Console.WriteLine("{0} {0} {0}!", first, second);
                Console.WriteLine($"{first} {second}!\n{second} {first}!\n{first} {first} {first}!");
                decimal price = 123.45m;
                int discount = 50;
                Console.WriteLine($"Price: {price:C} (Save {discount:C})");
                decimal measurement = 123456.78912m;
                // Specify precision because platform culture data can use different N defaults.
                Console.WriteLine($"Measurement: {measurement:N2} units\nMeasurement: {measurement:N4} units");
                decimal tax = .36785m;
                Console.WriteLine($"Tax rate: {tax:P2}");
                decimal regularPrice = 67.55m;
                decimal salePrice = 59.99m;
                string yourDiscount = string.Format("You saved {0:C2} off the regular {1:C2} price. ", regularPrice - salePrice, regularPrice);
                yourDiscount += $"A discount of {(regularPrice - salePrice) / regularPrice:P2}!";
                Console.WriteLine(yourDiscount);
                break;
            case "receipt":
                int invoiceNumber = 1201;
                decimal productShares = 25.4568m;
                decimal subtotal = 2750.00m;
                decimal taxPercentage = .15825m;
                decimal total = 3185.19m;
                Console.WriteLine($"Invoice Number: {invoiceNumber}");
                Console.WriteLine($"   Shares: {productShares:N3} Product");
                Console.WriteLine($"     Sub Total: {subtotal:C}");
                Console.WriteLine($"           Tax: {taxPercentage:P2}");
                Console.WriteLine($"     Total Billed: {total:C}");
                break;
            case "padding":
                string input = "Pad this";
                Console.WriteLine("[" + input.PadLeft(12) + "]");
                Console.WriteLine("[" + input.PadRight(12) + "]");
                Console.WriteLine(input.PadLeft(12, '-'));
                Console.WriteLine(input.PadRight(12, '-'));
                string paymentId = "769C";
                string payeeName = "Mr. Stephen Ortega";
                string paymentAmount = "$5,000.00";
                string formattedLine = paymentId.PadRight(6);
                formattedLine += payeeName.PadRight(24);
                formattedLine += paymentAmount.PadLeft(10);
                Console.WriteLine("1234567890123456789012345678901234567890");
                Console.WriteLine(formattedLine);
                Console.WriteLine("C110".PadLeft(6, '0'));
                break;
            case "letter":
                string customerName = "Ms. Barros";
                string currentProduct = "Magic Yield";
                int currentShares = 2975000;
                decimal currentReturn = 0.1275m;
                decimal currentProfit = 55000000.0m;
                string newProduct = "Glorious Future";
                decimal newReturn = 0.13125m;
                decimal newProfit = 63000000.0m;
                Console.WriteLine($"Dear {customerName},");
                Console.WriteLine($"As a customer of our {currentProduct} offering we are excited to tell you about a new financial product that would dramatically increase your return.\n");
                Console.WriteLine($"Currently, you own {currentShares:N2} shares at a return of {currentReturn:P2}.\n");
                Console.WriteLine($"Our new product, {newProduct} offers a return of {newReturn:P2}.  Given your current volume, your potential profit would be {newProfit:C2}.\n");
                Console.WriteLine("Here's a quick comparison:\n");
                string comparisonMessage = "";
                comparisonMessage += currentProduct.PadRight(20) + $"{currentReturn:P2}".PadRight(10) + $"{currentProfit:C2}".PadRight(20) + "\n";
                comparisonMessage += newProduct.PadRight(20) + $"{newReturn:P2}".PadRight(10) + $"{newProfit:C2}".PadRight(20);
                Console.WriteLine(comparisonMessage);
                break;
        }
    }
}

