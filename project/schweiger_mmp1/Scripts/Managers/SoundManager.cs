/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Audio;

public class SoundManager
{
    private static SoundManager? instance;
    public static SoundManager Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }
    private SoundManager() { }

    public Dictionary<string, Sound> sounds = new();
    public List<Sound> wehSounds = new(), ughSounds = new(), ahSounds = new();
    private float maxPointIncrement = .005f, pointIncrement = .005f, pointReducer = .99f;

    private const float minTimeBetweenPoints = .06f;
    private Dictionary<string, int> pointQueue = new();
    Dictionary<string, float> lastPoints = new();

    Random rand = new Random();


    public void Initialize()
    {
        foreach (var item in AssetManager.Instance.soundBuffers)
            sounds.Add(item.Key, new Sound(item.Value));


        // Lilguy
        sounds["slash1"].Volume = Settings.soundVolumeWeapons;
        sounds["slash2"].Volume = Settings.soundVolumeWeapons;

        sounds["tinitus"].Volume = Settings.soundVolumeWeapons * 0.8f;

        for (int i = 1; i <= 5; i++)
        {
            string sound = $"lilguy/weh{i}";
            wehSounds.Add(sounds[sound]);
            sounds[sound].Volume = Settings.soundVolumeVoice;
        }

        for (int i = 1; i <= 4; i++)
        {
            string sound = $"lilguy/ugh{i}";
            ughSounds.Add(sounds[sound]);
            sounds[sound].Volume = Settings.soundVolumeVoice;
        }

        sounds["lilguy/maumau"].Volume = Settings.soundVolumeVoice;

        // Enemies

        for (int i = 1; i <= 5; i++)
        {
            string sound = $"enemies/ah{i}";
            ahSounds.Add(sounds[sound]);
            sounds[sound].Volume = Settings.soundVolumeVoice * .36f;
        }
        sounds["enemies/ahhh"].Volume = Settings.soundVolumeVoice * .5f;

        // Points

        for (int i = 1; i <= 2; i++)
        {
            string sound = $"points/point{i}";
            ResetPitch(sound);
            pointQueue.Add(sound, 0);
            lastPoints.Add(sound, 0);
            sounds[sound].Volume = Settings.soundVolumePoints;
        }

        for (int i = 1; i <= 4; i++)
        {
            string sound = $"points/point_yeah{i}";
            sounds[sound].Volume = Settings.soundVolumePoints * (2f + i);
        }

        // UI

        for (int i = 1; i <= 3; i++)
            sounds[$"UI/click{i}"].Volume = Settings.soundVolumeWeapons;

        // Rest

        sounds["effects/base"].Volume = Settings.soundVolumeWeapons;
        sounds["effects/baseup"].Volume = Settings.soundVolumeWeapons;
    }

    public void Update()
    {
        foreach (var item in pointQueue)
        {
            if (item.Value > 0 && Game.gameTime >= lastPoints[item.Key] + minTimeBetweenPoints)
            {
                Play(item.Key, true);
                pointQueue[item.Key]--;
            }
        }
    }

    public void Play(string sound, bool increment = false, bool randomPitch = false)
    {
        if (CheckQueue(sound, "points/point1") && CheckQueue(sound, "points/point2"))
        {
            if (!increment && randomPitch && sounds[sound].Pitch > .8f)
                sounds[sound].Pitch = rand.Next(85, 115) / 100f;
            sounds[sound].Play();
        }

        if (sound == "points/point_yeah3" || sound == "points/point_yeah4")
            MusicManager.Instance.LowerMusicVolumeImpact();

        if (increment)
        {
            sounds[sound].Pitch += pointIncrement;
            pointIncrement *= pointReducer;
        }
    }

    public void Stop(string sound) => sounds[sound].Stop();

    public void PlayRandomWeh()
    {
        int soundToPlay = rand.Next(wehSounds.Count) + 1;
        Play($"lilguy/weh{soundToPlay}", false, true);
    }

    public void PlayRandomAh()
    {
        const float ahProbablitiy = .4f;
        if (rand.NextDouble() > ahProbablitiy) return;
        int soundToPlay = rand.Next(ahSounds.Count) + 1;
        Play($"enemies/ah{soundToPlay}", false, true);
    }

    public void PlayRandomUgh()
    {
        int soundToPlay = rand.Next(ughSounds.Count) + 1;
        Play($"lilguy/ugh{soundToPlay}", false, true);
    }

    private bool CheckQueue(string sound, string queuingSound)
    {
        if (sound == queuingSound)
        {
            if (Game.gameTime < lastPoints[queuingSound] + minTimeBetweenPoints)
            {
                pointQueue[queuingSound]++;
                return false;
            }
            lastPoints[queuingSound] = Game.gameTime;
        }
        return true;
    }

    public void ResetPitch(string sound)
    {
        pointIncrement = maxPointIncrement;
        sounds[sound].Pitch = 1 - pointIncrement;
    }
}