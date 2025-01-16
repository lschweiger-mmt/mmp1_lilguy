/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class SpriteRenderer : Component
{
    public Sprite sprite;
    public RectangleShape debugSpriteBounds = new();
    private CircleShape origin = new();

    public SpriteRenderer(GameObject parent, Texture texture) : base(parent)
    {
        sprite = new Sprite(texture);
    }

    public override void SetComponentProperties()
    {
        canHaveMultiple = true;
    }

    public override void ChangePos(Vector2f pos, float rotation, Vector2f scale)
    {
        sprite.Origin = new Vector2f(sprite.TextureRect.Width / 2, sprite.TextureRect.Height / 2) + parent.localOrigin;
        sprite.Position = pos;
        sprite.Scale = scale;
        sprite.Rotation = rotation;

        if (Settings.showDebug)
        {
            Vector2f originInWorld =
                parent.Position;
            ShapeDrawer.DrawCircle(this.origin, originInWorld, .05f, 8, Color.Cyan);
            ShapeDrawer.TextureRectOutline(debugSpriteBounds, this);
        }
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        if (sprite == null) return;
        sprite.Draw(target, states);
    }

    public override void DebugDraw(RenderTarget target, RenderStates states)
    {
        debugSpriteBounds.Draw(target, states);
        origin.Draw(target, states);
    }

    public void SetTexture(Texture texture) => sprite.Texture = texture;
    public Texture GetTexture() => sprite.Texture;
}