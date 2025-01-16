/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

public class Waiter
{
    private float waitSeconds;
    private Action methodToCall;
    private float startTime;
    private bool realTime;
    private string name;

    public Waiter(float waitSeconds, string name, Action methodToCall, bool realTime = false)
    {
        this.waitSeconds = waitSeconds;
        this.name = name;
        this.methodToCall = methodToCall;
        this.startTime = realTime ? Game.gameTimeReal : Game.gameTime;
        this.realTime = realTime;
    }
    public void Update()
    {
        if (startTime + waitSeconds < (realTime ? Game.gameTimeReal : Game.gameTime))
        {
            methodToCall.Invoke();
            WaitManager.waiters.Remove(name);
        }
    }
}