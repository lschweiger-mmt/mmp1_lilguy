/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;

public class SceneSplash : Scene
{
    private const float waitBeforeLogo = 1.8f;
    private const float loadSceneAfterLogoTime = .7f;
    private const float playMusicAfterLogoTime = .8f;
    private const float endTransitionAfterLogoTime = 1f;

    public override void Initialize()
    {
        hud = new HudSplash(this);
        base.Initialize();

        const float logoTime = 4.6f;

        WaitManager.Wait(waitBeforeLogo, () => (hud as HudSplash)!.ShowLogo());
        WaitManager.Wait(waitBeforeLogo, () => SoundManager.Instance.Play("lilguy/maumau"));
        WaitManager.Wait(logoTime, () => game.SetDesiredRadius(-.1f));
        WaitManager.Wait(logoTime + loadSceneAfterLogoTime, () => game.LoadScene(ref game.sceneMenu));
        WaitManager.Wait(logoTime + playMusicAfterLogoTime, () => MusicManager.Instance.StartTrack("menu"));
        WaitManager.Wait(logoTime + endTransitionAfterLogoTime, "circleRadius", () => game.SetMaxCircleRadius());
    }

    public override void BeforeDraw(RenderTarget target, RenderStates states)
    {
        game.window.Clear(Game.col_pink);
        game.shaderRectangleBackground.Draw(target, new RenderStates(game.backgroundShaderStates));
    }
}