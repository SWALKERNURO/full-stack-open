internal static class StringExercises
{
    public static void Run(string? mode)
    {
        switch (mode)
        {
            case "substring":
                string message = "Find what is (inside the parentheses)";
                int openingPosition = message.IndexOf('(');
                int closingPosition = message.IndexOf(')');
                Console.WriteLine($"{openingPosition}\n{closingPosition}");
                Console.WriteLine(message.Substring(openingPosition, closingPosition - openingPosition));
                openingPosition++;
                Console.WriteLine(message.Substring(openingPosition, closingPosition - openingPosition));
                string tagged = "What is the value <span>between the tags</span>?";
                const string openSpan = "<span>";
                const string closeSpan = "</span>";
                int start = tagged.IndexOf(openSpan) + openSpan.Length;
                Console.WriteLine(tagged.Substring(start, tagged.IndexOf(closeSpan) - start));
                break;
            case "last":
                string greeting = "hello there!";
                Console.WriteLine($"First h: {greeting.IndexOf('h')}; last h: {greeting.LastIndexOf('h')}");
                string last = "(What if) I am (only interested) in the last (set of parentheses)?";
                int begin = last.LastIndexOf('(') + 1;
                Console.WriteLine(last.Substring(begin, last.LastIndexOf(')') - begin));
                string all = "(What if) there are (more than) one (set of parentheses)?";
                while (true)
                {
                    int open = all.IndexOf('(');
                    if (open == -1) break;
                    int close = all.IndexOf(')', open + 1);
                    Console.WriteLine(all.Substring(open + 1, close - open - 1));
                    all = all.Substring(close + 1);
                }
                break;
            case "any":
                string hello = "Hello, world!";
                int index = hello.IndexOfAny(['a', 'e', 'i']);
                Console.WriteLine($"Found '{hello[index]}' in '{hello}' at index: {index}.");
                string help = "Help (find) the {opening symbols}";
                char[] openSymbols = ['[', '{', '('];
                Console.WriteLine(help.Substring(help.IndexOfAny(openSymbols)));
                Console.WriteLine(help.Substring(help.IndexOfAny(openSymbols, 5)));
                string symbols = "(What if) I have [different symbols] but every {open symbol} needs a [matching closing symbol]?";
                int position = 0;
                while (true)
                {
                    int open = symbols.IndexOfAny(openSymbols, position);
                    if (open == -1) break;
                    char match = symbols[open] switch { '[' => ']', '{' => '}', _ => ')' };
                    int close = symbols.IndexOf(match, open + 1);
                    Console.WriteLine(symbols.Substring(open + 1, close - open - 1));
                    position = close + 1;
                }
                break;
            case "replace":
                string data = "12345John Smith          5000  3  ";
                Console.WriteLine(data.Remove(5, 20));
                Console.WriteLine("This--is--ex-amp-le--da-ta".Replace("--", " ").Replace("-", ""));
                break;
            case "html":
                const string input = "<div><h2>Widgets &trade;</h2><span>5000</span></div>";
                string quantity = "";
                string output = "";
                const string quantityOpen = "<span>";
                const string quantityClose = "</span>";
                int quantityStart = input.IndexOf(quantityOpen) + quantityOpen.Length;
                quantity = "Quantity: " + input.Substring(quantityStart, input.IndexOf(quantityClose) - quantityStart);
                output = input.Remove(0, "<div>".Length);
                output = output.Remove(output.IndexOf("</div>"), "</div>".Length);
                output = "Output: " + output.Replace("&trade;", "&reg;");
                Console.WriteLine(quantity);
                Console.WriteLine(output);
                break;
        }
    }
}
