/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Window;

public class InputManager
{
    private static InputManager? instance;
    public static InputManager Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }
    private InputManager() { }

    readonly Dictionary<Keyboard.Key, bool> keysPressed = new();
    readonly Dictionary<Keyboard.Key, bool> keysDown = new();
    readonly Dictionary<Keyboard.Key, bool> keysUp = new();
    readonly Dictionary<Keyboard.Key, float> keysHeld = new();

    public void Initialize(Window window)
    {
        EventManager.KeyPressed += OnKeyPressed;
        EventManager.KeyReleased += OnKeyReleased;

        // Add used Keys to that List
        List<Keyboard.Key> keys = [
            Keyboard.Key.A,
            Keyboard.Key.B,
            Keyboard.Key.C,
            Keyboard.Key.D,
            Keyboard.Key.E,
            Keyboard.Key.F,
            Keyboard.Key.G,
            Keyboard.Key.H,
            Keyboard.Key.I,
            Keyboard.Key.J,
            Keyboard.Key.K,
            Keyboard.Key.L,
            Keyboard.Key.M,
            Keyboard.Key.N,
            Keyboard.Key.O,
            Keyboard.Key.P,
            Keyboard.Key.Q,
            Keyboard.Key.R,
            Keyboard.Key.S,
            Keyboard.Key.T,
            Keyboard.Key.U,
            Keyboard.Key.V,
            Keyboard.Key.W,
            Keyboard.Key.X,
            Keyboard.Key.Y,
            Keyboard.Key.Z,
            Keyboard.Key.Num0,
            Keyboard.Key.Num1,
            Keyboard.Key.Num2,
            Keyboard.Key.Num3,
            Keyboard.Key.Num4,
            Keyboard.Key.Num5,
            Keyboard.Key.Num6,
            Keyboard.Key.Num7,
            Keyboard.Key.Num8,
            Keyboard.Key.Num9,
            Keyboard.Key.Space,
            Keyboard.Key.Enter,
            Keyboard.Key.F1,
            Keyboard.Key.F2,
            Keyboard.Key.F3,
            Keyboard.Key.F4,
            Keyboard.Key.F5,
            Keyboard.Key.F6,
            Keyboard.Key.F7,
            Keyboard.Key.F8,
            Keyboard.Key.F9,
            Keyboard.Key.F10,
            Keyboard.Key.F11,
            Keyboard.Key.F12,
            Keyboard.Key.Right,
            Keyboard.Key.Up,
            Keyboard.Key.Down,
            Keyboard.Key.Left,
            Keyboard.Key.Escape,
            Keyboard.Key.Backspace
        ];

        foreach (var key in keys)
        {
            keysPressed[key] = false;
            keysDown[key] = false;
            keysUp[key] = false;
            keysHeld[key] = 0;
        }
    }

    public void Update(float deltatime)
    {
        foreach (var item in keysDown.Keys)
            keysDown[item] = false;

        foreach (var item in keysUp.Keys)
            keysUp[item] = false;

        foreach (var item in keysHeld.Keys)
            if (keysPressed[item]) keysHeld[item] += deltatime;
    }

    private void OnKeyReleased(object? sender, KeyEventArgs e)
    {
        if (keysPressed.ContainsKey(e.Code))
        {
            keysPressed[e.Code] = false;
            keysUp[e.Code] = true;
        }
        if (keysHeld.ContainsKey(e.Code))
        {
            keysHeld[e.Code] = 0;
        }
    }

    private void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        if (keysPressed.ContainsKey(e.Code))
        {
            keysPressed[e.Code] = true;
            keysDown[e.Code] = true;
        }
    }

    /// <summary>
    /// Returns true when is pressed or held this frame
    /// </summary>
    public bool GetKeyPressed(Keyboard.Key key) => keysPressed.GetValueOrDefault(key);

    /// <summary>
    /// Returns true when is pressed this frame
    /// </summary>
    public bool GetKeyDown(Keyboard.Key key) => keysDown.GetValueOrDefault(key);

    /// <summary>
    /// Returns True when is released this frame
    /// </summary>
    public bool GetKeyUp(Keyboard.Key key) => keysUp.GetValueOrDefault(key);

    public float GetKeyHeld(Keyboard.Key key) => keysHeld.GetValueOrDefault(key);

    public void ResetKeyHeld(Keyboard.Key key)
    {
        if (keysHeld.ContainsKey(key))
        {
            keysHeld[key] = 0;
        }
    }
}