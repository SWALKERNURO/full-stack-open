namespace Billing
{
    public static class Calculator
    {
        public static double CalculateTax(double subtotal, double taxRate = 0.08)
        {
            return subtotal * taxRate;
        }

        public static double CalculateTotal(double subtotal, double tax)
        {
            return subtotal + tax;
        }
    }
}
