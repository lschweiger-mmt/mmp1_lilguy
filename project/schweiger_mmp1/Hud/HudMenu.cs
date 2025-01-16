/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;

public class HudMenu : Hud
{
    public UIPanel innerPanel;
    public HudMenu(Scene scene) : base(scene) { }

    public override void Initialize()
    {

        SetupPanel(out innerPanel);
        base.Initialize();

        UISprite lilguySprite = new UISprite(innerPanel, new Anchor(true, true), new(0, paddingTop), new Vector2f(panelSize.X, panelSize.X) / 2.5f, AssetManager.Instance.textures["UI/lilguy_happy"]);
        UIText titleTextA = new UIText(innerPanel, new(), new(0, textOffset), new(buttonWidth, buttonHeight / 2), Game.col_purple, headlineSize, "This lil’guy is", UIText.TextPosition.center);
        UIText titleTextB = new UIText(innerPanel, new(), new(0, buttonHeight / 2 + textOffset), new(buttonWidth, buttonHeight / 2), Game.col_purple, headlineSize, "feeling hungry", UIText.TextPosition.center);

        MakeButton(
            innerPanel,
            "Let’s get some food",
            paddingBottom + (spacing + buttonHeight) * 2,
            Game.col_pink, Game.col_white,
            () => Game.Instance.LoadScene(ref Game.Instance.sceneGame)
        );

        MakeButton(
            innerPanel,
            "Leaderboard",
            paddingBottom + (spacing + buttonHeight) * 1,
            Game.col_white, Game.col_purple,
            () => Game.Instance.LoadScene(ref Game.Instance.sceneLeaderboard)
        );

        MakeHalfButton(
            innerPanel,
            "Quit",
            false,
            paddingBottom + (spacing + buttonHeight) * 0,
            Game.col_white, Game.col_purple,
            () => Game.Instance.window.Close()
        );

        MakeHalfButton(
            innerPanel,
            "Credits",
            true,
            paddingBottom + (spacing + buttonHeight) * 0,
            Game.col_white, Game.col_purple,
            () => Game.Instance.LoadScene(ref Game.Instance.sceneCredits)
        );


        UIButton fullscreen = new UIButton(innerPanel, new Anchor(false, true), new(spacing, spacing), new Vector2f(buttonHeight, buttonHeight) / 2, Game.col_transparent);
        UISprite fullscreenSprite = new UISprite(fullscreen, new(), Settings.fullscreen ? AssetManager.Instance.textures["UI/fullscreenoff"] : AssetManager.Instance.textures["UI/fullscreenon"]);
        fullscreen.OnRelease += () => Settings.Instance.SetFullscreen(!Settings.fullscreen);
        Settings.ChangeFullscreen += () => fullscreenSprite.ChangeTexture(Settings.fullscreen ? AssetManager.Instance.textures["UI/fullscreenoff"] : AssetManager.Instance.textures["UI/fullscreenon"]);
        UIButton music = new UIButton(innerPanel, fullscreen, new(spacing, spacing * 2 + buttonHeight / 2));
        UISprite musicSprite = new UISprite(music, new(), Settings.mute ? AssetManager.Instance.textures["UI/musicoff"] : AssetManager.Instance.textures["UI/musicon"]);
        music.OnRelease += () => Settings.Instance.SetMute(!Settings.mute);
        Settings.ChangeMute += () => musicSprite.ChangeTexture(Settings.mute ? AssetManager.Instance.textures["UI/musicoff"] : AssetManager.Instance.textures["UI/musicon"]);

    }

    public override void Load()
    {
        base.Load();
        MusicManager.Instance.PlayMusic("menu");
    }
}