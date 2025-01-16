/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class Spritesheet : SpriteRenderer
{
    public List<Animation> animations = new();

    private int currentAnimation = 0;
    private float animationTime;
    private float currentAnimationSpeed = 1;

    public bool looping = false;
    private bool playing = false;

    public Spritesheet(GameObject parent, Texture texture, Vector2i dimensions, float framesPerSecond, bool looping = true) : base(parent, texture)
    {
        List<int> lengths = new();
        for (int i = 0; i < dimensions.Y; i++)
            lengths.Add(dimensions.X);

        Construct(texture, dimensions, framesPerSecond, lengths, looping);
    }
    public Spritesheet(GameObject parent, Texture texture, Vector2i dimensions, float framesPerSecond, List<int> lengths, bool looping = true) : base(parent, texture)
    {
        Construct(texture, dimensions, framesPerSecond, lengths, looping);
    }

    private void Construct(Texture tex, Vector2i dimensions, float framesPerSecond, List<int> lengths, bool looping = false)
    {
        this.looping = looping;
        playing = looping;
        int width = (int)(tex.Size.X / dimensions.X);
        int height = (int)(tex.Size.Y / dimensions.Y);

        // Calculates intrects of all the frames and adds them to the animations list
        for (int j = 0; j < dimensions.Y; j++)
        {
            List<IntRect> tempList = new();
            for (int i = 0; i < dimensions.X; i++)
                tempList.Add(new IntRect(new Vector2i(i * width, j * height), new Vector2i(width, height)));

            animations.Add(new Animation(this, tempList, framesPerSecond, lengths[j]));
        }
        Animate(0);
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
        if (playing)
            Animate(deltatime);
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        if (playing)
            base.Draw(target, states);
    }

    public void ChangeAnimation(int to) => currentAnimation = to;
    public void ChangeAnimationSpeed(float mult) => currentAnimationSpeed = mult;

    public virtual void Animate(float deltatime)
    {
        animationTime += deltatime * currentAnimationSpeed;
        sprite!.TextureRect = animations[currentAnimation].Animate(animationTime);
        sprite.Origin = new Vector2f(sprite.TextureRect.Width / 2, sprite.TextureRect.Height / 2);
    }

    public void Play()
    {
        animationTime = 0;
        playing = true;
    }

    public void Stop()
    {
        playing = false;
    }

    public class Animation
    {
        public Spritesheet spritesheet;
        public List<IntRect> rects = new();
        float animationSpeed;
        int frameCount;

        public Animation(Spritesheet spritesheet, List<IntRect> rects, float animationSpeed, int frameCount)
        {
            this.spritesheet = spritesheet;
            this.rects = rects;
            this.animationSpeed = animationSpeed;
            this.frameCount = frameCount;
        }

        /// <summary>
        /// Returns the according IntRect for every frame this is called
        /// </summary>
        public IntRect Animate(float animationTime)
        {
            if ((animationTime * animationSpeed) >= frameCount && !spritesheet.looping)
            {
                spritesheet.Stop();
            }
            return rects[(int)(animationTime * animationSpeed % frameCount)];
        }
    }
}