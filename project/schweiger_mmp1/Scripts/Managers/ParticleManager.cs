/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */


using SFML.Graphics;

public static class ParticleManager
{
    public static List<Particle> particles = new();
    public static void Update(float deltatime)
    {
        foreach (var item in particles.ToList())
            item.Despawn(deltatime);
    }

    public static void Draw(RenderTarget target, RenderStates states)
    {
        foreach (var item in particles)
        {
            item.Draw(target, states);
        }
    }
}