/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class UIPanel : UIElement
{
    protected Color color;
    protected float radius;

    public UIPanel(UIElement parent, Distances distances = new(), Color bgColor = new(), float radius = 0) : base(parent, distances)
    {
        this.color = bgColor;
        this.radius = radius;
    }

    public UIPanel(GameObjectHolder parent, Anchor anchor = new(), Vector2f positionFromAnchor = new(), Vector2f size = new(), Color bgColor = new(), float radius = 0) : base(parent, anchor, positionFromAnchor, size)
    {
        this.color = bgColor;
        this.radius = radius;
    }

    public UIPanel(GameObjectHolder parent, UIPanel copy) : base(parent, copy.anchor, copy.positionFromAnchor, copy.size)
    {
        this.color = copy.color;
        this.radius = copy.radius;
    }
    public void SetColor(Color color) => this.color = color;

    public override void SetupShape()
    {
        shape = new RoundedRectangleShape(size, radius * (float)Hud.uiScale);
        (shape as Shape)!.FillColor = color;
    }
}