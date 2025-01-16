/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

public class HudSplash : Hud
{
    public UIPanel innerPanel;
    public UISprite logo;
    public UIPanel holder;
    public HudSplash(Scene scene) : base(scene) { }

    public override void Initialize()
    {
        holder = new UIPanel(scene, new Anchor(), new(), new(1200, 1200), Game.col_white, 2000);
        logo = new UISprite(holder, new UIElement.Distances(180, 180, 120, 180), AssetManager.Instance.textures["fhlogo"]);
        holder.currentScaleOffset = new(0, 0);
        holder.SetDesiredScaleOffset(new(), 10000);
    }

    public void ShowLogo()
    {
        holder.SetDesiredScaleOffset(new(1, 1));
    }
}