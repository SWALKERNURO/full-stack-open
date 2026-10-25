// the ourAnimals array will store the following: 
string animalSpecies = "";
string animalID = "";
string animalAge = "";
string animalPhysicalDescription = "";
string animalPersonalityDescription = "";
string animalNickname = "";

// variables that support data entry
int maxPets = 8;
string? readResult;
string menuSelection = "";

// array used to store runtime data, there is no persisted data
string[,] ourAnimals = new string[maxPets, 6];

// Sample records supplied by Microsoft Learn.

// create some initial ourAnimals array entries
for (int i = 0; i < maxPets; i++)
{
    switch (i)
    {
    case 0:
        animalSpecies = "dog";
        animalID = "d1";
        animalAge = "2";
        animalPhysicalDescription = "medium sized cream colored female golden retriever weighing about 65 pounds. housebroken.";
        animalPersonalityDescription = "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.";
        animalNickname = "lola";
        break;
    case 1:
        animalSpecies = "dog";
        animalID = "d2";
        animalAge = "9";
        animalPhysicalDescription = "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.";
        animalPersonalityDescription = "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.";
        animalNickname = "loki";
        break;
    case 2:
        animalSpecies = "cat";
        animalID = "c3";
        animalAge = "1";
        animalPhysicalDescription = "small white female weighing about 8 pounds. litter box trained.";
        animalPersonalityDescription = "friendly";
        animalNickname = "Puss";
        break;
    case 3:
        animalSpecies = "cat";
        animalID = "c4";
        animalAge = "?";
        animalPhysicalDescription = "";
        animalPersonalityDescription = "";
        animalNickname = "";
        break;
    default:
        animalSpecies = "";
        animalID = "";
        animalAge = "";
        animalPhysicalDescription = "";
        animalPersonalityDescription = "";
        animalNickname = "";
        break;
    }

    ourAnimals[i, 0] = "ID #: " + animalID;
    ourAnimals[i, 1] = "Species: " + animalSpecies;
    ourAnimals[i, 2] = "Age: " + animalAge;
    ourAnimals[i, 3] = "Nickname: " + animalNickname;
    ourAnimals[i, 4] = "Physical description: " + animalPhysicalDescription;
    ourAnimals[i, 5] = "Personality: " + animalPersonalityDescription;
}


