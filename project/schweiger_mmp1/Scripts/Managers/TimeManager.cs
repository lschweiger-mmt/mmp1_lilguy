/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

public class TimeManager
{
    private static TimeManager? instance;
    public static TimeManager Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }
    private TimeManager() { }
    public float timeScale = 1;
    public float returnSpeed = .6f;

    public void Update(float otherDeltatime)
    {
        float expFactor = 1f - MathF.Exp(-otherDeltatime * returnSpeed);
        timeScale += (1 - timeScale) * expFactor;
    }

    public void SetTimeScale(float to)
    {
        timeScale = to;
    }
}