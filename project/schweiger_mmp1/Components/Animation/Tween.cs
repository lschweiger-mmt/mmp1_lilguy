/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;

public class Tween
{
    public Animation animation;
    private float duration;
    public Vector2f startPosition { get; private set; }
    public Vector2f endPosition;
    public float startRotation { get; private set; }
    public float endRotation;
    private EasingType easingType;

    public float timeSincePlay;
    private bool singleGlobalTiming;
    public float timeOffset;

    public Tween(Vector2f? startPosition = null, Vector2f? endPosition = null, float startRotation = 0, float endRotation = 0, float duration = 0, EasingType easingType = EasingType.Linear, bool singleGlobalTiming = false, float timeOffset = 0)
    {
        this.startPosition = startPosition == null ? new(0, 0) : (Vector2f)startPosition;
        this.endPosition = endPosition == null ? new(0, 0) : (Vector2f)endPosition;
        this.duration = duration;
        this.easingType = easingType;
        this.startRotation = startRotation;
        this.endRotation = endRotation;
        this.singleGlobalTiming = singleGlobalTiming;
        this.timeOffset = timeOffset;
    }

    public void Update(float deltaTime)
    {
        if (singleGlobalTiming) timeSincePlay = (timeOffset + Game.gameTime) % duration;
        timeSincePlay += deltaTime;

        float progress = MathF.Min(timeSincePlay / duration, 1.0f);

        // Apply easing function
        float easedProgress = Utilities.EasingFunction(progress, easingType);

        Vector2f posChange = Utilities.Lerp(startPosition, endPosition, easedProgress);
        animation.animator.parent.localPosition = animation.animator.originalPosition + posChange;

        float rotChange = Utilities.Lerp(startRotation, endRotation, easedProgress);
        animation.animator.parent.localRotation = animation.animator.originalRotation + rotChange;


        if (progress >= .99999f && !singleGlobalTiming)
            animation.TweenDone(this);
    }
}


public static class TweenLibrary
{
    public static Tween Bobbing(Vector2f startPos, Vector2f endPos, float startRotation = 0, float endRotation = 0, float duration = 2, float offset = 0)
    {
        return new Tween(startPos, endPos, startRotation, endRotation, duration, EasingType.Cos, true, offset);
    }
}

