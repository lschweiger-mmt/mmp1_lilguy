/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class UIButton : UIPanel
{
    public Action OnRelease, OnClick, OnCancel, OnEnter, OnExit;
    public bool hovering, clicking;

    Vector2f buttonCenter;
    Vector2f buttonSize;

    public UIButton(GameObjectHolder parent, Anchor anchor = default, Vector2f positionFromAnchor = default, Vector2f size = default, Color color = default, float radius = 0) : base(parent, anchor, positionFromAnchor, size, color, radius)
    {
        SetupEvents();
    }
    public UIButton(GameObjectHolder parent, UIButton copy, Vector2f positionFromAnchor) : base(parent, copy.anchor, positionFromAnchor, copy.size, copy.color, copy.radius)
    {
        SetupEvents();
    }

    public UIButton(UIElement parent, Distances distances = default, Color color = default, float radius = 0) : base(parent, distances, color, radius)
    {
        SetupEvents();
    }

    void SetupEvents()
    {

        OnClick += () => SetButtonClickingAnimation(true);
        OnClick += () => SoundManager.Instance.Play("UI/click2");
        OnRelease += () => SetButtonClickingAnimation(false);
        OnRelease += () => SetCursorToPointer(false);
        OnRelease += () => SoundManager.Instance.Play("UI/click1");
        OnEnter += () => SetCursorToPointer(true);
        OnEnter += () => SetButtonHoveringAnimation(true);
        OnExit += () => SetCursorToPointer(false);
        OnExit += () => SetButtonHoveringAnimation(false);

        EventManager.MouseButtonPressed += OnMouseButtonPressed;
        EventManager.MouseButtonReleased += OnMouseButtonReleased;
        EventManager.MouseMovedManual += OnMouseMoved;
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
    }

    public override void SetupShape()
    {
        base.SetupShape();
        buttonCenter = (Vector2f)game.window.MapCoordsToPixel(Position);

        buttonSize = (shape as RoundedRectangleShape)!.size / (float)Settings.zoom;
    }

    private void OnMouseMoved()
    {
        if (!ActiveSelf())
        {
            if (hovering)
            {
                hovering = false;
                OnExit?.Invoke();
            }
            return;
        }

        Vector2f mousePosition = new Vector2f(Mouse.GetPosition(game.window).X, Mouse.GetPosition(game.window).Y);
        Vector2f parentPosition = scene.hud.mainPanel!.Position + (Vector2f)game.window.Size / 2;
        Vector2f mousePositionRelativeToParent = mousePosition - parentPosition;

        Vector2f mouseOffsetRotated = Utilities.RotateVector(mousePositionRelativeToParent, Utilities.ToRadians(-GetRotationOffsetFromAllParents()));
        mouseOffsetRotated -= GetPositionOffsetFromAllParents() / (float)Settings.zoom;
        Vector2f rotatedMousePosition = parentPosition + mouseOffsetRotated;

        bool hoveringBefore = hovering;
        FloatRect buttonBounds;
        if (anchor.Equals(new Anchor(false, false, true, true))) // Bottom Anchor
            buttonBounds = new FloatRect(buttonCenter - new Vector2f(buttonSize.X / 2, buttonSize.Y), buttonSize);
        else if (anchor.Equals(new Anchor())) // Center Anchor
            buttonBounds = new FloatRect(buttonCenter - new Vector2f(buttonSize.X / 2, buttonSize.Y / 2), buttonSize);
        else if (anchor.Equals(new Anchor(false, true))) // Top Right Anchor
            buttonBounds = new FloatRect(buttonCenter - new Vector2f(buttonSize.X, 0), buttonSize);
        else if (anchor.Equals(new Anchor(true))) // Top Left Anchor
            buttonBounds = new FloatRect(buttonCenter - new Vector2f(0, 0), buttonSize);
        else throw new Exception("Button Anchor not predefined");
        hovering = buttonBounds.Contains(rotatedMousePosition.X, rotatedMousePosition.Y);

        if (hovering) SetCursorToPointer(true);

        if (!hoveringBefore && hovering) OnEnter?.Invoke();
        if (hoveringBefore && !hovering) OnExit?.Invoke();
    }

    private void OnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        if (!ActiveSelf() || !scene.ActiveSelf()) return;
        if (e.Button == Mouse.Button.Left && clicking)
        {
            clicking = false;
            if (hovering)
                OnRelease?.Invoke();
            else
                OnCancel?.Invoke();
        }
        if (!ActiveSelf()) OnExit?.Invoke();

    }

    private void OnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        if (!ActiveSelf() || !scene.ActiveSelf()) return;
        if (e.Button == Mouse.Button.Left && hovering)
        {
            clicking = true;
            OnClick?.Invoke();
        }
    }

    const float hoveringOffset = -10, pressingOffset = 12;

    private void SetButtonClickingAnimation(bool clicking)
    {
        if (clicking) SetDesiredPositionOffset(new(0, pressingOffset), 30);
        else SetDesiredPositionOffset(new(), 5);
    }

    public void SetButtonHoveringAnimation(bool hovering)
    {
        if (clicking && hovering) SetButtonClickingAnimation(true);
        else if (hovering) SetDesiredPositionOffset(new(0, hoveringOffset), 20);
        else SetDesiredPositionOffset(new(), 10);
    }

    private void SetCursorToPointer(bool b)
    {
        return; // doest really work so i wont use it for now
        // if (b) CustomCursor.ChangeType(CustomCursorType.hand);
        // else CustomCursor.ChangeType(CustomCursorType.pointer);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.MouseButtonPressed -= OnMouseButtonPressed;
        EventManager.MouseButtonReleased -= OnMouseButtonReleased;
        EventManager.MouseMovedManual -= OnMouseMoved;
    }
}