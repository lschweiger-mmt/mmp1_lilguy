
/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class UIUnderline : UIElement{
    public float thickness;
    public Color color;

    public UIUnderline(UIElement parent, Distances distances = default, float thickness = 3, Color color = default) : base(parent, distances)
    {
        this.thickness = thickness;
        this.color = color;
    }

    public override void SetupShape()
    {
        base.SetupShape();
        shape = new RectangleShape(new Vector2f(size.X, thickness * (float)Hud.uiScale));
        (shape as Shape)!.FillColor = color;
        shape.Position = new(0, size.Y);
    }
    public override void SetTransforms(Transformable obj)
    {
        base.SetTransforms(obj);
        if(obj != debug){
            obj.Position += new Vector2f(0, size.Y);
        }
    }
}