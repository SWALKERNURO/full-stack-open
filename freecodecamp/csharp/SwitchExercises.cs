internal static class SwitchExercises
{
    public static void Run(string[] args)
    {
        if (args.FirstOrDefault() == "sku")
        {
            string sku = args.ElementAtOrDefault(1) ?? "01-MN-L";
            string[] product = sku.Split('-');
            string type;
            string color;
            string size;
            switch (product[0])
            {
                case "01": type = "Sweat shirt"; break;
                case "02": type = "T-Shirt"; break;
                case "03": type = "Sweat pants"; break;
                default: type = "Other"; break;
            }
            switch (product[1])
            {
                case "BL": color = "Black"; break;
                case "MN": color = "Maroon"; break;
                default: color = "White"; break;
            }
            switch (product[2])
            {
                case "S": size = "Small"; break;
                case "M": size = "Medium"; break;
                case "L": size = "Large"; break;
                default: size = "One Size Fits All"; break;
            }
            Console.WriteLine($"Product: {size} {color} {type}");
            return;
        }

        int employeeLevel = int.Parse(args.ElementAtOrDefault(1) ?? "200");
        string title;
        switch (employeeLevel)
        {
            case 100:
                title = "Junior Associate";
                break;
            case 200: title = "Senior Associate"; break;
            case 300: title = "Manager"; break;
            case 400: title = "Senior Manager"; break;
            default: title = "Associate"; break;
        }
        // Demonstrate two case labels sharing one switch section.
        if (args.FirstOrDefault() == "shared")
        {
            switch (employeeLevel)
            {
                case 100:
                case 200:
                    title = "Senior Associate";
                    break;
            }
        }
        Console.WriteLine($"John Smith, {title}");
    }
}
