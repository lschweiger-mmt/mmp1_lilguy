/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
public class SpriteObject : GameObject
{
    public SpriteRenderer spriteRenderer;
    public SpriteObject(GameObjectHolder parent, Texture? texture = null, Transform? transform = null) : base(parent, transform)
    {
        if (texture != null)
            spriteRenderer = new SpriteRenderer(this, texture);
    }
}