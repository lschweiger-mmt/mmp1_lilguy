/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class RoundedRectangleShape : CircleShape
{
    public Vector2f size;
    public RoundedRectangleShape(Vector2f size = new(), float radius = 0) : base(radius, 100u)
    {
        this.size = size;
        Update();
    }

    public override Vector2f GetPoint(uint index)
    {
        float adjustedRadius = MathF.Min(Radius, Math.Min(size.X / 2, size.Y / 2));

        float num = (float)(index * 2 * Math.PI / GetPointCount() - Math.PI / 2.0);
        float num2 = (float)Math.Cos(num) * adjustedRadius;
        float num3 = (float)Math.Sin(num) * adjustedRadius;

        Vector2f returnVector = new Vector2f(adjustedRadius + num2, adjustedRadius + num3);
        if (index < GetPointCount() * (1f / 4f)) returnVector += new Vector2f(size.X - adjustedRadius - adjustedRadius, 0);
        else if (index < GetPointCount() * (2f / 4f)) returnVector += new Vector2f(size.X - adjustedRadius - adjustedRadius, size.Y - adjustedRadius - adjustedRadius);
        else if (index < GetPointCount() * (3f / 4f)) returnVector += new Vector2f(0, size.Y - adjustedRadius - adjustedRadius);

        return returnVector;
    }
}