/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class UIText : UIElement
{
    private Color textColor;
    private uint fontSize;
    private string text;
    private TextPosition textPosition;

    public UIText(UIElement parent, Distances distances = new(), Color textColor = default, uint fontSize = 100, string text = "", TextPosition textPosition = TextPosition.left) : base(parent, distances)
    {
        SetTextProperties(textColor, fontSize, text, textPosition);
    }

    public UIText(GameObjectHolder parent, Anchor anchor = default, Vector2f positionFromAnchor = default, Vector2f size = default, Color textColor = default, uint fontSize = 100, string text = "", TextPosition textPosition = TextPosition.left) : base(parent, anchor, positionFromAnchor, size)
    {
        SetTextProperties(textColor, fontSize, text, textPosition);
    }

    public UIText(GameObjectHolder parent, UIText copy) : base(parent, copy.anchor, copy.positionFromAnchor, copy.size)
    {
        SetTextProperties(copy.textColor, copy.fontSize, copy.text, copy.textPosition);
    }
    private void SetTextProperties(Color textColor, uint fontSize, string text, TextPosition textPosition)
    {
        this.textColor = textColor;
        this.fontSize = fontSize;
        this.text = text;
        this.textPosition = textPosition;
    }

    public override void SetupShape()
    {
        Text textShape = new Text(text, AssetManager.Instance.fonts["NotoBold"]);
        textShape.FillColor = textColor;
        textShape.CharacterSize = (uint)(fontSize * Hud.uiScale);
        shape = textShape;
    }

    public void ChangeText(string to, Color color = new())
    {
        text = to;
        (shape as Text)!.DisplayedString = text;
        if (color != new Color()) textColor = color;
        (shape as Text)!.FillColor = textColor; ;
    }

    public override void SetTransforms(Transformable obj)
    {
        base.SetTransforms(obj);

        Vector2f textOffset = new();
        if (obj is Text)
        {
            Text textObj = (Text)obj;
            float horizontalOffset = 0, verticalOffset = 0;
            if (textPosition == TextPosition.left) horizontalOffset = 0;
            if (textPosition == TextPosition.center) horizontalOffset = size.X / 2 - textObj.GetLocalBounds().Width / 2;
            if (textPosition == TextPosition.right) horizontalOffset = size.X - textObj.GetLocalBounds().Width;

            verticalOffset = size.Y / 2 - fontSize / 1.6f * (float)Hud.uiScale;

            textOffset = new(horizontalOffset, verticalOffset);
        }
        obj.Position += Utilities.RotateVector(Utilities.ComponentProduct(textOffset, Scale), Utilities.ToRadians(Rotation));
    }

    public enum TextPosition
    {
        left, center, right
    }
}