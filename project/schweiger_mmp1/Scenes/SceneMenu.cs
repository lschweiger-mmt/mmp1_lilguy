/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using System.Diagnostics;
using SFML.Graphics;

public class SceneMenu : Scene
{
    public new HudMenu hud;
    public override void Initialize()
    {
        hud = new HudMenu(this);
        base.hud = hud;
        base.Initialize();
        
        string url = "https://users.multimediatechnology.at/~fhs50567/mmp1/";
        if(Settings.Instance.newestVersion == "") hud.MakeBottomText("Not connected to the internet", Game.col_pink, SoundManager.Instance.PlayRandomAh);
        else if(Settings.Instance.newestVersion != Settings.version) hud.MakeBottomText($"A newer version is available! ({Settings.Instance.newestVersion})", Game.col_pink, () => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }));
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

    public override void AfterUpdate(float deltatime)
    {
        if(game.inputManager.GetKeyDown(SFML.Window.Keyboard.Key.Enter)) game.LoadScene(ref game.sceneGame);
    }
}