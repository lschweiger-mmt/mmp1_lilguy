/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class UIProgressBar : UIPanel
{
    private float progress = .3f;
    private UIPanel bar;

    public UIProgressBar(UIElement parent, Anchor anchor = new(), Vector2f positionFromAnchor = new(), Vector2f size = new(), Color bgColor = new(), float radius = 0, Color barColor = new()) : base(parent, anchor, positionFromAnchor, size, bgColor, radius)
    {
        bar = new UIPanel(this, new Anchor(true, false, false, true), new(), new(size.X * progress, size.Y), barColor, radius);
    }

    public void SetProgress(float to)
    {
        progress = to;
        progress = Math.Clamp(progress, 0, 1);
        bar.ChangeSize(new Vector2f(originalSize.X * progress, bar.originalSize.Y));
    }
}