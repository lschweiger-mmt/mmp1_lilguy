/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;

public class Animator : Component
{
    private Dictionary<string, Animation> animations = new();
    string playingAnimation = "";
    public Vector2f originalPosition;
    public float originalRotation;

    private bool stopped = false;

    public Animator(GameObject parent) : base(parent)
    {
        originalPosition = parent.localPosition;
        originalRotation = parent.localRotation;
        SetPlayingAnimation("default");
    }
    public Animator(GameObject parent, Tween tween) : base(parent)
    {
        originalPosition = parent.localPosition;
        originalRotation = parent.localRotation;

        Animation defaultAnim = new Animation(this);
        defaultAnim.AddTween(tween);
        AddAnimation("default", defaultAnim);
        SetPlayingAnimation("default");
    }
    public override void Update(float deltatime)
    {
        if (animations.ContainsKey(playingAnimation))
            animations[playingAnimation].Update(deltatime);
        else
        {
            parent.localPosition = originalPosition;
            parent.localRotation = originalRotation;
        }
    }

    public void AddAnimation(string name, Animation animation)
    {
        animations.Add(name, animation);
    }
    public void AddAnimation(string name, Tween tween)
    {
        Animation anim = new Animation(this);
        anim.AddTween(tween);
        animations.Add(name, anim);
    }

    public void SetPlayingAnimation(string name)
    {
        if (!animations.ContainsKey(name)) return;
        if (playingAnimation == name) return;
        if (name != "original") stopped = false;

        foreach (var item in animations)
        {
            item.Value.Reset();
        }
        playingAnimation = name;
    }

    public void Stop(float duration = 0, string to = "")
    {
        if (stopped) return;

        stopped = true;
        animations.Remove("original");
        Animation original = new Animation(this, false);

        original.AddTween(
            to == "" ?
            new Tween(parent.localPosition - originalPosition, originalPosition, parent.localRotation, originalRotation, duration) :
            new Tween(parent.localPosition - originalPosition, animations[to].tweens[0].startPosition, parent.localRotation - originalRotation, animations[to].tweens[0].startRotation, duration)
        );

        original.transitioningTo = to!;

        AddAnimation("original", original);
        SetPlayingAnimation("original");
    }

    public Animation GetAnimation(string anim) => animations[anim];
}