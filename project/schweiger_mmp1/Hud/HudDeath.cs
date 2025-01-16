/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using System.Diagnostics;
using SFML.System;

public class HudDeath : Hud
{
    public UIText finalScoreText;
    UIInputfield nameInput;
    public HudDeath(Scene gameScene) : base(gameScene) { }

    public override void Load()
    {
        finalScoreText.ChangeText(ScoreManager.Instance.GetFinalScore().ToString("N0", Settings.nfi));
        nameInput.SetText(Settings.username);
        nameInput.canEdit = false;
        WaitManager.Wait(.5f, "enableInputField", () => nameInput.canEdit = true);
        base.Load();
    }

    const uint scoreSize = 180;
    public override void Initialize()
    {
        base.Initialize();

        SetupPanel(out UIPanel innerPanel);

        UISprite lilguySprite = new UISprite(innerPanel, new Anchor(true, true), new(0, paddingTop + 170), new Vector2f(panelSize.X, panelSize.X) / 2.5f, AssetManager.Instance.textures["UI/lilguy_sleeping"]);
        UIText titleTextA = new UIText(innerPanel, new(), new(0, textOffset), new(buttonWidth, buttonHeight / 2), Game.col_purple, headlineSize, "You had to take a nap", UIText.TextPosition.center);
        finalScoreText = new UIText(innerPanel, new(), new(0, textOffset * -1f), new(buttonWidth, buttonHeight / 2), Game.col_pink, scoreSize, "0", UIText.TextPosition.center);

        nameInput = MakeInputfield(
            innerPanel,
            paddingBottom + (spacing + buttonHeight) * 1
        );
        nameInput.onEnter += () => ScoreManager.Instance.SubmitScoreToLeaderboard(nameInput.GetText());

        MakeHalfButton(
            innerPanel,
            "Next",
            false,
            paddingBottom + (spacing + buttonHeight) * 0,
            Game.col_pink, Game.col_white,
            () => ScoreManager.Instance.SubmitScoreToLeaderboard(nameInput.GetText())
        );

        UIButton share = MakeHalfButton(
            innerPanel,
            "Share",
            true,
            paddingBottom + (spacing + buttonHeight) * 0,
            Game.col_white, Game.col_purple
        );
        share.OnRelease += () => ScoreManager.Instance.SubmitScoreToLeaderboard(nameInput.GetText());
        share.OnRelease += () => ScoreManager.Instance.PostOnImposter(nameInput.GetText());

        MakeBottomText("Skip", Game.col_purple, () => Game.Instance.LoadScene(ref Game.Instance.sceneLeaderboard));
    }
}