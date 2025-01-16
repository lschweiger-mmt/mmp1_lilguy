/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFML.Window;

public class Game
{
    // SYSTEM
    public static Game Instance;
    public RenderWindow window;
    public View view;
    public static float gameTime { get; private set; }
    public static float gameTimeReal { get; private set; }
    public static float deltatime { get; private set; }
    public static float deltatimeDebug { get; private set; } // Time passed influenced by Debug-Timescale
    Clock clock = new Clock();

    // REFERENCES
    public Settings settings;
    public InputManager inputManager;

    // COLORS
    public static Color col_purple = new Color(61, 54, 149); // #3d3695
    public static Color col_pink = new Color(224, 94, 161); // #e05e95
    public static Color col_white = new Color(250, 248, 196); // #faf8c4
    public static Color col_transparent = new Color(0, 0, 0, 0);

    // Scenes
    public Scene currentScene;
    public SceneSplash sceneSplash;
    public SceneGame sceneGame;
    public SceneMenu sceneMenu;
    public SceneDeath sceneDeath;
    public SceneLeaderboard sceneLeaderboard;
    public SceneCredits sceneCredits;


    // Shader
    public RenderStates circleShaderStates, backgroundShaderStates;
    public RectangleShape shaderRectangleCircle, shaderRectangleBackground;


    public Game()
    {
        Instance = this;

        settings = Settings.Instance;
        settings.ReadSettings();
        settings.MakeWindow();

        // i keep the assertion here because settings.MakeWindow() creates a window and its 100% there
        view = new View(new Vector2f(0, 0), new Vector2f(window!.Size.X * (float)Settings.zoom, window.Size.Y * (float)Settings.zoom));
        inputManager = InputManager.Instance;

        window.SetView(view);
        EventManager.Closed += (s, e) => window.Close();
        EventManager.Resized += (sender, e) => settings.ResizeWindow(e);
    }


    public void Run()
    {
        window.DispatchEvents();
        Initialize();

        while (window.IsOpen)
        {
            float absoluteDeltatime = clock.Restart().AsSeconds(); // Real time passed
            deltatimeDebug = absoluteDeltatime * (float)Settings.timeScale; // Time passed influenced by Debug-Timescale
            gameTimeReal += deltatimeDebug;
            TimeManager.Instance.Update(absoluteDeltatime);
            Update(deltatimeDebug * TimeManager.Instance.timeScale);// Time passed influenced by Ingame-Timescale

            HandleEvents();

            CustomCursor.Update(deltatime);
            Draw();
            window.Display();
        }
    }

    private void HandleEvents() => window.DispatchEvents();

    private void Initialize()
    {

        LoadAssets();
        CustomCursor.Initialize();
        settings.Initialize();
        SoundManager.Instance.Initialize();
        MusicManager.Instance.Initialize();

        EventManager.MouseMoved += (sender, e) => CustomCursor.SetVisible(true);

        // Load all scenes at Initialize
        LoadScene(ref sceneGame);
        LoadScene(ref sceneDeath);
        LoadScene(ref sceneLeaderboard);
        LoadScene(ref sceneMenu);
        LoadScene(ref sceneSplash);

        InitializeShaders();

        Settings.ApplyUiScale();

        inputManager.Initialize(window);

        currentCircleRadius = 0;
    }

    private void Update(float dt)
    {
        Game.deltatime = dt;
        gameTime += dt;

        settings.Update();
        MusicManager.Instance.Update();
        SoundManager.Instance.Update();

        foreach (var waiter in WaitManager.waiters.ToDictionary())
            waiter.Value.Update();

        CollisionChecker.CheckCollisions();

        currentScene.Update(dt);

        if (sceneGame != null) ScoreManager.Instance.Update();

        inputManager.Update(dt);
    }

    private void Draw()
    {
        window.Clear(col_white);
        UpdateBackgroundShader();

        currentScene.Draw(window, RenderStates.Default);

        UpdateCircleShader();
        CustomCursor.Draw(window, RenderStates.Default);
        shaderRectangleCircle.Draw(window, new RenderStates(circleShaderStates));
    }



