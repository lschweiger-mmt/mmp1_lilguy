/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;

public class Animation
{
    public Animator animator;
    private bool looping;
    public string transitioningTo = "";

    public Animation(Animator animator, bool looping = true)
    {
        this.animator = animator;
        this.looping = looping;
    }

    public List<Tween> tweens { get; private set; }  = new();
    private int currentTween = 0;

    public void Update(float deltatime)
    {
        tweens[currentTween].Update(deltatime);
    }

    public void TweenDone(Tween tween)
    {
        if(!looping && (currentTween + 1) >= tweens.Count){
            if(transitioningTo != "") animator.SetPlayingAnimation(transitioningTo);
            else animator.Stop(0.35f / 2, "default");
            return;
        };
        currentTween = (currentTween + 1) % tweens.Count;
            tween.timeSincePlay = tween.timeOffset;
    }

    public void AddTween(float duration, Vector2f endPosition = new(), float endRotation = 0, EasingType easingType = EasingType.Linear, bool oneLoopingTween = false, float timeOffset = 0)
    {
        Tween tween = new Tween(
            tweens.Count > 0 ? tweens.Last().endPosition : Utilities.vZero, 
            endPosition, 
            tweens.Count > 0 ? tweens.Last().endRotation : 0, 
            endRotation, 
            duration, 
            easingType, 
            oneLoopingTween, 
            timeOffset
        );
        tween.animation = this;
        tweens.Add(tween);
    }
    
    public void AddTween(Tween tween)
    {
        tween.animation = this;
        tweens.Add(tween);
    }

    public void Reset(){
        currentTween = 0;
        foreach (var item in tweens)
        {
            item.timeSincePlay = 0;
        }
    }
}