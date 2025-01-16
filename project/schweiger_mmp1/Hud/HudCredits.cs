/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using System.Diagnostics;
using SFML.System;
using SFML.Window;

public class HudCredits : Hud
{
    public UIPanel innerPanel;
    public HudCredits(Scene scene) : base(scene) { }

    public override void Initialize()
    {
        base.Initialize();
        SetupPanel(out innerPanel);

        const uint itemSpacing = 140, littleHeadlineSize = 110, heightOffset = 540;

        UISprite lilguySprite = new UISprite(innerPanel, new Anchor(true, true), new(0, paddingTop), new Vector2f(panelSize.X, panelSize.X) / 3.8f, AssetManager.Instance.textures["UI/lilguy_happy"]);

        UIText gameBy = new UIText(innerPanel, new(), new(0, -heightOffset + itemSpacing * 1.5f), new(buttonWidth, buttonHeight / 2), Game.col_pink, littleHeadlineSize, "Game by", UIText.TextPosition.center);
        UIButton leoButton = new UIButton(innerPanel, new(), new(0, -heightOffset + itemSpacing * 2.5f), new(buttonWidth, buttonHeight / 2), Game.col_transparent);
        UIText leo = new UIText(leoButton, new(), Game.col_purple, buttonTextSize, "Leo Schweiger", UIText.TextPosition.center);
        const string leoUrl = "https://leoschweiger.com/";
        leoButton.OnRelease += () => Process.Start(new ProcessStartInfo(leoUrl) { UseShellExecute = true });

        UIText musicBy = new UIText(innerPanel, new(), new(0, -heightOffset + itemSpacing * 4f), new(buttonWidth, buttonHeight / 2), Game.col_pink, littleHeadlineSize, "Music & Sounds", UIText.TextPosition.center);
        UIButton sebiButton = new UIButton(innerPanel, new(), new(0, -heightOffset + itemSpacing * 5f), new(buttonWidth, buttonHeight / 2), Game.col_transparent);
        UIText sebi = new UIText(sebiButton, new(), Game.col_purple, buttonTextSize, "Sebastian Schweiger", UIText.TextPosition.center);
        const string sebiUrl = "https://www.schweigersebi.com/";
        sebiButton.OnRelease += () => Process.Start(new ProcessStartInfo(sebiUrl) { UseShellExecute = true });

        UIText voiceBy = new UIText(innerPanel, new(), new(0, -heightOffset + itemSpacing * 6.5f), new(buttonWidth, buttonHeight / 2), Game.col_pink, littleHeadlineSize, "Voices", UIText.TextPosition.center);
        UIButton saidaButton = new UIButton(innerPanel, new(), new(0, -heightOffset + itemSpacing * 7.5f), new(buttonWidth, buttonHeight / 2), Game.col_transparent);
        UIText saida = new UIText(saidaButton, new(), Game.col_purple, buttonTextSize, "Saida Feitl     ", UIText.TextPosition.center);
        UIText herz = new UIText(saidaButton, new(), Game.col_pink, (uint)(buttonTextSize / 1.4), "                                ♥", UIText.TextPosition.center);
        const string saidaUrl = "https://www.instagram.com/saidaftl";
        saidaButton.OnRelease += () => Process.Start(new ProcessStartInfo(saidaUrl) { UseShellExecute = true });

        MakeHalfButton(
            innerPanel,
            "Back",
            true,
            paddingBottom + (spacing + buttonHeight) * 0,
            Game.col_white, Game.col_purple,
            () => scene.game.LoadScene(ref scene.game.sceneMenu)
        );
        const string websiteUrl = "https://leoschweiger.com/projects/games/01+lilguy";
        MakeHalfButton(
            innerPanel,
            "About",
            false,
            paddingBottom + (spacing + buttonHeight) * 0,
            Game.col_white, Game.col_purple,
            () => Process.Start(new ProcessStartInfo(websiteUrl) { UseShellExecute = true })
        );
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
        if (InputManager.Instance.GetKeyPressed(Keyboard.Key.Escape)) scene.game.LoadScene(ref scene.game.sceneMenu);
    }
}