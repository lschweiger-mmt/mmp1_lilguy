/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class Particle : CircleShape
{
    public Vector2f velocity;
    public float lifetime, maxLifetime;
    private readonly bool hasGravity;
    private float despawnLifetime;
    public bool stayonground;
    const float gravity = 8000;

    public Particle(Vector2f position, float radius, Vector2f velocity, float maxLifetime, Color color, bool hasGravity = true, bool stayonground = true) : base(radius)
    {
        Position = position;
        this.velocity = velocity;
        this.maxLifetime = maxLifetime;
        this.hasGravity = hasGravity;
        this.lifetime = maxLifetime;
        FillColor = color;
        Origin = new Vector2f(radius, radius) / 2;
        despawnLifetime = (float)new Random().Next(5, 25) / 10f;
        this.stayonground = stayonground;
    }
    const float minScale = .25f;
    public void Update(float deltatime)
    {
        if (!IsAlive) return;
        if(hasGravity) velocity += new Vector2f(0, gravity) * deltatime;
        Position += velocity * deltatime;

        float scaleMult = Utilities.EasingFunction(lifetime / maxLifetime, EasingType.EaseOut);
        scaleMult = Utilities.Map(scaleMult, 0, 1, minScale, 1f);
        Scale = new(scaleMult, scaleMult);
        lifetime -= deltatime;
    }

    public void Despawn(float deltatime)
    {
        if (IsAlive) return;
        lifetime -= deltatime;
        float scaleMult = Utilities.EasingFunction((lifetime + despawnLifetime) / despawnLifetime, EasingType.SteepOut) * minScale;
        Scale = new(scaleMult, scaleMult*.666f);
        if (lifetime <= -despawnLifetime)
        {
            Scale = new();
            ParticleManager.particles.Remove(this);
        }
    }
    public bool IsAlive => lifetime > 0;


}