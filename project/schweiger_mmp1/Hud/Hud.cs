/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using System.Diagnostics;
using SFML.Graphics;
using SFML.System;

public abstract class Hud
{
    public Scene scene;
    public List<UIElement> uiElements = new();
    public UIPanel? mainPanel;

    public Hud(Scene scene)
    {
        this.scene = scene;
    }

    public static double uiScale = 1;

    public virtual void Initialize() { }

    public virtual void Load()
    {
        Settings.Instance.ResizeWindow();
        // dreimal das gleiche weil es anders nicht wirklich funktioniert hat weil verschiedene geräte verschieden schnell laden
        // also wenn die zeit zu kurz ist kommen manche geräte beim laden nicht mit
        // und wenn es zu lang ist schauts laggy und unresponsive auf neuen geräten aus
        // es gibt sicher eine bessere lösung
        WaitManager.Wait(.05f, "resize1", () => Settings.Instance.ResizeWindow());
        WaitManager.Wait(1f, "resize2", () => Settings.Instance.ResizeWindow());
        WaitManager.Wait(3f, "resize3", () => Settings.Instance.ResizeWindow());
    }

    public virtual void Update(float deltatime)
    {
        foreach (UIElement item in uiElements)
            item.Update(deltatime);
    }
    public void UpdateWindow()
    {
        foreach (var uiElement in uiElements)
            uiElement.ChangeAnchor();
    }
    public void UpdateScale()
    {
        foreach (var uiElement in uiElements)
            uiElement.UpdateScale();
    }

    public Vector2f panelSize = new(1800, 2800);
    public const float paddingTop = 200, paddingBottom = buttonHeight / 1.5f;
    public const float textOffset = -130;
    public const float borderWidth = 20, borderRadius = 60;
    public const float buttonWidth = 1400, buttonHeight = 280, spacing = buttonHeight / 10;
    public const float buttonBorderWidth = borderWidth * .66f;
    public const uint headlineSize = 130, buttonTextSize = 90;

    public void SetupPanel(out UIPanel innerPanel)
    {
        mainPanel = new UIPanel(scene, new Anchor(), new(), panelSize, Game.col_pink, borderRadius);
        innerPanel = new UIPanel(mainPanel, new UIElement.Distances(borderWidth, borderWidth, borderWidth * 2, borderWidth), Game.col_white, borderRadius * 0.9f);
        UIText versionText = new UIText(scene, new Anchor(false, false, true), new(spacing, spacing), new(buttonWidth, buttonHeight / 4), Game.col_white, buttonTextSize / 2, Settings.version, UIText.TextPosition.right);

        // Tutorial
        if (Settings.firstTimeOpen)
        {
            const float keysWidth = 360, keysSpacing = 70, keysDistanceTop = 100;
            new UISprite(innerPanel, new Anchor(false, false, true, true), new((keysWidth + keysSpacing) / -2, -keysWidth - keysDistanceTop), new(keysWidth, keysWidth), AssetManager.Instance.textures["UI/keysmove"]);
            new UISprite(innerPanel, new Anchor(false, false, true, true), new((keysWidth + keysSpacing) / 2, -keysWidth - keysDistanceTop), new(keysWidth, keysWidth), AssetManager.Instance.textures["UI/keysslash"]);
        }
    }

    public UIButton MakeButton(UIElement parent, string text, float verticalposition, Color bgColor, Color textColor, Action? action = null)
    {
        UIButton button = new UIButton(parent, new(false, false, true, true), new(0, verticalposition), new(buttonWidth, buttonHeight), Game.col_purple, borderRadius);
        UIPanel buttonOverlay = new UIPanel(button, new(buttonBorderWidth, buttonBorderWidth, buttonBorderWidth * 2, buttonBorderWidth), bgColor, borderRadius * .67f);
        UIText buttonText = new UIText(buttonOverlay, new(borderWidth), textColor, buttonTextSize, text, UIText.TextPosition.center);

        if (action != null)
            button.OnRelease += action;
        return button;
    }
    public UIButton MakeHalfButton(UIElement parent, string text, bool left, float verticalposition, Color bgColor, Color textColor, Action? action = null)
    {
        UIButton button = new UIButton(parent, new(false, false, true, true), new((buttonWidth / 4 + spacing / 4) * (left ? -1 : 1), verticalposition), new((buttonWidth - spacing) / 2, buttonHeight), Game.col_purple, borderRadius);
        UIPanel buttonOverlay = new UIPanel(button, new(buttonBorderWidth, buttonBorderWidth, buttonBorderWidth * 2, buttonBorderWidth), bgColor, borderRadius * .67f);
        UIText buttonText = new UIText(buttonOverlay, new(borderWidth), textColor, buttonTextSize, text, UIText.TextPosition.center);

        if (action != null)
            button.OnRelease += action;
        return button;
    }

    public UIInputfield MakeInputfield(UIElement parent, float verticalposition, string preText = "")
    {
        UIPanel fieldHolder = new UIPanel(parent, new(false, false, true, true), new(0, verticalposition), new(buttonWidth, buttonHeight), Game.col_purple, borderRadius);
        UIInputfield inputfield = new UIInputfield(fieldHolder, new(buttonBorderWidth, buttonBorderWidth, buttonBorderWidth * 2, buttonBorderWidth), Game.col_white, borderRadius * .67f, buttonTextSize, Game.col_purple, preText);
        return inputfield;
    }

    public void MakeBottomText(string text, Color color, Action action)
    {
        if (mainPanel == null) throw new Exception("No UI Panel here");
        UIButton bottomButton = new(mainPanel, new Anchor(false, false, true, true), new(0, spacing * 2), new(buttonWidth, buttonHeight / 2), Game.col_white, 0);
        UIText buttonText = new UIText(bottomButton, new UIElement.Distances(), color, (uint)(buttonTextSize / 1.5f), text, UIText.TextPosition.center);
        bottomButton.OnRelease += action;
    }
}