public static class WaitManager
{
    public static Dictionary<string, Waiter> waiters = new();

    /// <summary>
    /// Call action after waitSeconds seconds
    /// </summary>
    public static void Wait(float waitSeconds, string name, Action methodToCall, bool realTime = false)
    {
        if (waiters.ContainsKey(name)) StopWait(name);
        waiters.Add(name, new Waiter(waitSeconds, name, methodToCall, realTime));
    }

    /// <summary>
    /// Call action after waitSeconds seconds
    /// </summary>
    public static void Wait(float waitSeconds, Action methodToCall, bool realTime = false)
    {
        string name = Utilities.RandomString(30);
        if (waiters.ContainsKey(name)) StopWait(name);
        waiters.Add(name, new Waiter(waitSeconds, name, methodToCall, realTime));
    }

    public static void StopWait(string name) => waiters.Remove(name);
}