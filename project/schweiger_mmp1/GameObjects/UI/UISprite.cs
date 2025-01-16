/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class UISprite : UIElement
{
    private Texture? originalTexture;
    private Vector2f scaleMult;
    public UISprite(UIElement parent, Distances distances = default, Texture? texture = default) : base(parent, distances)
    {
        this.originalTexture = texture;
        SetupShape();
    }

    public UISprite(UIElement parent, Anchor anchor, Vector2f positionFromAnchor = default, Vector2f size = default, Texture? texture = default) : base(parent, anchor, positionFromAnchor, size)
    {
        this.originalTexture = texture;
        SetupShape();
    }

    public override void SetupShape()
    {
        if (originalTexture == null) return;
        shape = new Sprite(originalTexture);
        scaleMult = Utilities.ComponentDivision(size, (Vector2f)originalTexture!.Size);
        scaleMult = Utilities.vOne * MathF.Min(scaleMult.X, scaleMult.Y);
    }

    public override void SetTransforms(Transformable obj)
    {
        base.SetTransforms(obj);
        if (obj is Sprite)
        {
            obj.Scale = Utilities.ComponentProduct(obj.Scale, scaleMult);
            Vector2u size = (obj as Sprite)!.Texture.Size;

            if (anchor.Equals(new Anchor())) obj.Origin = (Vector2f)size / 2;
            else if (anchor.Equals(new Anchor(true, true))) obj.Origin = new Vector2f(size.X / 2, 0);
            else if (anchor.Equals(new Anchor(true, false, false, true))) obj.Origin = new Vector2f(0, size.Y / 2);
            else if (anchor.Equals(new Anchor(false, false, true, true))) obj.Origin = new Vector2f(size.X / 2, size.Y);
        }
    }

    public void ChangeTexture(Texture to)
    {
        if (to == originalTexture) return;
        originalTexture = to;

        (shape as Sprite)!.Texture = originalTexture;
        scaleMult = Utilities.ComponentDivision(size, (Vector2f)originalTexture!.Size);
        scaleMult = new(scaleMult.X, scaleMult.X);
        UpdateScale();
    }
}