// Console rendering and input adapt the Microsoft Learn Starter methods.
if (args.Contains("--verify")) { GameChecks.Run(); return; }
if (Console.IsInputRedirected || Console.IsOutputRedirected)
{
    Console.WriteLine("Run the mini-game in an interactive terminal. Arrow keys move; Escape exits.");
    return;
}
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
if (width < 10 || height < 3) { Console.WriteLine("Enlarge the terminal before starting the game."); return; }
GameState game = new GameState(width, height);
Random random = new Random();
bool exitOnOtherKey = !args.Contains("--allow-other-keys");
bool speedEnabled = !args.Contains("--no-speed");
bool resized = false;
bool oldCursorVisible = OperatingSystem.IsWindows() ? Console.CursorVisible : true;
try
{
    Console.CursorVisible = false;
    InitializeGame();
    while (!game.ShouldExit)
    {
        if (TerminalResized()) { resized = true; break; }
        if (game.ShouldFreeze()) { game.FreezePlayer(); DrawPlayer(); }
        Move(exitOnOtherKey, game.ShouldSpeed() && speedEnabled ? 3 : 1);
        if (resized || game.ShouldExit) break;
        if (game.AteFood())
        {
            Console.SetCursorPosition(game.FoodX, game.FoodY);
            Console.Write(new string(' ', game.Foods[game.FoodIndex].Length));
            game.ChangePlayer();
            DrawPlayer();
            ShowFood();
        }
    }
}
finally
{
    Console.Clear();
    Console.CursorVisible = oldCursorVisible;
}
Console.WriteLine(resized ? "Console was resized. Program exiting." : "Game ended.");

bool TerminalResized() => game.TerminalResized(Console.WindowWidth - 5, Console.WindowHeight - 1);
void DrawPlayer() { Console.SetCursorPosition(game.PlayerX, game.PlayerY); Console.Write(game.Player); }
void ShowFood()
{
    game.ShowFood(random);
    Console.SetCursorPosition(game.FoodX, game.FoodY);
    Console.Write(game.Foods[game.FoodIndex]);
}
void Move(bool exitOnOtherKey = false, int movement = 1)
{
    ConsoleKey key = Console.ReadKey(true).Key;
    // A resize can occur while ReadKey is waiting; check again before drawing.
    if (TerminalResized()) { resized = true; return; }
    Console.SetCursorPosition(game.PlayerX, game.PlayerY);
    Console.Write(new string(' ', game.Player.Length));
    game.Move(key, exitOnOtherKey, movement);
    if (!game.ShouldExit) DrawPlayer();
}
void InitializeGame()
{
    Console.Clear();
    ShowFood();
    string? demoFood = args.FirstOrDefault(x => x.StartsWith("--demo-food="));
    if (demoFood is not null)
    {
        Console.SetCursorPosition(game.FoodX, game.FoodY);
        Console.Write("     ");
        game.PlaceFood(5, 0, int.Parse(demoFood.Split('=')[1]));
        Console.SetCursorPosition(game.FoodX, game.FoodY);
        Console.Write(game.Foods[game.FoodIndex]);
    }
    DrawPlayer();
}