do
{
    Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are:");
    Console.WriteLine(" 1. List all of our current pet information");
    Console.WriteLine(" 2. Add a new animal friend to the ourAnimals array");
    Console.WriteLine(" 3. Ensure animal ages and physical descriptions are complete");
    Console.WriteLine(" 4. Ensure animal nicknames and personality descriptions are complete");
    Console.WriteLine(" 5. Edit an animal�?Ts age");
    Console.WriteLine(" 6. Edit an animal�?Ts personality description");
    Console.WriteLine(" 7. Display all cats with a specified characteristic");
    Console.WriteLine(" 8. Display all dogs with a specified characteristic");
    Console.WriteLine();
    Console.WriteLine("Enter your selection number (or type Exit to exit the program)");
    
    readResult = Console.ReadLine();
    if (readResult is null) break;
    menuSelection = readResult.Trim().ToLower();
    switch (menuSelection)
    {
        case "1":
            for (int i = 0; i < maxPets; i++)
            {
                if (ourAnimals[i, 0] != "ID #: ")
                {
                    Console.WriteLine();
                    for (int j = 0; j < 6; j++) Console.WriteLine(ourAnimals[i, j]);
                }
            }
            Console.WriteLine("Press the Enter key to continue.");
            if (Console.ReadLine() is null) return;
            break;
        case "2":
            int petCount = 0;
            for (int i = 0; i < maxPets; i++)
                if (ourAnimals[i, 0] != "ID #: ") petCount++;
            if (petCount < maxPets)
                Console.WriteLine($"We currently have {petCount} pets that need homes. We can manage {maxPets - petCount} more.");
            string anotherPet = "y";
            while (anotherPet == "y" && petCount < maxPets)
            {
                bool validEntry;
                do
                {
                    Console.WriteLine("Enter 'dog' or 'cat' to begin a new entry");
                    readResult = Console.ReadLine();
                    if (readResult is null) return;
                    animalSpecies = readResult.Trim().ToLower();
                    validEntry = animalSpecies == "dog" || animalSpecies == "cat";
                } while (!validEntry);
                animalID = animalSpecies.Substring(0, 1) + (petCount + 1);
                do
                {
                    Console.WriteLine("Enter the pet's age or enter ? if unknown");
                    readResult = Console.ReadLine();
                    if (readResult is null) return;
                    animalAge = readResult.Trim();
                    validEntry = animalAge == "?" || (int.TryParse(animalAge, out int petAge) && petAge >= 0);
                } while (!validEntry);
                Console.WriteLine("Enter a physical description of the pet (size, color, gender, weight, housebroken)");
                readResult = Console.ReadLine();
                if (readResult is null) return;
                animalPhysicalDescription = readResult.Trim().ToLower();
                if (animalPhysicalDescription == "") animalPhysicalDescription = "tbd";
                Console.WriteLine("Enter a description of the pet's personality (likes or dislikes, tricks, energy level)");
                readResult = Console.ReadLine();
                if (readResult is null) return;
                animalPersonalityDescription = readResult.Trim().ToLower();
                if (animalPersonalityDescription == "") animalPersonalityDescription = "tbd";
                Console.WriteLine("Enter a nickname for the pet");
                readResult = Console.ReadLine();
                if (readResult is null) return;
                animalNickname = readResult.Trim().ToLower();
                if (animalNickname == "") animalNickname = "tbd";
                ourAnimals[petCount, 0] = "ID #: " + animalID;
                ourAnimals[petCount, 1] = "Species: " + animalSpecies;
                ourAnimals[petCount, 2] = "Age: " + animalAge;
                ourAnimals[petCount, 3] = "Nickname: " + animalNickname;
                ourAnimals[petCount, 4] = "Physical description: " + animalPhysicalDescription;
                ourAnimals[petCount, 5] = "Personality: " + animalPersonalityDescription;
                petCount++;
                if (petCount < maxPets)
                {
                    do
                    {
                        Console.WriteLine("Do you want to enter info for another pet (y/n)");
                        readResult = Console.ReadLine();
                        if (readResult is null) return;
                        anotherPet = readResult.Trim().ToLower();
                    } while (anotherPet != "y" && anotherPet != "n");
                }
            }
            if (petCount >= maxPets)
            {
                Console.WriteLine("We have reached our limit on the number of pets that we can manage.");
                Console.WriteLine("Press the Enter key to continue.");
                if (Console.ReadLine() is null) return;
            }
            break;
        case "3":
            for (int i = 0; i < maxPets; i++)
            {
                if (ourAnimals[i, 0] == "ID #: ") continue;
                string age = ourAnimals[i, 2].Substring("Age: ".Length).Trim();
                while (!int.TryParse(age, out int parsedAge) || parsedAge < 0)
                {
                    Console.WriteLine($"Enter an age for {ourAnimals[i, 0]}");
                    readResult = Console.ReadLine();
                    if (readResult is null) return;
                    age = readResult.Trim();
                }
                ourAnimals[i, 2] = "Age: " + age;
                if (Missing(ourAnimals[i, 4], "Physical description: "))
                {
                    string? description = ReadRequired($"Enter a physical description for {ourAnimals[i, 0]} (size, color, breed, gender, weight, housebroken)");
                    if (description is null) return;
                    ourAnimals[i, 4] = "Physical description: " + description;
                }
            }
            Console.WriteLine("Age and physical description fields are complete for all of our friends.");
            Console.WriteLine("Press the Enter key to continue.");
            if (Console.ReadLine() is null) return;
            break;
        case "4":
            for (int i = 0; i < maxPets; i++)
            {
                if (ourAnimals[i, 0] == "ID #: ") continue;
                if (Missing(ourAnimals[i, 3], "Nickname: "))
                {
                    string? nickname = ReadRequired($"Enter a nickname for {ourAnimals[i, 0]}");
                    if (nickname is null) return;
                    ourAnimals[i, 3] = "Nickname: " + nickname;
                }
                if (Missing(ourAnimals[i, 5], "Personality: "))
                {
                    string? personality = ReadRequired($"Enter a personality description for {ourAnimals[i, 0]} (likes or dislikes, tricks, energy level)");
                    if (personality is null) return;
                    ourAnimals[i, 5] = "Personality: " + personality;
                }
            }
            Console.WriteLine("Nickname and personality description fields are complete for all of our friends.");
            Console.WriteLine("Press the Enter key to continue.");
            if (Console.ReadLine() is null) return;
            break;
        case "5":
        case "6":
        case "7":
        case "8":
            Console.WriteLine("UNDER CONSTRUCTION - please check back next month to see progress.");
            Console.WriteLine("Press the Enter key to continue.");
            if (Console.ReadLine() is null) return;
            break;
        case "exit": break;
        default: Console.WriteLine("Please select a menu option from 1 to 8, or Exit."); break;
    }
} while (menuSelection != "exit");

static bool Missing(string field, string prefix)
{
    string value = field.Substring(prefix.Length).Trim();
    return value == "" || value == "?" || value.Equals("tbd", StringComparison.OrdinalIgnoreCase);
}

static string? ReadRequired(string prompt)
{
    string? value;
    do
    {
        Console.WriteLine(prompt);
        value = Console.ReadLine();
        if (value is null) return null;
        value = value.Trim();
    } while (value == "" || value == "?" || value.Equals("tbd", StringComparison.OrdinalIgnoreCase));
    return value;
}
