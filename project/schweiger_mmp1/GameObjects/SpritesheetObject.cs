/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;
public class SpritesheetObject : GameObject
{
    public SpritesheetObject(GameObjectHolder parent, Texture texture, Vector2i dimensions, float framesPerSecond, bool looping = true, Transform? transform = null) : base(parent, transform)
    {
        new Spritesheet(this, texture, dimensions, framesPerSecond, looping);
    }
}