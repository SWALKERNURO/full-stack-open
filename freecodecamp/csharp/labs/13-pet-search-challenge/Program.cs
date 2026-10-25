using System;
using System.Globalization;
CultureInfo.CurrentCulture = new CultureInfo("en-US");

// ourAnimals array will store the following: 
string animalSpecies = "";
string animalID = "";
string animalAge = "";
string animalPhysicalDescription = "";
string animalPersonalityDescription = "";
string animalNickname = "";
string suggestedDonation = "";

// variables that support data entry
int maxPets = 8;
string? readResult;
string menuSelection = "";
decimal decimalDonation = 0.00m;

// array used to store runtime data
string[,] ourAnimals = new string[maxPets, 7];

// sample data ourAnimals array entries
for (int i = 0; i < maxPets; i++)
{
    switch (i)
    {
        case 0:
            animalSpecies = "dog";
            animalID = "d1";
            animalAge = "2";
            animalPhysicalDescription = "medium sized cream colored female golden retriever weighing about 45 pounds. housebroken.";
            animalPersonalityDescription = "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.";
            animalNickname = "lola";
            suggestedDonation = "85.00";
            break;

        case 1:
            animalSpecies = "dog";
            animalID = "d2";
            animalAge = "9";
            animalPhysicalDescription = "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.";
            animalPersonalityDescription = "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.";
            animalNickname = "gus";
            suggestedDonation = "49.99";
            break;

        case 2:
            animalSpecies = "cat";
            animalID = "c3";
            animalAge = "1";
            animalPhysicalDescription = "small white female weighing about 8 pounds. litter box trained.";
            animalPersonalityDescription = "friendly";
            animalNickname = "snow";
            suggestedDonation = "40.00";
            break;

        case 3:
            animalSpecies = "cat";
            animalID = "c4";
            animalAge = "";
            animalPhysicalDescription = "";
            animalPersonalityDescription = "";
            animalNickname = "lion";
            suggestedDonation = "";

            break;

        default:
            animalSpecies = "";
            animalID = "";
            animalAge = "";
            animalPhysicalDescription = "";
            animalPersonalityDescription = "";
            animalNickname = "";
            suggestedDonation = "";
            break;

    }

    ourAnimals[i, 0] = "ID #: " + animalID;
    ourAnimals[i, 1] = "Species: " + animalSpecies;
    ourAnimals[i, 2] = "Age: " + animalAge;
    ourAnimals[i, 3] = "Nickname: " + animalNickname;
    ourAnimals[i, 4] = "Physical description: " + animalPhysicalDescription;
    ourAnimals[i, 5] = "Personality: " + animalPersonalityDescription;
    
    if (!decimal.TryParse(suggestedDonation, out decimalDonation)){
        decimalDonation = 45.00m; // if suggestedDonation NOT a number, default to 45.00
    }
    ourAnimals[i, 6] = $"Suggested Donation: {decimalDonation:C2}";
}

// top-level menu options
do
{
    // NOTE: the Console.Clear method is throwing an exception in debug sessions
    if (!Console.IsOutputRedirected) Console.Clear();

    Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are:");
    Console.WriteLine(" 1. List all of our current pet information");
    Console.WriteLine(" 2. Display all dogs with a specified characteristic");
    Console.WriteLine();
    Console.WriteLine("Enter your selection number (or type Exit to exit the program)");

    readResult = Console.ReadLine();
    if (readResult is null) break;
    menuSelection = readResult.Trim().ToLower();

    // switch-case to process the selected menu option
    switch (menuSelection)
    {
        case "1":
            // list all pet info
            for (int i = 0; i < maxPets; i++)
            {
                if (ourAnimals[i, 0] != "ID #: ")
                {
                    Console.WriteLine();
                    for (int j = 0; j < 7; j++)
                    {
                        Console.WriteLine(ourAnimals[i, j].ToString());
                    }
                }
            }

            Console.WriteLine("\r\nPress the Enter key to continue");
            readResult = Console.ReadLine();

            break;

        case "2":
            string[] terms;
            do
            {
                Console.WriteLine("Enter dog characteristics to search for separated by commas");
                readResult = Console.ReadLine();
                if (readResult is null) return;
                terms = readResult.ToLower().Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            } while (terms.Length == 0);
            Array.Sort(terms, StringComparer.Ordinal);
            bool noMatchesDog = true;
            for (int i = 0; i < maxPets; i++)
            {
                if (ourAnimals[i, 1] != "Species: dog") continue;
                bool thisDogMatches = false;
                string dogDescription = ourAnimals[i, 4] + "\n" + ourAnimals[i, 5];
                foreach (string term in terms)
                {
                    AnimateSearch(ourAnimals[i, 3], term);
                    if (dogDescription.Contains(term, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Our dog {ourAnimals[i, 3]} matches your search for {term}");
                        thisDogMatches = true;
                    }
                }
                if (thisDogMatches)
                {
                    Console.WriteLine($"{ourAnimals[i, 3]} ({ourAnimals[i, 0]})");
                    Console.WriteLine(dogDescription);
                    noMatchesDog = false;
                }
            }
            if (noMatchesDog) Console.WriteLine("No matches found for any available dogs: " + string.Join(", ", terms));
            Console.WriteLine("Press the Enter key to continue");
            if (Console.ReadLine() is null) return;
            break;

        default:
            break;
    }

} while (menuSelection != "exit");

static void AnimateSearch(string nickname, string term)
{
    string[] searchingIcons = { "|", "/", "-", "\\" };
    int width = 0;
    for (int countdown = 2; countdown >= 0; countdown--)
    {
        foreach (string icon in searchingIcons)
        {
            string status = $"searching our dog {nickname} for {term} {icon} {countdown}";
            width = Math.Max(width, status.Length);
            Console.Write("\r" + status.PadRight(width));
            if (!Console.IsOutputRedirected) Thread.Sleep(80);
        }
    }
    Console.Write("\r" + new string(' ', width) + "\r");
}
