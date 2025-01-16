/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;

public class SceneCredits : Scene
{
    public override void Initialize()
    {
        hud = new HudCredits(this);
        base.Initialize();
    }

    public override void BeforeDraw(RenderTarget target, RenderStates states)
    {
        game.window.Clear(Game.col_purple);
    }
}