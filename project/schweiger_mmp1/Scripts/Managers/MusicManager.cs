/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Audio;

public class MusicManager
{
    private static MusicManager? instance;
    public static MusicManager Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }
    private MusicManager() { }

    readonly Dictionary<string, Music> music = new();
    private const float fadeDuration = .2f;
    private float changedAt = 0;
    private Music oldTrack, newTrack;

    float tinitusMult = 1;

    private bool started = false;
    private float currentMusicVolume;

    public void Initialize()
    {
        music.Add("menu", AssetManager.Instance.music["menu"]);
        music.Add("einleben", AssetManager.Instance.music["einleben"]);
        music.Add("zweileben", AssetManager.Instance.music["zweileben"]);
        music.Add("dreileben", AssetManager.Instance.music["dreileben"]);

        foreach (var item in music)
        {
            item.Value.Volume = 0;
            item.Value.Loop = true;
        }
    }

    public void StartTrack(string track)
    {
        if (started) PlayMusic(track);
        else
        {
            newTrack = music[track];
            newTrack.Volume = currentMusicVolume;
            started = true;
            foreach (var item in music)
            {
                item.Value.Play();
            }
        }
    }

    public void Update()
    {
        float changefactor = (Game.gameTime - changedAt) / fadeDuration;
        if (changefactor <= 1)
        {
            if (oldTrack != null)
                oldTrack.Volume = (1 - changefactor) * currentMusicVolume;
            if (newTrack != null)
                newTrack.Volume = changefactor * currentMusicVolume;
        }
        else
        {
            if (oldTrack != null)
                oldTrack.Volume = 0;
            if (newTrack != null)
                newTrack.Volume = currentMusicVolume;
        }

        float expFactor = 1f - MathF.Exp(-Game.deltatime * .4f);
        tinitusMult += (1 - tinitusMult) * expFactor;

        if (newTrack != null) newTrack.Volume *= tinitusMult;
        if (oldTrack != null) oldTrack.Volume *= tinitusMult;
    }

    public void PlayMusic(string name)
    {
        if (!started) return;
        if (music[name] == newTrack) return;
        changedAt = Game.gameTime;
        foreach (var item in music)
        {
            if (item.Value.Volume > 0) oldTrack = item.Value;
        }
        newTrack = music[name];
    }

    public void LowerMusicVolumeImpact()
    {
        tinitusMult = 0f;
    }

    internal void Mute(bool v)
    {
        if (v) currentMusicVolume = 0;
        else currentMusicVolume = Settings.musicVolume;

        if (newTrack != null) newTrack.Volume = currentMusicVolume;
    }
}