// Developed from the Microsoft Learn mini-game starter's player, food and terminal state.
internal sealed class GameState
{
    public readonly string[] States = { "('-')", "(^-^)", "(X_X)" };
    public readonly string[] Foods = { "@@@@@", "$$$$$", "#####" };
    public int Width { get; }
    public int Height { get; }
    public int PlayerX { get; private set; }
    public int PlayerY { get; private set; }
    public int FoodX { get; private set; }
    public int FoodY { get; private set; }
    public int FoodIndex { get; private set; }
    public int PlayerState { get; private set; }
    public bool ShouldExit { get; private set; }
    public string Player => States[PlayerState];

    public GameState(int width, int height) { Width = width; Height = height; }
    public bool TerminalResized(int width, int height) => Width != width || Height != height;
    public bool ShouldFreeze() => PlayerState == 2;
    public bool ShouldSpeed() => PlayerState == 1;
    public bool AteFood() => PlayerY == FoodY && PlayerX <= FoodX + Foods[FoodIndex].Length - 1 && PlayerX + Player.Length - 1 >= FoodX;
    public void ChangePlayer() { PlayerState = FoodIndex; }
    public void PlaceFood(int x, int y, int index) { FoodX = x; FoodY = y; FoodIndex = index; }
    public void ShowFood(Random random)
    {
        do { PlaceFood(random.Next(0, Width + 1), random.Next(0, Height + 1), random.Next(Foods.Length)); }
        while (AteFood());
    }
    public void FreezePlayer(int milliseconds = 1000)
    {
        Thread.Sleep(milliseconds);
        PlayerState = 0;
    }
    public void Move(ConsoleKey key, bool exitOnOtherKey = false, int movement = 1)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow: PlayerY--; break;
            case ConsoleKey.DownArrow: PlayerY++; break;
            case ConsoleKey.LeftArrow: PlayerX -= movement; break;
            case ConsoleKey.RightArrow: PlayerX += movement; break;
            case ConsoleKey.Escape: ShouldExit = true; break;
            default: if (exitOnOtherKey) ShouldExit = true; break;
        }
        PlayerX = Math.Clamp(PlayerX, 0, Width);
        PlayerY = Math.Clamp(PlayerY, 0, Height);
    }
}