    private void LoadAssets()
    {
        AssetManager am = AssetManager.Instance;

        am.LoadTexture("fhlogo", "fhlogo.png");

        // Character
        am.LoadTexture("lilguy/body", "lilguy/body.png");
        am.LoadTexture("lilguy/head", "lilguy/head.png");
        am.LoadTexture("lilguy/head_back", "lilguy/head_back.png");
        am.LoadTexture("lilguy/head_evil", "lilguy/head_evil.png");
        am.LoadTexture("lilguy/head_blink", "lilguy/head_blink.png");
        am.LoadTexture("lilguy/hand", "lilguy/hand.png");

        am.LoadTexture("lilguy/iframe/body", "lilguy/iframe/body_p.png");
        am.LoadTexture("lilguy/iframe/hand", "lilguy/iframe/hand_p.png");
        am.LoadTexture("lilguy/iframe/sword", "lilguy/iframe/sword_p.png");
        am.LoadTexture("lilguy/iframe/head", "lilguy/iframe/head_p.png");
        am.LoadTexture("lilguy/shadow", "lilguy/shadow.png");


        // Weapon
        am.LoadTexture("sword/sword", "sword/sword.png");
        am.LoadTexture("slash_spritesheet", "slash_spritesheet.png");

        // Enemies
        am.LoadTexture("enemy/enemy", "enemy/enemy.png");
        am.LoadTexture("enemy/enemy_frame", "enemy/enemy_frame.png");
        am.LoadTexture("enemy/damage1", "enemy/damage1.png");
        am.LoadTexture("enemy/damage2", "enemy/damage2.png");
        am.LoadTexture("enemy/damage3", "enemy/damage3.png");

        // Fonts
        am.LoadFonts("NotoReg", "NotoSansKR-Regular.otf");
        am.LoadFonts("NotoBold", "NotoSansKR-Black.otf");

        // UI
        am.LoadTexture("UI/heart", "UI/heart.png");
        am.LoadTexture("UI/heart_flash", "UI/heart_flash.png");
        am.LoadTexture("UI/heart_empty", "UI/heart_empty.png");

        DateTime today = DateTime.Today;
        DateTime start = new DateTime(today.Year, 12, 1);
        DateTime end = new DateTime(today.Year, 12, 31);

        if (today >= start && today <= end)
        {
            am.LoadTexture("UI/lilguy_happy", "UI/lilguy_happy_xmas.png");
            am.LoadTexture("UI/lilguy_sleeping", "UI/lilguy_sleeping_xmas.png");
        }
        else
        {
            am.LoadTexture("UI/lilguy_happy", "UI/lilguy_happy.png");
            am.LoadTexture("UI/lilguy_sleeping", "UI/lilguy_sleeping.png");
        }

        am.LoadTexture("UI/musicon", "UI/musicon_1.png");
        am.LoadTexture("UI/musicoff", "UI/musicoff_1.png");
        am.LoadTexture("UI/fullscreenon", "UI/fullscreenon_1.png");
        am.LoadTexture("UI/fullscreenoff", "UI/fullscreenoff_1.png");

        am.LoadTexture("UI/keysmove", "UI/keysmove.png");
        am.LoadTexture("UI/keysslash", "UI/keysslash.png");

        am.LoadTexture("UI/pointer", "UI/pointer.png");
        am.LoadTexture("UI/hand", "UI/hand.png");
        am.LoadTexture("UI/handdown", "UI/handdown.png");

        // Music
        am.LoadMusic("einleben", "einleben.ogg");
        am.LoadMusic("zweileben", "zweileben.ogg");
        am.LoadMusic("dreileben", "dreileben.ogg");
        am.LoadMusic("menu", "menu.ogg");

        // SFX
        am.LoadManySoundBuffers("points/point", 2);
        am.LoadManySoundBuffers("points/point_yeah", 4);

        am.LoadManySoundBuffers("slash", 2);

        am.LoadSoundBuffers("tinitus", "tinitus.wav");

        am.LoadManySoundBuffers("UI/click", 3);

        am.LoadManySoundBuffers("enemies/ah", 5);
        am.LoadSoundBuffers("enemies/ahhh", "enemies/ahhhhhh.wav");
        am.LoadManySoundBuffers("lilguy/weh", 5);
        am.LoadManySoundBuffers("lilguy/ugh", 4);
        am.LoadSoundBuffers("lilguy/maumau", "lilguy/maumau.wav");

        am.LoadSoundBuffers("effects/base", "effects/base.wav");
        am.LoadSoundBuffers("effects/baseup", "effects/baseup.wav");

        // VFX
        am.LoadTexture("shaderUV", "shaderUV.png");

    }
    private void InitializeShaders()
    {
        shaderRectangleCircle = new RectangleShape();
        shaderRectangleCircle.Texture = AssetManager.Instance.textures["shaderUV"];
        circleShaderStates = new RenderStates(new Shader(null, null, $"{AssetManager.Instance.ASSETFOLDER}Shaders/circle.glsl"));
        SetMaxCircleRadius();

        Color transitionColor = col_pink;
        circleShaderStates.Shader.SetUniform("color", new Vec4(transitionColor.R / 255f, transitionColor.G / 255f, transitionColor.B / 255f, 1));


        shaderRectangleBackground = new RectangleShape();
        shaderRectangleBackground.Texture = AssetManager.Instance.textures["shaderUV"];
        backgroundShaderStates = new RenderStates(new Shader(null, null, $"{AssetManager.Instance.ASSETFOLDER}Shaders/background.glsl"));

        Color col = col_purple;
        backgroundShaderStates.Shader.SetUniform("colorA", new Vec4(col.R / 255f, col.G / 255f, col.B / 255f, 1));
        col = col_pink;
        backgroundShaderStates.Shader.SetUniform("colorB", new Vec4(col.R / 255f, col.G / 255f, col.B / 255f, 1));
    }

