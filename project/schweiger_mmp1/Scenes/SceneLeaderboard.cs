/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using System.Diagnostics;
using SFML.Graphics;

public class SceneLeaderboard : Scene
{
    public new HudLeaderboard hud;

    public override void Initialize()
    {
        hud = new HudLeaderboard(this);
        base.hud = hud;
        base.Initialize();
    }
    string newestVersion = PostgresAccess.SelectNewestVersion().Result;

    public override void Load()
    {
        base.Load();
        cangoon = false;
        hud.ClearLeaderboard();
        WaitManager.Wait(.1f, "loadleaderboard", LoadLeaderboard);
    }

    List<LeaderboardEntry> currentLeaderboard = new();
    private bool cangoon = false;
    public const int maxEntries = 5;
    public async void LoadLeaderboard()
    {
        // currentLeaderboard = await PostgresAccess.SelectLeaderboard($"SELECT * FROM leaderboard WHERE ver = '{Settings.version}' ORDER BY score DESC LIMIT {maxEntries};");
        currentLeaderboard = await PostgresAccess.SelectLeaderboard($"SELECT * FROM(SELECT DISTINCT ON (TRIM(player)) * FROM leaderboard WHERE ver LIKE '{Settings.bigVersion}%' AND score > 0 AND hide IS NOT true ORDER BY TRIM(player), score DESC) subquery ORDER BY score DESC LIMIT {maxEntries}");

        for (int i = 0; i < currentLeaderboard.Count; i++)
        {
            (hud as HudLeaderboard)?.AddLeaderboardPanel(currentLeaderboard[i], i);
        }
    }
    public override void BeforeDraw(RenderTarget target, RenderStates states)
    {
        game.window.Clear(Game.col_purple);
    }

    public override void AfterUpdate(float deltatime)
    {
        if (cangoon) if (game.inputManager.GetKeyDown(SFML.Window.Keyboard.Key.Enter)) game.LoadScene(ref game.sceneGame);
        cangoon = true;
    }
}

public struct LeaderboardEntry
{
    public string username;
    public int score;
    public DateTime dateTime;
    public string version;

    public LeaderboardEntry(string username, int score, DateTime dateTime, string version)
    {
        this.username = username;
        this.score = score;
        this.dateTime = dateTime;
        this.version = version;
    }

    public override string ToString()
    {
        return $"{username} {score} {dateTime} {version} ";
    }
}