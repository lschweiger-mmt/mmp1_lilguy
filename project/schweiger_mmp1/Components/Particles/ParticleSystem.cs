/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class ParticleSystem : Component
{
    private List<Particle> particles;
    private Random random;
    private List<TextParticle> textParticles;

    public ParticleSystem(GameObject parent) : base(parent)
    {
        particles = new List<Particle>();
        textParticles = new List<TextParticle>();
        random = new Random();
    }

    const int varietyMin = 50, varietyMax = 125;

    public void Emit(Vector2f position, int count, float size, Color color, float lifetime, float speed, Vector2f direction = default, int spreadInDegrees = 360, bool gravity = true, bool stayonground = true)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = (float)(Utilities.ToRadians(random.Next(-spreadInDegrees / 2, spreadInDegrees)) + Utilities.AngleBetween(new(1, 0), direction));
            Vector2f velocity = new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;
            particles.Add(new Particle(position, random.Next(varietyMin, varietyMax) / 100f * size, random.Next(varietyMin, varietyMax) / 100f * velocity, random.Next(varietyMin, varietyMax) / 100f * lifetime, color, gravity, stayonground));
        }
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        for (int i = particles.Count - 1; i >= 0; i--)
        {
            particles[i].Update(deltaTime);
            if (!particles[i].IsAlive)
            {
                if (random.NextDouble() < .25f && particles[i].stayonground) // Probability that particles will stay on the ground
                    ParticleManager.particles.Add(particles[i]);
                particles.RemoveAt(i);
            }
        }
        for (int i = textParticles.Count - 1; i >= 0; i--)
        {
            textParticles[i].Update(deltaTime);
            if (!textParticles[i].IsAlive)
            {
                textParticles.RemoveAt(i);
            }
        }
    }

    const float textVelocity = 2100;
    public void EmitText(string text, Vector2f position, Color color, uint size = 140)
    {
        textParticles.Add(new TextParticle(position, text, size, new(0, random.Next(varietyMin, varietyMax) / 100f * -textVelocity), random.Next(varietyMin, varietyMax) / 100f * .3f, color));
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        base.Draw(target, states);
        foreach (var particle in particles)
        {
            particle.Draw(target, states);
        }
        foreach (var particle in textParticles)
        {
            particle.Draw(target, states);
        }
    }
}