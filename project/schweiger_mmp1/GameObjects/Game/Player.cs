/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Audio;
using SFML.System;

public class Player : GameObject
{
    public SpriteObject body, head, hand, hand2;
    public Sword sword;
    public bool giant = false;

    public Player(GameObjectHolder parent, Transform? transform = null) : base(parent, transform)
    {
        localOrigin = new Vector2f(0, 106);

        SetupComponents();
        SetupChildren();
        SetupAnimation();

        WaitManager.Wait(1, Blink);
    }

    internal void Load()
    {
        Position = new();
        GetComponent<HealthPlayer>()?.Load();
        GetComponent<HealthPlayer>()?.SetHp();
        GetComponent<MovementPlayer>()?.LoadAccelartionAndVelocity();
        sword.Load();
        body.GetComponent<Animator>()?.Stop(0);
        head.GetComponent<Animator>()?.Stop(0);
        hand.GetComponent<Animator>()?.Stop(0);
        hand2.GetComponent<Animator>()?.Stop(0);
        sword.GetComponent<Animator>()?.Stop(0);

        head.GetComponent<SpriteRenderer>()?.SetTexture(AssetManager.Instance.textures["lilguy/head"]);

        head.GetComponent<Animator>()?.SetPlayingAnimation("default");
        hand.GetComponent<Animator>()?.SetPlayingAnimation("default");
        hand2.GetComponent<Animator>()?.SetPlayingAnimation("default");
        sword.GetComponent<Animator>()?.SetPlayingAnimation("default");

        MakeNormal();
        WaitManager.StopWait("makenormal");
        Scale = new(1, 1);
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);

        float expFactor = 1f - MathF.Exp(-deltatime * 5);
        Scale += ((giant ? new(2.4f, 2.4f) : new(1, 1)) - Scale) * expFactor;

