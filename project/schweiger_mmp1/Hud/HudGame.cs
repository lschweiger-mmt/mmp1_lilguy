using SFML.Graphics;
using SFML.System;

public class HudGame : Hud
{
    public HudGame(SceneGame scene) : base(scene) { }

    public override void Initialize()
    {
        base.Initialize();

        Health? playerHealth = (scene as SceneGame)!.player.GetComponent<Health>();
        if (playerHealth != null) playerHealth.hit += SetHp;

        SetupUI();
    }

    public UIElement scorePanel;
    public UIText currentScoreText, currentMultText, finalScoreText;
    public UIProgressBar comboBar;

    public float distanceFromScreenTop = 30, distanceFromScreenSide = 70;

    // Score
    public const float panelWidth = 900, panelHeight = 360;
    public const float spacingMiddle = 75;
    public const uint fontSize = 110;

    // Hp
    private float heartHeight = 120, heartSpacing = 16;
    private List<UISprite> heartSprites = new();

    // Rest
    private UIText guideText;

    public void SetupUI()
    {
        float barHeight = spacing;

        // Score
        scorePanel = new UIPanel(scene, anchor: new(true, true), size: new(panelWidth, panelHeight), positionFromAnchor: new(0, distanceFromScreenTop), bgColor: Game.col_purple, radius: 50);
        UIPanel scorePoints = new UIPanel(scorePanel, new UIElement.Distances((scorePanel.size.Y + spacing) / 2, (scorePanel.size.X + spacingMiddle) / 2, spacing, spacing), bgColor: Game.col_white, radius: 33);
        currentScoreText = new UIText(scorePoints, new UIElement.Distances(spacing * 1.5f), textColor: Game.col_purple, text: "0", fontSize: fontSize, textPosition: UIText.TextPosition.right);
        UIPanel scoreMult = new UIPanel(scorePanel, new UIElement.Distances((scorePanel.size.Y + spacing) / 2, spacing, spacing, (scorePanel.size.X + spacingMiddle) / 2), bgColor: Game.col_pink, radius: 33);
        currentMultText = new UIText(scoreMult, new UIElement.Distances(spacing * 1.5f), textColor: Game.col_white, text: "0", fontSize: fontSize, textPosition: UIText.TextPosition.left);
        UIText xText = new UIText(scorePanel, anchor: new Anchor(true, true, true, true), new(0, panelHeight / 4), new(spacingMiddle, panelHeight / 2), Game.col_pink, 80, "X", UIText.TextPosition.center);
        UIPanel finalScorePoints = new UIPanel(scorePanel, new UIElement.Distances(spacing, spacing, (scorePanel.size.Y + spacing) / 2, spacing), bgColor: Game.col_purple, radius: 33);
        finalScoreText = new UIText(finalScorePoints, new UIElement.Distances(spacing * 1.5f), textColor: Game.col_white, fontSize: fontSize, text: "0", textPosition: UIText.TextPosition.center);
        comboBar = new UIProgressBar(scorePanel, new(false, false, true, true), new(0, -barHeight - spacing), new(panelWidth - spacing - spacing, barHeight), Color.Transparent, 50, Game.col_purple);

        // Health
        int maxHp = 0, hp = 0;
        if ((scene as SceneGame)!.player.GetComponent(out Health? h))
        {
            maxHp = h!.maxhp;
            hp = h!.hp;
        }
        UIPanel healthPanel = new UIPanel(scene, new Anchor(true), new(distanceFromScreenSide, distanceFromScreenTop), new(99, heartHeight));
        for (int i = 0; i < maxHp; i++)
            heartSprites.Add(new UISprite(healthPanel, new Anchor(true, false, false, true), new Vector2f(heartSpacing + heartHeight, 0) * i, new(heartHeight, heartHeight), AssetManager.Instance.textures["UI/heart"]));
        SetHp(null, hp);

        // Guides
        guideText = new UIText(scene, anchor: new Anchor(false, false, true, true), new(0, distanceFromScreenTop), new(spacingMiddle, panelHeight / 2), Game.col_purple, 70, "Hold [R] to restart    Hold [ESC] to go to menu", UIText.TextPosition.center);
        guideText.SetDesiredPositionOffset(new(0, 5 * distanceFromScreenTop), 130);
    }

    public void ChangeText(UIText text, string to, Color color = new())
    {
        text.ChangeText(to, color);
    }
    public void SetHp(object? sender, int to)
    {
        for (int i = 0; i < heartSprites.Count; i++)
        {
            if (to > i) heartSprites[i].ChangeTexture(AssetManager.Instance.textures["UI/heart"]);
            else if (to == i)
            {
                int thisI = i;
                ChangeHeartTexture(thisI, AssetManager.Instance.textures["UI/heart_flash"]);
                WaitManager.Wait(.02f, () => ChangeHeartTexture(thisI, AssetManager.Instance.textures["UI/heart_empty"]));
            }
            else
            {
                ChangeHeartTexture(i, AssetManager.Instance.textures["UI/heart_empty"]);
            }
        }
    }
    private void ChangeHeartTexture(int i, Texture to)
    {
        heartSprites[i].ChangeTexture(to);
    }

    public void ShowGuideText()
    {
        guideText.SetDesiredPositionOffset(new(0, 0), 30);

        WaitManager.Wait(2, () => guideText.SetDesiredPositionOffset(new(0, 5 * distanceFromScreenTop), 30));
    }
}