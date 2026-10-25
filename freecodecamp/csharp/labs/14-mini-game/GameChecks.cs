using System.Diagnostics;

internal static class GameChecks
{
    public static void Run()
    {
        void Check(bool condition, string name) { if (!condition) throw new Exception(name); Console.WriteLine("PASS " + name); }
        GameState g = new GameState(40, 20);
        g.Move(ConsoleKey.LeftArrow); g.Move(ConsoleKey.UpArrow);
        Check(g.PlayerX == 0 && g.PlayerY == 0, "left and upper bounds");
        for (int i = 0; i < 50; i++) { g.Move(ConsoleKey.RightArrow); g.Move(ConsoleKey.DownArrow); }
        Check(g.PlayerX == 40 && g.PlayerY == 20, "right and lower bounds");
        g = new GameState(40, 20);
        g.PlaceFood(5, 0, 1);
        Check(!g.AteFood(), "food initially separate");
        g.Move(ConsoleKey.RightArrow);
        Check(g.AteFood(), "food overlap detection");
        g.ChangePlayer();
        Check(g.Player == "(^-^)" && g.ShouldSpeed() && !g.ShouldFreeze(), "happy food changes appearance and speed state");
        g.Move(ConsoleKey.RightArrow, movement: 3);
        Check(g.PlayerX == 4, "optional horizontal speed");
        g.Move(ConsoleKey.RightArrow);
        Check(g.PlayerX == 5, "default horizontal speed");
        g.PlaceFood(5, 0, 2); g.ChangePlayer();
        Check(g.ShouldFreeze() && g.Player == "(X_X)", "bad food changes appearance and freeze state");
        Stopwatch clock = Stopwatch.StartNew();
        g.FreezePlayer(50);
        Check(clock.ElapsedMilliseconds >= 40 && g.Player == "('-')" && !g.ShouldFreeze(), "temporary freeze and normal-state recovery");
        g.ShowFood(new Random(42));
        Check(!g.AteFood() && g.FoodX >= 0 && g.FoodX <= 40 && g.FoodY >= 0 && g.FoodY <= 20, "food regeneration within bounds");
        Check(!g.TerminalResized(40, 20) && g.TerminalResized(41, 20) && g.TerminalResized(40, 21), "terminal resize detection");
        g.Move(ConsoleKey.A);
        Check(!g.ShouldExit, "unsupported input allowed by default");
        g.Move(ConsoleKey.A, exitOnOtherKey: true);
        Check(g.ShouldExit, "optional unsupported-input termination");
        g = new GameState(40, 20); g.Move(ConsoleKey.Escape);
        Check(g.ShouldExit, "escape termination");
    }
}