        // Hide score panel when near it
        if (game.sceneGame.hud != null && Game.gameTime > .1)
        {
            HudGame ui = (game.sceneGame.hud as HudGame)!;
            float spacingSide = 750, spacingBottom = 800;
            if (
                Position.Y < ui.scorePanel.Position.Y + (HudGame.panelHeight - ui.scorePanel.GetCurrentPositionOffsetFromAllParents().Y + spacingBottom) * Hud.uiScale
                && Position.X < ((HudGame.panelWidth / 2) + spacingSide) * Hud.uiScale && Position.X > ((-HudGame.panelWidth / 2) - spacingSide) * Hud.uiScale
            )
            {
                ui.scorePanel.SetDesiredPositionOffset(new(0, -(HudGame.panelHeight + HudGame.spacing * 1.5f)), 30);
                ui.scorePanel.dontAnimate = true;
            }
            else
            {
                ui.scorePanel.SetDesiredPositionOffset(new(), 8);
                ui.scorePanel.dontAnimate = false;
            }
        }
    }

    private void SetupComponents()
    {
        new HealthPlayer(this, 3, 1.6f, .3f);
        CircleCollider collider = new CircleCollider(this, "player", 110, new(0, -130));
        collider.collided += Collide;
        new MovementPlayer(this, 20000, 1800, .93f);
        new RigidBodyCircle(this, true);

        new ParticleSystem(this);

        // shadow beneath player, not happy really
        // new SpriteRenderer(this, AssetManager.Instance.textures["lilguy/shadow"]);
    }

    private void SetupChildren()
    {
        body = new SpriteObject(this, AssetManager.Instance.textures["lilguy/body"], new Transform(new(0, -53)));
        hand = new SpriteObject(body, AssetManager.Instance.textures["lilguy/hand"], new Transform(new Vector2f(-230, 0)));
        hand2 = new SpriteObject(body, AssetManager.Instance.textures["lilguy/hand"], new Transform(new Vector2f(230, 0), localScale: new())); // set scale to 0 to hide second hand
        sword = new Sword(body, transform: new Transform(localScale: new(1, 1))); //set scale to 0 to hide sword (screenshots)
        head = new SpriteObject(body, AssetManager.Instance.textures["lilguy/head"], new Transform(new Vector2f(0, -170)));
        sword.slashAction += Slash;
    }


    private void SetupAnimation()
    {
        Tween jumpingTween = new Tween(new(0, -53), new(0, -65 - 53), 0, 0, 0.33f, EasingType.QuadraticBounce, false);
        new Animator(body, jumpingTween);
        new Animator(head, TweenLibrary.Bobbing(new(0, 0), new(0, 15), duration: 2, offset: .4f));
        new Animator(hand, TweenLibrary.Bobbing(new(0, 0), new(0, 15), duration: 2));
        new Animator(hand2, TweenLibrary.Bobbing(new(0, 0), new(0, 15), duration: 2));
    }

    private void Collide(Collider other)
    {
        if (other.tag == "enemy")
            if (!other.parent.GetComponent<HealthEnemy>()!.knockedOut)
                GetComponent<HealthPlayer>()?.Hit(other.parent);
    }

    private void Blink()
    {
        if (head.spriteRenderer.GetTexture() == AssetManager.Instance.textures["lilguy/head"])
        {
            head.spriteRenderer.SetTexture(AssetManager.Instance.textures["lilguy/head_blink"]);
            WaitManager.Wait(.16f, () => head.spriteRenderer.SetTexture(AssetManager.Instance.textures["lilguy/head"]));
        }
        WaitManager.Wait(4, Blink);
    }

    public override void Destroy()
    {
        GetComponent<CircleCollider>()!.collided -= Collide;
        base.Destroy();
    }


    private void Slash()
    {
        //slash particles, not really happy right now
        // GetComponent<ParticleSystem>()?.Emit(Position + GetComponent<CircleCollider>()!.offset, 3, 60, Game.col_purple, .15f, 500, Utilities.vUp, 150, gravity: false, stayonground: false);
    }

    public void MakeGiant()
    {
        const float giantDuration = 6.2f * 1.5f;
        SoundManager.Instance.sounds["slash1"].Pitch = .8f;
        SoundManager.Instance.sounds["slash2"].Pitch = .8f;
        foreach (var sound in SoundManager.Instance.wehSounds) sound.Pitch = .9f;
        foreach (var sound in SoundManager.Instance.ughSounds) sound.Pitch = .9f;
        MusicManager.Instance.PlayMusic("einleben");
        // MusicManager.Instance.Tinitus();
        SoundManager.Instance.Play("effects/baseup");
        WaitManager.Wait(giantDuration, "makenormal", MakeNormal);
        for (int i = 0; i < 30; i++)
        {
            game.sceneGame.enemySpawner.SpawnEnemy(false, true);
        }
        GetComponent<HealthPlayer>()!.invulnerable = true;
        giant = true;
    }
    public void MakeNormal()
    {
        SoundManager.Instance.sounds["slash1"].Pitch = 1.0f;
        SoundManager.Instance.sounds["slash2"].Pitch = 1.0f;
        foreach (var sound in SoundManager.Instance.wehSounds) sound.Pitch = 1f;
        foreach (var sound in SoundManager.Instance.ughSounds) sound.Pitch = 1f;

        GetComponent<HealthPlayer>()!.invulnerable = false;
        giant = false;
        MusicManager.Instance.PlayMusic(GetComponent<HealthPlayer>()!.GetMusicTrack());
    }

    public void BloodEffect()
    {
        const int bloodCount = 42;
        const int bloodScale = 70;
        const float bloodLifetime = .66f;
        const float bloodSpeed = 2800f;

        GetComponent<ParticleSystem>()?.Emit(
            GetComponent<CircleCollider>()!.center,
            bloodCount,
            bloodScale * Scale.X,
            Game.col_pink,
            bloodLifetime,
            bloodSpeed);
    }

}