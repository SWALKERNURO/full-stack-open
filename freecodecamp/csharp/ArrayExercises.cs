internal static class ArrayExercises
{
    public static void Run(string[] args)
    {
        switch (args.FirstOrDefault())
        {
            case "sort":
                string[] pallets = ["B14", "A11", "B12", "A13"];
                Array.Sort(pallets);
                Console.WriteLine("Sorted...\n" + string.Join("\n", pallets));
                Array.Reverse(pallets);
                Console.WriteLine("Reversed...\n" + string.Join("\n", pallets));
                break;
            case "resize":
                string?[] tracked = ["B14", "A11", "B12", "A13"];
                Console.WriteLine($"Before: {tracked[0]}");
                Array.Clear(tracked, 0, 2);
                Console.WriteLine($"After: {tracked[0]}");
                Console.WriteLine($"Cleared value is null: {tracked[0] is null}");
                Console.WriteLine($"Clearing 2 ... count: {tracked.Length}\n{string.Join("|", tracked)}");
                try { Console.WriteLine(tracked[0]!.ToLower()); }
                catch (NullReferenceException) { Console.WriteLine("Expected NullReferenceException on cleared element."); }
                if (tracked[0] != null) Console.WriteLine(tracked[0]!.ToLower());
                Array.Resize(ref tracked, 6);
                tracked[4] = "C01";
                tracked[5] = "C02";
                Console.WriteLine($"Resizing 6 ... count: {tracked.Length}\n{string.Join("|", tracked)}");
                Array.Resize(ref tracked, 3);
                Console.WriteLine($"Resizing 3 ... count: {tracked.Length}\n{string.Join("|", tracked)}");
                break;
            case "split":
                char[] chars = "abc123".ToCharArray();
                Array.Reverse(chars);
                Console.WriteLine(new string(chars));
                string joined = string.Join(",", chars);
                Console.WriteLine(joined);
                foreach (string item in joined.Split(',')) Console.WriteLine(item);
                break;
            case "words":
                string pangram = args.Length > 1 ? string.Join(" ", args.Skip(1)) : "The quick brown fox jumps over the lazy dog";
                string[] message = pangram.Split(' ');
                string[] newMessage = new string[message.Length];
                for (int i = 0; i < message.Length; i++)
                {
                    char[] letters = message[i].ToCharArray();
                    Array.Reverse(letters);
                    newMessage[i] = new string(letters);
                }
                Console.WriteLine(string.Join(" ", newMessage));
                break;
            case "orders":
                string orderStream = args.ElementAtOrDefault(1) ?? "B123,C234,A345,C15,B177,G3003,C235,B179";
                string[] orders = orderStream.Split(',');
                Array.Sort(orders);
                foreach (string order in orders) Console.WriteLine(order + (order.Length == 4 ? "" : " - Error"));
                break;
        }
    }
}
