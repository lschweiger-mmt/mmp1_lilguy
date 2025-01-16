/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

static class ShapeDrawer
{
    public static void DrawCircle(CircleShape modify, Vector2f position, float radius, float thickness, Color color)
    {
        modify.Origin = new Vector2f(radius, radius);
        modify.Position = position;
        modify.Radius = radius;
        modify.FillColor = Color.Transparent;
        modify.OutlineColor = color;
        modify.OutlineThickness = thickness;
    }

    // Unused, but could be used in the future
    public static CircleShape DrawCircle(Vector2f position, float radius, float thickness, Color color)
    {
        CircleShape ret = new();
        DrawCircle(ret, position, radius, thickness, color);
        return ret;
    }

    public static void DrawLine(RectangleShape modify, Vector2f startPoint, Vector2f endPoint, float thickness, Color color)
    {
        modify.Origin = new Vector2f(0, thickness / 2);
        modify.Rotation = 180 + Utilities.ToDegrees(Utilities.AngleBetween(startPoint, endPoint));
        modify.Position = startPoint;
        modify.FillColor = color;
    }

    // Unused, but could be used in the future
    public static RectangleShape DrawLine(Vector2f startPoint, Vector2f endPoint, float thickness, Color color)
    {
        RectangleShape ret = new();
        DrawLine(ret, startPoint, endPoint, thickness, color);
        return ret;
    }

    public static List<Drawable> DrawFloor(Vector2f position, Vector2i tiles, Vector2f tileSize, Color colA, Color colB)
    {
        List<Drawable> ret = new List<Drawable>();

        for (int i = 0; i < tiles.X; i++)
        {
            for (int j = 0; j < tiles.Y; j++)
            {
                RectangleShape tile = new RectangleShape(tileSize);
                tile.Position = position + new Vector2f(i * tileSize.X, j * tileSize.Y);
                if ((i + j) % 2 == 0) tile.FillColor = colA;
                else tile.FillColor = colB;

                ret.Add(tile);
            }
        }
        return ret;
    }

    public static void TextureRectOutline(RectangleShape modify, SpriteRenderer spriteRenderer)
    {
        Vector2f size = new(spriteRenderer.sprite.TextureRect.Width, spriteRenderer.sprite.TextureRect.Height);
        Vector2f sizeScaled = Utilities.ComponentProduct(size, spriteRenderer.parent.Scale);
        RectOutline(
            modify,
            spriteRenderer.sprite.Position,
            sizeScaled.X,
            sizeScaled.Y,
            spriteRenderer.parent.Rotation,
            Utilities.ComponentProduct(spriteRenderer.parent.localOrigin + size / 2, spriteRenderer.parent.Scale),
            6f,
            Color.Red
        );
    }

    // Unused, but could be used in the future
    public static void TextureRectOutline(RectangleShape modify, UISprite uiSprite)
    {
        Vector2f size = new((uiSprite.shape as Sprite)!.TextureRect.Width, (uiSprite.shape as Sprite)!.TextureRect.Height);
        Vector2f sizeScaled = Utilities.ComponentProduct(size, uiSprite.parent!.Scale);
        RectOutline(
            modify,
            (uiSprite.shape as Sprite)!.Position,
            sizeScaled.X,
            sizeScaled.Y,
            uiSprite.parent.Rotation,
            Utilities.ComponentProduct(uiSprite.parent.localOrigin + size / 2, uiSprite.parent.Scale),
            6f,
            Color.Red
        );
    }

    public static void RectOutline(RectangleShape modify, Vector2f position, float width, float height, float rotation, Vector2f origin, float thickness, Color color)
    {
        modify.Size = new Vector2f(width, height);
        modify.Position = position;
        modify.OutlineThickness = thickness;
        modify.OutlineColor = color;
        modify.FillColor = Color.Transparent;
        modify.Rotation = rotation;
        modify.Origin = origin;
    }
    public static RectangleShape RectOutline(Vector2f position, float width, float height, float rotation, Vector2f origin, float thickness, Color color)
    {
        RectangleShape ret = new();
        RectOutline(ret, position, width, height, rotation, origin, thickness, color);
        return ret;
    }
    public static void RectOutline(RectangleShape modify, Vector2f min, Vector2f max, float rotation, Vector2f origin, float thickness, Color color)
    {
        RectOutline(modify, min, (max - min).X, (max - min).Y, rotation, origin, thickness, color);
    }

    // Unused, but could be used in the future
    public static RectangleShape RectOutline(Vector2f min, Vector2f max, float rotation, Vector2f origin, float thickness, Color color)
    {
        RectangleShape ret = new();
        RectOutline(ret, min, (max - min).X, (max - min).Y, rotation, origin, thickness, color);
        return ret;
    }

}