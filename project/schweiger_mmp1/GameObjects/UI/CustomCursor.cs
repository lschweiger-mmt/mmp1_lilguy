using SFML.Graphics;
using SFML.System;
using SFML.Window;

public static class CustomCursor
{
    public static Sprite cursor;
    public static Texture pointer;
    public static Texture hand, handdown;
    private static Game game;
    private const float cursorScale = 0.125f;
    private static float lastMoved = 0;

    public static void Initialize()
    {

        pointer = AssetManager.Instance.textures["UI/pointer"];
        hand = AssetManager.Instance.textures["UI/hand"];
        handdown = AssetManager.Instance.textures["UI/handdown"];
        game = Game.Instance;
        cursor = new Sprite(pointer);
        cursor.Origin = new(155, 80);
    }
    public static void Update(float deltatime)
    {
        if (game.currentScene == game.sceneGame && Game.gameTime > lastMoved + 2) SetVisible(false);

        Vector2f desiredPosition = ((Vector2f)Mouse.GetPosition(game.window) - (Vector2f)(game.window.Size / 2)) * (float)Settings.zoom;
        cursor.Scale = Utilities.vOne * (float)Settings.zoom * cursorScale * (VideoMode.DesktopMode.Height / 1080f);

        float expFactor = 1f - MathF.Exp(-deltatime * 50);
        cursor.Position += (desiredPosition - cursor.Position) * expFactor;

    }
    public static void Draw(RenderTarget target, RenderStates states)
    {
        cursor.Draw(target, states);
    }

    public static void ChangeType(CustomCursorType type)
    {
        switch (type)
        {
            case CustomCursorType.pointer:
                cursor.Texture = pointer;
                break;
            case CustomCursorType.hand:
                cursor.Texture = hand;
                break;
            case CustomCursorType.handdown:
                cursor.Texture = handdown;
                break;

        }
    }

    public static void SetVisible(bool v)
    {
        lastMoved = Game.gameTimeReal;
        if (!v) cursor.Texture = AssetManager.Instance.textures["shaderUV"];
        else cursor.Texture = pointer;
    }
}
public enum CustomCursorType
{
    pointer, hand, handdown
}