// Log a new event without replacing any prior entries.
string logFile = "log.txt";
string timestamp = DateTime.Now.ToString("HH:mm:ss");
File.AppendAllText(logFile, $"[{timestamp}] Application started\n");
Console.WriteLine("Event logged.\n");

// Read back every logged event, disposing the stream at the end of the scope.
if (File.Exists(logFile))
{
    Console.WriteLine("Event history:");
    using StreamReader reader = new StreamReader(logFile);
    string? line;
    while ((line = reader.ReadLine()) != null) Console.WriteLine(line);
}
else Console.WriteLine("No log file is available.");

// Count the logged events.
string[] allLines = File.ReadAllLines(logFile);
Console.WriteLine($"\nTotal events logged: {allLines.Length}");
