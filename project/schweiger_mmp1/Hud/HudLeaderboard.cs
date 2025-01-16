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

public class HudLeaderboard : Hud
{
    public UIText finalScoreText;
    public UIPanel leaderboardHolder;
    public UIPanel innerPanel;
    public const float entryHeight = buttonHeight / 1.5f;

    public HudLeaderboard(Scene gameScene) : base(gameScene) { }

    public override void Initialize()
    {
        base.Initialize();

        SetupPanel(out innerPanel);
        UISprite lilguySprite = new UISprite(innerPanel, new Anchor(true, true), new(0, paddingTop), new Vector2f(panelSize.X, panelSize.X) / 3.8f, AssetManager.Instance.textures["UI/lilguy_happy"]);
        UIText titleTextA = new UIText(innerPanel, new(), new(0, -400), new(buttonWidth, buttonHeight / 2), Game.col_purple, headlineSize, "Leaderboard", UIText.TextPosition.center);
        finalScoreText = new UIText(innerPanel, new(), new(0, -400 - buttonHeight / 2), new(buttonWidth, buttonHeight), Game.col_pink, headlineSize, "0", UIText.TextPosition.center);

        leaderboardHolder = new UIPanel(innerPanel, new Anchor(false, false, true, true), new(0, paddingBottom + buttonHeight + paddingBottom / 2), new(buttonWidth, (entryHeight + spacing) * SceneLeaderboard.maxEntries), Game.col_white);

        MakeHalfButton(
            innerPanel,
            "New run",
            false,
            paddingBottom + (spacing + buttonHeight) * 0,
            Game.col_pink, Game.col_white,
            () => Game.Instance.LoadScene(ref Game.Instance.sceneGame)
        );

        MakeHalfButton(
            innerPanel,
            "Back",
            true,
            paddingBottom + (spacing + buttonHeight) * 0,
            Game.col_white, Game.col_purple,
            () => Game.Instance.LoadScene(ref Game.Instance.sceneMenu)
        );

        string url = "https://users.multimediatechnology.at/~fhs50567/mmp1/";
        if (Settings.Instance.newestVersion == "") MakeBottomText("Not connected to the internet", Game.col_pink, SoundManager.Instance.PlayRandomAh);
        else MakeBottomText("Full Leaderboard", Game.col_purple, () => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }));
    }

    public void ClearLeaderboard()
    {
        foreach (var item in leaderboardHolder.children.ToList())
        {
            item.Destroy();
        }
    }

    public void AddLeaderboardPanel(LeaderboardEntry leaderboardEntry, int index)
    {
        UIElement entry = new UIElement(leaderboardHolder, new Anchor(true, true), new(0, (entryHeight / (float)Hud.uiScale + spacing / (float)Hud.uiScale) * index), new(buttonWidth / (float)Hud.uiScale, entryHeight / (float)Hud.uiScale));
        UIText number = new UIText(entry, new UIElement.Distances(spacing * 2 / (float)Hud.uiScale, spacing * 2 / (float)Hud.uiScale, spacing / (float)Hud.uiScale, spacing * 2 / (float)Hud.uiScale), Game.col_pink, buttonTextSize, $"{index + 1}.");
        UIText Name = new UIText(entry, new UIElement.Distances(spacing * 2 / (float)Hud.uiScale, spacing * 2 / (float)Hud.uiScale, spacing / (float)Hud.uiScale, entryHeight / (float)Hud.uiScale), Game.col_purple, buttonTextSize, $"{leaderboardEntry.username}");
        UIText score = new UIText(entry, new UIElement.Distances(spacing * 2 / (float)Hud.uiScale, spacing * 2 / (float)Hud.uiScale, spacing / (float)Hud.uiScale, spacing * 2 / (float)Hud.uiScale), Game.col_purple, buttonTextSize, $"{leaderboardEntry.score.ToString("N0", Settings.nfi)}", UIText.TextPosition.right);
        UIUnderline underline = new UIUnderline(entry, new(), 8 / (float)Hud.uiScale, Game.col_purple);
    }

    public override void Load()
    {
        string scoreToWrite = ScoreManager.Instance.GetFinalScore().ToString("N0", Settings.nfi);
        if (scoreToWrite == "0") scoreToWrite = "";
        finalScoreText.ChangeText(scoreToWrite);
        ScoreManager.Instance.SetFinalScore(0);
        base.Load();
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
        if (InputManager.Instance.GetKeyPressed(Keyboard.Key.Escape)) scene.game.LoadScene(ref scene.game.sceneMenu);
    }
}