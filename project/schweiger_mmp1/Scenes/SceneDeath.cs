/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;

public class SceneDeath : Scene
{
    public override void Initialize()
    {
        hud = new HudDeath(this);
        base.Initialize();
    }

    public override void Load()
    {
        base.Load();
        MusicManager.Instance.PlayMusic("menu");
    }

    public override void BeforeDraw(RenderTarget target, RenderStates states)
    {
        game.window.Clear(Game.col_purple);
    }
}