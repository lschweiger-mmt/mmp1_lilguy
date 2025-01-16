using SFML.Graphics;
using SFML.Window;

public static class EventManager
{
    public static event EventHandler<EventArgs> Closed;
    public static event EventHandler<SizeEventArgs> Resized;
    public static event EventHandler<KeyEventArgs> KeyPressed;
    public static event EventHandler<KeyEventArgs> KeyReleased;
    public static event EventHandler<TextEventArgs> TextEntered;
    public static event EventHandler<MouseButtonEventArgs> MouseButtonPressed;
    public static event EventHandler<MouseButtonEventArgs> MouseButtonReleased;
    public static event EventHandler<MouseMoveEventArgs> MouseMoved;
    public static event Action MouseMovedManual;
    public static event EventHandler<MouseWheelScrollEventArgs> MouseWheelScrolled;

    /// <summary>
    /// Register existing events to a new window
    /// </summary>
    public static void RegisterEvents(RenderWindow window)
    {

        window.Closed += (sender, args) => Closed?.Invoke(sender, args);
        window.Resized += (sender, args) => Resized?.Invoke(sender, args);
        window.KeyPressed += (sender, args) => KeyPressed?.Invoke(sender, args);
        window.KeyReleased += (sender, args) => KeyReleased?.Invoke(sender, args);
        window.TextEntered += (sender, args) => TextEntered?.Invoke(sender, args);
        window.MouseButtonPressed += (sender, args) => MouseButtonPressed?.Invoke(sender, args);
        window.MouseButtonReleased += (sender, args) => MouseButtonReleased?.Invoke(sender, args);
        window.MouseMoved += (sender, args) => MouseMoved?.Invoke(sender, args);
        window.MouseMoved += (sender, args) => MouseMovedManual?.Invoke();
        window.MouseWheelScrolled += (sender, args) => MouseWheelScrolled?.Invoke(sender, args);
    }

    /// <summary>
    /// Remove events from an old window
    /// </summary>
    public static void UnregisterEvents(RenderWindow window)
    {
        window.Closed -= (sender, args) => Closed?.Invoke(sender, args);
        window.Resized -= (sender, args) => Resized?.Invoke(sender, args);
        window.KeyPressed -= (sender, args) => KeyPressed?.Invoke(sender, args);
        window.KeyReleased -= (sender, args) => KeyReleased?.Invoke(sender, args);
        window.TextEntered -= (sender, args) => TextEntered?.Invoke(sender, args);
        window.MouseButtonPressed -= (sender, args) => MouseButtonPressed?.Invoke(sender, args);
        window.MouseButtonReleased -= (sender, args) => MouseButtonReleased?.Invoke(sender, args);
        window.MouseMoved -= (sender, args) => MouseMoved?.Invoke(sender, args);
        window.MouseWheelScrolled -= (sender, args) => MouseWheelScrolled?.Invoke(sender, args);
    }

    public static void InvokeMouseMoved()
    {
        MouseMovedManual?.Invoke();
    }
}