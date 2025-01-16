/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class TextParticle : Transformable, Drawable
{
    public Vector2f velocity;
    public float lifetime, maxLifetime;
    const float gravity = 8000;
    private float rotationOffset;

    private Text textObject = new();

    public TextParticle(Vector2f position, string text, uint size, Vector2f velocity, float maxLifetime, Color color)
    {
        Position = position;
        this.velocity = velocity;
        this.maxLifetime = maxLifetime;
        this.lifetime = maxLifetime;

        textObject = new Text(text, AssetManager.Instance.fonts["NotoBold"], size);
        textObject.FillColor = Game.col_white;
        textObject.OutlineThickness = 30;
        textObject.Origin += new Vector2f(textObject.GetGlobalBounds().Width, textObject.GetGlobalBounds().Height)/2;
        textObject.OutlineColor = color;

        rotationOffset = new Random().Next(-15,15);
    }

    public void Update(float deltatime)
    {
        velocity += new Vector2f(0, gravity) * deltatime;
        Position += velocity * deltatime;
        Rotation += rotationOffset/maxLifetime * deltatime;

        if (maxLifetime - .14f > lifetime)
        {
            float scaleMult = Utilities.EasingFunction((lifetime + .14f) / (maxLifetime+.14f), EasingType.EaseOut);
            Scale = new(scaleMult, scaleMult);
        }
        lifetime -= deltatime;
        textObject.Position = Position;
        textObject.Scale = Scale;
        textObject.Rotation = Rotation;
    }
    public bool IsAlive => lifetime > 0;

    public void Draw(RenderTarget target, RenderStates states)
    {
        textObject.Draw(target, states);
    }
}