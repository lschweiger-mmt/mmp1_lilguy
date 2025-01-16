
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

public class UIInputfield : UIPanel
{
    private UIText uiText, placeHolder;
    private string inputText = "";
    public Action onEnter;
    const uint maxCharacters = 14;
    public bool canEdit = true;

    public UIInputfield(UIElement parent, Distances distances = default, Color bgColor = default, float radius = 0, uint textSize = 100, Color textColor = default, string preText = "") : base(parent, distances, bgColor, radius)
    {
        uiText = new UIText(this, new(60), textColor, textSize);
        placeHolder = new UIText(this, new(60), textColor, textSize);
        EventManager.TextEntered += OnTextEntered;
        inputText = preText;
    }

    public UIInputfield(GameObjectHolder parent, Anchor anchor = default, Vector2f positionFromAnchor = default, Vector2f size = default, Color bgColor = default, float radius = 0, uint textSize = 100, Color textColor = default, string preText = "") : base(parent, anchor, positionFromAnchor, size, bgColor, radius)
    {
        uiText = new UIText(this, new(30, 30, 30, 30), textColor, textSize);
        placeHolder = new UIText(this, new(30, 30, 30, 30), textColor, textSize, "Enter name...");
        EventManager.TextEntered += OnTextEntered;
        inputText = preText == null ? "" : preText;
    }

    const string AVAILABLESYMBOLS = " ♥’!.<():;-ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    const float blinkingTime = .75f;
    float lastLetterInput = 0, lastLetterRemovedBackspaceSpam = 0;

    public override void Update(float deltatime)
    {
        base.Update(deltatime);

        if (InputManager.Instance.GetKeyHeld(Keyboard.Key.Backspace) > blinkingTime)
        {
            if (lastLetterRemovedBackspaceSpam + (blinkingTime / 20) < Game.gameTime)
            {
                if (inputText.Length == 0) return;
                lastLetterInput = Game.gameTime;
                lastLetterRemovedBackspaceSpam = Game.gameTime;
                inputText = inputText.Substring(0, inputText.Length - 1);

                if (inputText == "") placeHolder.ChangeText(" Enter name");
                else placeHolder.ChangeText("");

            }
        }
        uiText.ChangeText(inputText + (((Game.gameTime - lastLetterInput) % (blinkingTime * 2) > blinkingTime) && lastLetterInput + blinkingTime < Game.gameTime ? "" : "|"));
    }

    private void OnTextEntered(object? sender, TextEventArgs e)
    {
        if (!ActiveSelf() || !scene.ActiveSelf()) return;
        if (!canEdit) return;

        // backspace
        if (e.Unicode == "\b")
        {
            if (inputText.Length == 0) return;
            inputText = inputText.Substring(0, inputText.Length - 1);
        }
        // enter key
        else if (e.Unicode == "\n" || e.Unicode == "\r")
        {
            onEnter?.Invoke();
        }
        // type text
        else if (AVAILABLESYMBOLS.Contains(e.Unicode))
        {
            if (inputText.Length < maxCharacters)
                inputText += e.Unicode;
        }
        lastLetterInput = Game.gameTime;
        if (inputText == "") placeHolder.ChangeText(" Enter name");
        else placeHolder.ChangeText("");
    }

    public void SetText(string to)
    {
        inputText = to;
        uiText.ChangeText(inputText);
        if (inputText == "") placeHolder.ChangeText(" Enter name");
        else placeHolder.ChangeText("");
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.TextEntered -= OnTextEntered;
    }

    public string GetText(bool trim = true){
        if(trim) return inputText.Trim();
        else return inputText;
    }
}