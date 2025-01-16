/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using System.Diagnostics;

public class ScoreManager
{
    private static ScoreManager? instance;
    public static ScoreManager Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }
    private ScoreManager() { }

    private int currentScore = 0, currentMult = 0, finalScore = 0;
    private int calcScore;

    public float lastKill, timeToCountCombo = .3f;

    public float comboStarted = -10000, maxComboTime = 3, comboBarProgress;
    private float showChangeDuration = 1;

    public int score1, score2, score3;
    private HudGame uiManager;

    public void Initialize()
    {
        uiManager = (Game.Instance.sceneGame.hud as HudGame)!;
    }

    public void SetCurrentScore(int to)
    {
        if (uiManager.currentScoreText == null) return;
        if (currentScore == 0)
            comboStarted = Game.gameTime;

        if (to != 0)
        {
            comboStarted = Game.gameTime;
            uiManager.currentScoreText.SetDesiredScaleOffset(new(1, 1), 2, new(1.2f, 1.2f));
            uiManager.currentScoreText.SetDesiredPositionOffset(new(), 2, new(0, -50));
        }
        currentScore = to;
        uiManager.ChangeText(uiManager.currentScoreText, currentScore.ToString("N0", Settings.nfi));
    }

    public void ChangeCurrentScore(int add)
    {
        SoundManager.Instance.Play("points/point2", true);
        SetCurrentScore(currentScore + add);
    }

    public int GetCurrentScore() => currentScore;
    public void SetCurrentMult(int to)
    {
        if (uiManager.currentMultText == null) return;
        if (to == currentMult) return;
        if (to != 0)
        {
            comboStarted = Game.gameTime;
            uiManager.currentMultText.SetDesiredScaleOffset(new(1, 1), 2, new(1.2f, 1.2f));
            uiManager.currentMultText.SetDesiredPositionOffset(new(), 2, new(0, -50));
        }
        currentMult = to;
        uiManager.ChangeText(uiManager.currentMultText, currentMult.ToString("N0", Settings.nfi));
    }

    public void ChangeCurrentMult(int add)
    {
        if (currentScore > 0 && currentMult <= 0) add += 1;
        SetCurrentMult(currentMult + add);
        SoundManager.Instance.Play("points/point1", true);
    }

    public int GetCurrentMult() => currentMult;
    public void SetFinalScore(int to)
    {
        if (uiManager == null) return;
        if (uiManager.finalScoreText == null) return;
        if (to == finalScore) return;

        finalScore = to;
        uiManager.finalScoreText.SetDesiredScaleOffset(new(1, 1), 2, new(1.2f, 1.2f));
        uiManager.finalScoreText.SetDesiredPositionOffset(new(), 2, new(10, -15));
        uiManager.finalScoreText.SetDesiredRotationOffset(0, 2, 10, true);
        uiManager.ChangeText(uiManager.finalScoreText, finalScore.ToString("N0", Settings.nfi), Game.col_white);
    }

    public void ChangeFinalScore(int add) => SetFinalScore(finalScore + add);
    public int GetFinalScore() => finalScore;

    public void CalculateScore(int hp, bool instantly = false)
    {
        calcScore = currentScore * currentMult;
        switch (hp)
        {
            case 3:
                score1 += calcScore;
                break;
            case 2:
                score2 += calcScore;
                break;
            case 1:
                score3 += calcScore;
                break;
            default: break;
        }

        SetCurrentScore(0);
        SetCurrentMult(0);
        SoundManager.Instance.ResetPitch("points/point1");
        SoundManager.Instance.ResetPitch("points/point2");

        if (instantly)
        {
            ChangeFinalScore(calcScore);
            return;
        }

        if (calcScore <= 0) return;

        const int yeah4threshold = 10000, yeah3threshold = 1000, yeah2threshold = 100;

        if (calcScore >= yeah4threshold) SoundManager.Instance.Play("points/point_yeah4");
        if (calcScore >= yeah3threshold) SoundManager.Instance.Play("points/point_yeah3");
        else if (calcScore >= yeah2threshold) SoundManager.Instance.Play("points/point_yeah2");
        else SoundManager.Instance.Play("points/point_yeah1");

        uiManager.finalScoreText.SetDesiredScaleOffset(new(1, 1), 2, new(1.2f, 1.2f));
        uiManager.finalScoreText.SetDesiredPositionOffset(new(), 2, new(10, -15));
        uiManager.finalScoreText.SetDesiredRotationOffset(0, 2, 10, true);
        uiManager.ChangeText(uiManager.finalScoreText, $"+{calcScore.ToString("N0", Settings.nfi)}", Game.col_pink);

        WaitManager.Wait(showChangeDuration, "showFinalScore", () => ChangeFinalScore(calcScore));
    }

    public void Update()
    {
        if (Game.Instance.currentScene != Game.Instance.sceneGame) return;

        if (currentScore > 0)
        {
            SetCurrentMult(Math.Max(1, currentMult));
            comboBarProgress = 1 - (Game.gameTime - comboStarted) / maxComboTime;
            comboBarProgress = Math.Clamp(comboBarProgress, 0, 1);
            if (comboBarProgress <= 0) CalculateScore(Game.Instance.sceneGame.player.GetComponent<HealthPlayer>()!.hp);
            uiManager?.comboBar.SetProgress(comboBarProgress);
        }
        else
            uiManager?.comboBar.SetProgress(0);
    }

    public void Load()
    {
        score1 = 0; score2 = 0; score3 = 0;
        SetCurrentScore(0);
        SetCurrentMult(0);
        SetFinalScore(0);
        uiManager.ChangeText(uiManager.finalScoreText, $"0", Game.col_white);
    }

    public async void SubmitScoreToLeaderboard(string username, bool hide = false)
    {
        if (Settings.Instance.newestVersion == Settings.version && username == "" && !hide) return;
        Settings.username = username;
        Settings.Instance.WriteSettings();
        // Score above 1 -> submit
        // Outdated version -> dont submit
        // Username Leo -> submit eitherway
        if (Settings.Instance.newestVersion == Settings.version || username == "Leo")
            if (GetFinalScore() > 0)
                await PostgresAccess.Insert($"INSERT INTO leaderboard (player, score, date, ver, hide, combo1, combo2, combo3) VALUES ('{username}', {ScoreManager.Instance.GetFinalScore()}, '{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK")}', '{Settings.version}', '{hide}', '{Instance.score1}', '{Instance.score2}', '{Instance.score3}');", Config.connString_Leaderboard);
        Game.Instance.LoadScene(ref Game.Instance.sceneLeaderboard);
    }

    public async void PostOnImposter(string username)
    {
        if (username == "") return;

        string linktourl = "https://tinyurl.com/playlilguy";

        string post =
            $"{Settings.username} just reached a score of {GetFinalScore().ToString("N0", Settings.nfi)}!<br>" +
            $"Can you beat their score?<br>" +
            $"{linktourl}";

        await PostgresAccess.Insert(
            $"INSERT INTO posts (user_id, text) VALUES (2, '{post}')", 
            Config.connString_Imposter
        );
        string url = "https://users.multimediatechnology.at/~fhs50561/mmp1/Scripts/PHP/home.php";
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }
}