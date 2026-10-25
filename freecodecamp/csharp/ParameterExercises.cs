internal static class ParameterExercises
{
    public static void Run(string? mode)
    {
        switch (mode)
        {
            case "times":
                int[] schedule = { 800, 1200, 1600, 2000 };
                DisplayAdjustedTimes(schedule, 6, -6);
                void DisplayAdjustedTimes(int[] times, int currentGMT, int newGMT)
                {
                    if (currentGMT < -12 || currentGMT > 12 || newGMT < -12 || newGMT > 12) { Console.WriteLine("Invalid GMT"); return; }
                    int diff = 100 * (newGMT - currentGMT);
                    foreach (int time in times) Console.WriteLine($"{time} -> {((time + diff) % 2400 + 2400) % 2400}");
                }
                break;
            case "scope":
                string[] students = { "Jenna", "Ayesha", "Carlos", "Viktor" };
                DisplayStudents(students);
                DisplayStudents(new[] { "Robert", "Vanya" });
                void DisplayStudents(string[] students)
                {
                    foreach (string student in students) Console.Write($"{student}, ");
                    Console.WriteLine();
                }
                double pi = 3.14159;
                PrintCircleInfo(12);
                PrintCircleInfo(24);
                void PrintCircleInfo(int radius)
                {
                    Console.WriteLine($"Circle with radius {radius}");
                    PrintCircleArea(radius);
                    PrintCircleCircumference(radius);
                }
                void PrintCircleArea(int radius) { Console.WriteLine($"Area = {pi * (radius * radius)}"); }
                void PrintCircleCircumference(int radius) { Console.WriteLine($"Circumference = {2 * pi * radius}"); }
                break;
            case "value":
                int a = 3, b = 4, c = 0;
                Multiply(a, b, c);
                Console.WriteLine($"global statement: {a} x {b} = {c}");
                void Multiply(int a, int b, int c)
                {
                    c = a * b;
                    Console.WriteLine($"inside Multiply method: {a} x {b} = {c}");
                }
                break;
            case "array":
                int[] array = { 1, 2, 3, 4, 5 };
                PrintArray(array);
                Clear(array);
                PrintArray(array);
                void PrintArray(int[] array) { Console.WriteLine(string.Join(" ", array)); }
                void Clear(int[] array) { for (int i = 0; i < array.Length; i++) array[i] = 0; }
                break;
            case "string":
                string status = "Healthy";
                Console.WriteLine($"Start: {status}");
                SetHealth(status, false);
                Console.WriteLine($"End: {status}");
                void SetHealth(string status, bool isHealthy)
                {
                    status = isHealthy ? "Healthy" : "Unhealthy";
                    Console.WriteLine($"Middle: {status}");
                }
                break;
            case "captured-string":
                string capturedStatus = "Healthy";
                Console.WriteLine($"Start: {capturedStatus}");
                SetCapturedHealth(false);
                Console.WriteLine($"End: {capturedStatus}");
                void SetCapturedHealth(bool isHealthy)
                {
                    capturedStatus = isHealthy ? "Healthy" : "Unhealthy";
                    Console.WriteLine($"Middle: {capturedStatus}");
                }
                break;
            case "rsvp":
                string[] guestList = { "Rebecca", "Nadia", "Noor", "Jonte" };
                string[] rsvps = new string[10];
                int count = 0;
                RSVP("Rebecca");
                RSVP("Nadia", 2, "Nuts");
                RSVP(name: "Linh", partySize: 2, inviteOnly: false);
                RSVP("Tony", allergies: "Jackfruit", inviteOnly: true);
                RSVP("Noor", 4, inviteOnly: false);
                RSVP("Jonte", 2, "Stone fruit", false);
                ShowRSVPs();
                void RSVP(string name, int partySize = 1, string allergies = "none", bool inviteOnly = true)
                {
                    if (inviteOnly)
                    {
                        bool found = false;
                        foreach (string guest in guestList)
                            if (guest.Equals(name)) { found = true; break; }
                        if (!found) { Console.WriteLine($"Sorry, {name} is not on the guest list"); return; }
                    }
                    rsvps[count++] = $"Name: {name}, \tParty Size: {partySize}, \tAllergies: {allergies}";
                }
                void ShowRSVPs()
                {
                    Console.WriteLine("\nTotal RSVPs:");
                    for (int i = 0; i < count; i++) Console.WriteLine(rsvps[i]);
                }
                break;
            case "email":
                string[,] corporate = { { "Robert", "Bavin" }, { "Simon", "Bright" }, { "Kim", "Sinclair" }, { "Aashrita", "Kamath" }, { "Sarah", "Delucchi" }, { "Sinan", "Ali" } };
                string[,] external = { { "Vinnie", "Ashton" }, { "Cody", "Dysart" }, { "Shay", "Lawrence" }, { "Daren", "Valdes" } };
                string externalDomain = "hayworth.com";
                for (int i = 0; i < corporate.GetLength(0); i++) DisplayEmail(corporate[i, 0], corporate[i, 1]);
                for (int i = 0; i < external.GetLength(0); i++) DisplayEmail(external[i, 0], external[i, 1], externalDomain);
                void DisplayEmail(string firstName, string lastName, string domain = "contoso.com")
                { Console.WriteLine($"{firstName.Substring(0, 2)}{lastName}@{domain}".ToLower()); }
                break;
        }
    }
}
