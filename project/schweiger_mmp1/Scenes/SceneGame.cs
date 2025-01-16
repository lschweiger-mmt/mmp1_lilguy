/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;

public class SceneGame : Scene
{
    public Player player;
    public EnemySpawner enemySpawner;
    public List<Enemy> enemies = new();
    private List<Drawable> debugFloor = ShapeDrawer.DrawFloor(new(-5000, -5000), new(80, 80), new(200, 200), new(254, 252, 194), new(248, 232, 193));

    public float runTime = 0;

    public SceneGame() { }

    public override void Initialize()
    {
        hud = new HudGame(this);
        player = new Player(this);

        base.Initialize();
        ScoreManager.Instance.Initialize();
    }

    public override void Load()
    {
        base.Load();
        CustomCursor.SetVisible(false);

        ScoreManager.Instance.Load();
        player.Load();

        foreach (var item in enemies.ToList())
            item.Destroy();
        enemies = new();

        enemySpawner?.Destroy();
        enemySpawner = new EnemySpawner(this, 4, 10);
        ParticleManager.particles = new();

        MusicManager.Instance.PlayMusic("dreileben");
        WaitManager.StopWait("showFinalScore");

        runTime = 0;
    }

    private const float loadSceneAfterLogoTime = .7f;
    private const float endTransitionAfterLogoTime = 1f;

    public override void AfterUpdate(float deltatime)
    {
        ParticleManager.Update(deltatime);

        if (game.inputManager.GetKeyDown(SFML.Window.Keyboard.Key.R) ||
            game.inputManager.GetKeyDown(SFML.Window.Keyboard.Key.Escape))
            (game.sceneGame.hud as HudGame)?.ShowGuideText();


        if (game.inputManager.GetKeyHeld(SFML.Window.Keyboard.Key.R) > 1.5f)
        {
            game.LoadScene(ref game.sceneGame);
            game.inputManager.ResetKeyHeld(SFML.Window.Keyboard.Key.R);

        }

        if (game.inputManager.GetKeyHeld(SFML.Window.Keyboard.Key.Escape) > 1.5f)
        {
            game.inputManager.ResetKeyHeld(SFML.Window.Keyboard.Key.Escape);
            WaitManager.Wait(0, () => game.SetDesiredRadius(-.1f));
            WaitManager.Wait(0 + loadSceneAfterLogoTime, () => game.LoadScene(ref game.sceneMenu));
            WaitManager.Wait(0 + endTransitionAfterLogoTime, "circleRadius", () => game.SetMaxCircleRadius());
        }

        runTime += deltatime;
    }
    public override void BeforeDraw(RenderTarget target, RenderStates states)
    {
        if (Settings.showDebug)
            foreach (var item in debugFloor)
                item.Draw(target, states);

        children.Sort();
        ParticleManager.Draw(target, states);
    }
}