    private float currentCircleRadius = 0, desiredCircleRadius = 0;
    private float circleSpeed = 8;
    private void UpdateCircleShader()
    {
        float expFactor = 1f - MathF.Exp(-deltatime * circleSpeed);
        currentCircleRadius += (desiredCircleRadius - currentCircleRadius) * expFactor;
        currentCircleRadius = Math.Clamp(currentCircleRadius, 0, 5);

        Vector2f circlePos = new(0, -window.Size.Y / 1.6f);
        if (sceneGame.ActiveSelf() && sceneGame.player.GetComponent(out CircleCollider? playerCollider))
            circlePos = sceneGame.player.Position + playerCollider!.offset * 2; // i used assertion because it gets checked above. getcomponent returns false if its null
        else if (sceneSplash.ActiveSelf())
            circlePos = new();

        circleShaderStates.Shader.SetUniform("circlePosition", circlePos);
        circleShaderStates.Shader.SetUniform("windowSize", (Vector2f)view.Size);
        circleShaderStates.Shader.SetUniform("radius", currentCircleRadius);
    }
    public void SetMaxCircleRadius(bool instantly = false, float mult = 1)
    {
        desiredCircleRadius = view.Size.Magnitude() / view.Size.X * 1.35f * mult;
        if (instantly) currentCircleRadius = desiredCircleRadius;
    }
    public void SetDesiredRadius(float v)
    {
        desiredCircleRadius = v;
    }

    private Vector2f backgroundMousePos;
    private void UpdateBackgroundShader()
    {
        float expFactor = 1f - MathF.Exp(-deltatime * 3.5f);
        backgroundMousePos += ((Vector2f)Mouse.GetPosition(window) - backgroundMousePos) * expFactor;

        backgroundShaderStates.Shader.SetUniform("windowSize", (Vector2f)window.Size);
        backgroundShaderStates.Shader.SetUniform("mousePos", backgroundMousePos);
    }

    /// <summary>
    /// Loads a scene. This scene will be load and then run in Update. 
    /// Some scenes have a transition handled by the CircleShader
    /// </summary>
    public void LoadScene<T>(ref T scene, bool animate = true) where T : Scene, new()
    {
        if (scene is SceneGame && animate)
        {
            SetDesiredRadius(-.1f);
            WaitManager.Wait(1f, "loadScene", LoadGame);
            WaitManager.Wait(1f, "circleRadius", () => SetMaxCircleRadius());
            WaitManager.Wait(1.5f, "circleRadiusBigger", () => SetMaxCircleRadius(false, 2f));
            return;
        }

        if (scene == null)
        {
            scene = new T();
            scene.Initialize();
        }
        currentScene?.SetActive(false);
        currentScene = scene;
        currentScene.Load();

        WaitManager.Wait(.01f, EventManager.InvokeMouseMoved);
        WaitManager.Wait(.1f, EventManager.InvokeMouseMoved);
    }
    private void LoadGame() => LoadScene(ref sceneGame, false);
}