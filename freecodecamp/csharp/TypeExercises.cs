internal static class TypeExercises
{
    public static void Run(string? mode)
    {
        switch (mode)
        {
            case "integral":
                Console.WriteLine("Signed integral types:");
                Console.WriteLine($"sbyte  : {sbyte.MinValue} to {sbyte.MaxValue}");
                Console.WriteLine($"short  : {short.MinValue} to {short.MaxValue}");
                Console.WriteLine($"int    : {int.MinValue} to {int.MaxValue}");
                Console.WriteLine($"long   : {long.MinValue} to {long.MaxValue}");
                Console.WriteLine("\nUnsigned integral types:");
                Console.WriteLine($"byte   : {byte.MinValue} to {byte.MaxValue}");
                Console.WriteLine($"ushort : {ushort.MinValue} to {ushort.MaxValue}");
                Console.WriteLine($"uint   : {uint.MinValue} to {uint.MaxValue}");
                Console.WriteLine($"ulong  : {ulong.MinValue} to {ulong.MaxValue}");
                break;
            case "floating":
                Console.WriteLine("Floating point types:");
                Console.WriteLine($"float  : {float.MinValue} to {float.MaxValue} (with ~6-9 digits of precision)");
                Console.WriteLine($"double : {double.MinValue} to {double.MaxValue} (with ~15-17 digits of precision)");
                Console.WriteLine($"decimal: {decimal.MinValue} to {decimal.MaxValue} (with 28-29 digits of precision)");
                break;
            case "reference":
                int[] data = new int[3];
                Console.WriteLine($"Default array: {string.Join(", ", data)}");
                string shortenedString = "Hello World!";
                Console.WriteLine(shortenedString);
                int val_A = 2;
                int val_B = val_A;
                val_B = 5;
                Console.WriteLine($"--Value Types--\nval_A: {val_A}\nval_B: {val_B}");
                int[] ref_A = new int[1];
                ref_A[0] = 2;
                int[] ref_B = ref_A;
                ref_B[0] = 5;
                Console.WriteLine($"--Reference Types--\nref_A[0]: {ref_A[0]}\nref_B[0]: {ref_B[0]}");
                break;
        }
    }
}
