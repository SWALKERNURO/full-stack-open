// Bring in the Billing namespace and demonstrate a short type alias.
using Billing;
using Calc = Billing.Calculator;
using System.Globalization;
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

// Set up the order.
double subtotal = args.Length > 0 ? double.Parse(args[0]) : 49.99;

// Calculate tax and total using the default 8% rate.
double tax = Calculator.CalculateTax(subtotal);
double total = Calculator.CalculateTotal(subtotal, tax);

// Print the receipt.
Console.WriteLine($"Subtotal: {subtotal:C}");
Console.WriteLine($"Tax:      {tax:C}");
Console.WriteLine($"Total:    {total:C}");

// Override the optional rate with a named argument.
double outOfStateSubtotal = 80.00;
double outOfStateTax = Calc.CalculateTax(outOfStateSubtotal, taxRate: 0.05);
double outOfStateTotal = Calc.CalculateTotal(outOfStateSubtotal, outOfStateTax);
Console.WriteLine($"\nOut-of-state subtotal: {outOfStateSubtotal:C}");
Console.WriteLine($"Out-of-state tax:      {outOfStateTax:C}");
Console.WriteLine($"Out-of-state total:    {outOfStateTotal:C}");
