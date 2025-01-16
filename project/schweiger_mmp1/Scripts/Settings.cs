/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using System.Globalization;
using System.Runtime.InteropServices;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Settings
{
    // This groups the Leaderboard entries
    public const string bigVersion = "0.1";
    public const string version = "0.1.3 Holidays";

    private static Settings? instance;
    public static Settings Instance
    {
        get
        {
            if (instance == null)
                instance = new Settings();

            return instance;
        }
    }
    private Settings()
    {
        game = Game.Instance;

        // Determine the folder to use for assets
        if (System.Diagnostics.Debugger.IsAttached)
        {
            // When debugging, use the assets folder relative to the project structure
            GAMEFOLDER = Path.GetFullPath("../../../../");
        }
        else
        {
            // When running as a built executable, use the folder where the .exe is located
            GAMEFOLDER = AppContext.BaseDirectory;
        }

        firstTimeOpen = !File.Exists(Path.Combine(GAMEFOLDER, "lilguy.settings"));
    }

    public static string GAMEFOLDER = "";


    // SETTINGS
    public static bool showDebug = false;
    public static double timeScale = 1;
    public static double zoom = 3.79036021232605; //my ideal zoom for 1080p
    public static Vector2u windowResolution = new(1700, 900);
    public const int ANTIALIASINGLEVEL = 16;
    public static string username;
    public static float musicVolume = 24;
    public static float soundVolumePoints = 5, soundVolumeWeapons = 38, soundVolumeVoice = 110;

    public Game game;
    public InputManager inputManager;
    public static double tempUIScale = 1;
    public static bool fullscreen = true;
    public static Action ChangeFullscreen;
    public static bool mute = false;
    public static Action ChangeMute;

    public static bool firstTimeOpen = true;

    public string newestVersion = PostgresAccess.SelectNewestVersion().Result;

    public static NumberFormatInfo nfi = new NumberFormatInfo
    {
        NumberGroupSeparator = "."
    };

    public void Initialize()
    {
        inputManager = InputManager.Instance;
        ReadSettings();
    }
    public void Update()
    {
        // This stuff is for Debugging purposes and i really want to keep it there

        // if (inputManager.GetKeyDown(Keyboard.Key.Num1)) game.LoadScene(ref game.sceneDeath);

        // if (inputManager.GetKeyDown(Keyboard.Key.F1)) AddTimescale(-.2);
        // if (inputManager.GetKeyDown(Keyboard.Key.F2)) SetTimescale(1);
        // if (inputManager.GetKeyDown(Keyboard.Key.F3)) AddTimescale(.2);

        // if (inputManager.GetKeyDown(Keyboard.Key.F4)) AddZoom(-.2);
        // if (inputManager.GetKeyDown(Keyboard.Key.F5)) SetZoom(3.8, true, true);
        // if (inputManager.GetKeyDown(Keyboard.Key.F6)) AddZoom(.2);

        if (inputManager.GetKeyDown(Keyboard.Key.F7)) AddUIscale(-.1);
        if (inputManager.GetKeyDown(Keyboard.Key.F8)) SetUIscale(1);
        if (inputManager.GetKeyDown(Keyboard.Key.F9)) AddUIscale(.1);

        // if (inputManager.GetKeyDown(Keyboard.Key.F10)) SetDebug(!showDebug);
        // if (inputManager.GetKeyDown(Keyboard.Key.F12)) game.sceneGame.player.GetComponent<HealthPlayer>()!.invulnerable = !game.sceneGame.player.GetComponent<HealthPlayer>()!.invulnerable;

        if (inputManager.GetKeyDown(Keyboard.Key.F11)) SetFullscreen(!fullscreen);
        if (inputManager.GetKeyDown(Keyboard.Key.M) && game.currentScene != game.sceneDeath) SetMute(!mute);
    }

    /// <summary>
    /// Write all settings into save-file
    /// </summary>
    public void WriteSettings()
    {
        TextWriter? writer = null;
        try
        {
            writer = new StreamWriter($"{GAMEFOLDER}lilguy.settings", false);
            string settings = $"showDebug: {showDebug}\nuiScale: {Hud.uiScale}\nwindowResolution.X: {windowResolution.X}\nwindowResolution.Y: {windowResolution.Y}\nusername: {username}\nmusicVolume: {musicVolume}\nsoundVolumePoints: {soundVolumePoints}\nsoundVolumeWeapons: {soundVolumeWeapons}\nsoundVolumeVoice: {soundVolumeVoice}\nfullscreen: {fullscreen}\nmute: {mute}";
            writer.Write(settings);
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }

    /// <summary>
    /// Read all settings from save-file
    /// </summary>
    public void ReadSettings()
    {
        TextReader? reader = null;
        try
        {
            reader = new StreamReader($"{GAMEFOLDER}lilguy.settings");
            string fileContents = reader.ReadToEnd();
            Vector2u newWindowRes = new();
            foreach (var item in fileContents.Split("\n"))
            {
                string key = item.Split(": ")[0];
                string value = item.Split(": ")[1];

                switch (key)
                {
                    case "showDebug":
                        showDebug = bool.Parse(value);
                        break;
                    case "uiScale":
                        tempUIScale = double.Parse(value);
                        break;
                    case "windowResolution.X":
                        newWindowRes = new(uint.Parse(value), 0);
                        break;
                    case "windowResolution.Y":
                        newWindowRes = new(newWindowRes.X, uint.Parse(value));
                        //Wait for window to settle in before setting size
                        WaitManager.Wait(1.7f, "resizeWindow", () => SetWindowRes(newWindowRes));
                        break;
                    case "username":
                        username = value;
                        break;
                    case "musicVolume":
                        musicVolume = float.Parse(value);
                        break;
                    case "soundVolumePoints":
                        soundVolumePoints = float.Parse(value);
                        break;
                    case "soundVolumeWeapons":
                        soundVolumeWeapons = float.Parse(value);
                        break;
                    case "soundVolumeVoice":
                        soundVolumeVoice = float.Parse(value);
                        break;
                    case "fullscreen":
                        fullscreen = bool.Parse(value);
                        break;
                    case "mute":
                        mute = bool.Parse(value);
                        MusicManager.Instance.Mute(mute);
                        break;
                    default: break;
                }
            }
        }
        catch
        {
            SetMute(mute);
            // Console.WriteLine($"some error up there: {ex}");
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }

    public void SetMute(bool v)
    {
        mute = v;
        MusicManager.Instance.Mute(v);
        WriteSettings();
        ChangeMute?.Invoke();
    }

    private void SetZoom(double to, bool resizeWindow = true, bool force = false)
    {
        zoom = to;
        if (resizeWindow) ResizeWindow(force);
        WriteSettings();
    }
    private void AddZoom(double add) => SetZoom(zoom + add, true, true);

    private void SetTimescale(double to)
    {
        timeScale = Math.Clamp(to, 0.01, 5);
        WriteSettings();
    }
    private void AddTimescale(double add) => SetTimescale(timeScale + add);

    private void SetUIscale(double to)
    {
        Hud.uiScale = to;
        game.currentScene.hud?.UpdateScale();
        WriteSettings();
    }
    private void AddUIscale(double add) => SetUIscale(Hud.uiScale + add);

    private void SetDebug(bool to)
    {
        showDebug = to;
        WriteSettings();
    }

    public void ResizeWindow(SizeEventArgs e)
    {
        if (game.view == null) return;
        windowResolution = new(e.Width, e.Height);
        ResizeWindow();
        game.SetMaxCircleRadius(true, 2);
    }

    /// <summary>
    /// Adjust everyting, view, ui, gamobjects to the new window size
    /// </summary>
    public void ResizeWindow(bool customZoom = false)
    {
        if (game.view == null) return;
        if (!customZoom) SetZoom((float)(7200f * 9 + 4050f * 16) / (float)(game.window.Size.X * 9 + game.window.Size.Y * 16), false);
        game.view.Size = new Vector2f(game.window.Size.X * (float)zoom, game.window.Size.Y * (float)zoom);
        if (game.shaderRectangleCircle != null)
        {
            game.shaderRectangleCircle.Size = game.view.Size;
            game.shaderRectangleCircle.Position = game.view.Size / -2;
        }
        if (game.shaderRectangleBackground != null)
        {
            game.shaderRectangleBackground.Size = game.view.Size;
            game.shaderRectangleBackground.Position = game.view.Size / -2;
        }
        game.window.SetView(game.view);
        game.currentScene?.hud?.UpdateWindow();
        if (!fullscreen)
            WriteSettings();
        game.currentScene?.hud?.UpdateScale();
    }

    private void SetWindowRes(Vector2u newWindowRes)
    {
        if (fullscreen) return;
        game.window.Size = newWindowRes;
        windowResolution = newWindowRes;
        game.window.Position = new(0, 0);
        ResizeWindow();
    }


    public void SetFullscreen(bool to)
    {
        fullscreen = to;
        WriteSettings();
        EventManager.UnregisterEvents(game.window);
        game.window.Close();
        MakeWindow();
        ChangeFullscreen?.Invoke();
    }
    public void MakeWindow()
    {
        if (!fullscreen)
            game.window = new RenderWindow(new VideoMode(windowResolution.X, windowResolution.Y), "MMT-B2023 MMP1 – LEO SCHWEIGER", Styles.Default, new ContextSettings() { AntialiasingLevel = Settings.ANTIALIASINGLEVEL });
        else
            game.window = new RenderWindow(VideoMode.DesktopMode, "MMT-B2023 MMP1 – LEO SCHWEIGER", Styles.Fullscreen, new ContextSettings() { AntialiasingLevel = Settings.ANTIALIASINGLEVEL });
        game.window.SetFramerateLimit(120);
        ResizeWindow();
        EventManager.RegisterEvents(game.window);
        game.window.SetKeyRepeatEnabled(false);
        game.window.SetMouseCursorVisible(false);
    }

    public static void ApplyUiScale()
    {
        Hud.uiScale = tempUIScale;
        Game.Instance.currentScene?.hud.UpdateScale();
    }
}