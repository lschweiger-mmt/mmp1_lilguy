/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;
public class Sword : GameObject
{
    public Player player;

    public SwordStatus swordStatus = SwordStatus.Idle;
    public float slash1cooldown = .41f, slash2cooldown = .50f;
    private float startedSlashAt;
    private string lastSlashDirection = "";
    private bool bufferedHit = false;
    private const float SLASHUPTIME = .06f;
    private const float TIMEBETWEENCOMBO = .23f;

    public SpriteObject sword;
    public SpritesheetObject slash;
    public List<Collider> slashColliders;

    public Action slashAction;

    public Sword(GameObjectHolder parent, Transform? transform = null) : base(parent, transform)
    {
        slash = new SpritesheetObject(this, AssetManager.Instance.textures["slash_spritesheet"], new(11, 1), 60, false);
        sword = new SpriteObject(this, AssetManager.Instance.textures["sword/sword"], new Transform(new Vector2f(243, 0))) { localOrigin = new(0, 151) };
        SetupSlashColliders();
    }

    public override void LateInitialize()
    {
        player = game.sceneGame.player;
        SetupAnimation();
    }

    public void Load()
    {
        lastSlashDirection = "";
        bufferedHit = false;
        SetSwordStatus(SwordStatus.Idle);
        WaitManager.StopWait("setwalkablefalse");
        WaitManager.StopWait("setslashcolliderstrue");
        WaitManager.StopWait("resetswordstatus");
        WaitManager.StopWait("resetheadsprite");
        WaitManager.StopWait("disableSlashGameObject");

        slash.SetActive(false);

        slash.GetComponent<Animator>()?.Stop(0);
        sword.GetComponent<Animator>()?.Stop(0);
        slash.GetComponent<Animator>()?.SetPlayingAnimation("default");
        sword.GetComponent<Animator>()?.SetPlayingAnimation("default");
    }

    private void SetupAnimation()
    {
        new Animator(slash, new Tween());
        new Animator(sword, TweenLibrary.Bobbing(new(0, 0), new(0, 15), startRotation: 20, endRotation: 25, duration: 2));

        SetupSwordAnimation.SetupSwordAnimations(this);
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
        if (bufferedHit && lastSlashDirection != "")
        {
            SlashLogic(lastSlashDirection);
            return;
        }
        if (InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.K) || InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.Down))
            SlashLogic("down");
        if (InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.I) || InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.Up))
            SlashLogic("up");
        if (InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.J) || InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.Left))
            SlashLogic("left");
        if (InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.L) || InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.Right))
            SlashLogic("right");
    }

    public void SlashLogic(string dir)
    {
        HealthPlayer? playerHealth = player.GetComponent<HealthPlayer>();
        if (playerHealth != null)
            if (playerHealth.knockedOut || playerHealth.dead) return;

        if (swordStatus == SwordStatus.Idle)
        {
            lastSlashDirection = dir;
            SlashExecution(dir);
            WaitManager.Wait(slash1cooldown, "resetswordstatus", () => SetSwordStatus(SwordStatus.Idle));
            SoundManager.Instance.Play("slash1");
            SoundManager.Instance.PlayRandomWeh();

            SetSwordStatus(SwordStatus.slash0);
            PlayAnimation("slash0" + dir);
        }
        else if (swordStatus == SwordStatus.slash0 && dir == lastSlashDirection)
        {
            WaitManager.StopWait("resetswordstatus");
            if (startedSlashAt + TIMEBETWEENCOMBO <= Game.gameTime)
            {
                lastSlashDirection = "";
                bufferedHit = false;
                SlashExecution(dir);
                SoundManager.Instance.Play("slash2");
                SoundManager.Instance.PlayRandomWeh();
                SetSwordStatus(SwordStatus.slash1);
                PlayAnimation("slash1" + dir);
            }
            else
            {
                bufferedHit = true;
            }
        }
    }

    private void SlashExecution(string dir)
    {
        slashAction?.Invoke();

        player.GetComponent<Movement>()?.ApplyForce(StringToDir(dir), 20000, .1f); // dash in slash direction
        // player.GetComponent<Movement>().ApplyForce(30000, .1f);// dash in movement direction
        startedSlashAt = Game.gameTime;
        SetSlashColliders(false);
        WaitManager.Wait(SLASHUPTIME / 2, "setslashcolliderstrue", () => SetSlashColliders(true)); // enable colliders a little bit later
        WaitManager.Wait(slash1cooldown, "resetswordstatus", () => SetSwordStatus(SwordStatus.Idle));
        WaitManager.Wait(SLASHUPTIME, "setslashcollidersfalse", () => SetSlashColliders(false));
    }

    public void SetSlashColliders(bool to)
    {
        foreach (var item in slashColliders)
            item.SetActive(to);
    }

    void SetSwordStatus(SwordStatus status)
    {
        swordStatus = status;
        if (status == SwordStatus.Idle)
        {
            player?.GetComponent<MovementPlayer>()?.SetActive(true);
        }
        else
        {
            WaitManager.Wait(.11f, "setwalkablefalse", () => player.GetComponent<MovementPlayer>()?.SetAcceptsInput(false));
            WaitManager.Wait(.40f, "setwalkabletrue", () => player.GetComponent<MovementPlayer>()?.SetAcceptsInput(true));
        }
    }

    private void PlayAnimation(string anim)
    {
        sword.GetComponent<Animator>()?.SetPlayingAnimation(anim);
        if (player == null) return;

        slash.SetActive(true);
        slash.GetComponent<Animator>()?.Stop(0);
        slash.GetComponent<Animator>()?.SetPlayingAnimation(anim);
        slash.GetComponent<Spritesheet>()?.Play();
        player.hand.GetComponent<Animator>()?.SetPlayingAnimation(anim);
        player.head.GetComponent<Animator>()?.SetPlayingAnimation(anim);

        // Flip Slash when hitting backwards
        if (anim.Contains("1"))
            slash.localScale = new(-1f, 1);
        else
            slash.localScale = new(1, 1);

        // Change Player Head Sprite
        if (anim.Contains("down") || anim.Contains("left") || anim.Contains("right"))
            player.head.GetComponent<SpriteRenderer>()?.SetTexture(AssetManager.Instance.textures["lilguy/head_evil"]);
        else if (anim.Contains("up"))
            player.head.GetComponent<SpriteRenderer>()?.SetTexture(AssetManager.Instance.textures["lilguy/head_back"]);

        WaitManager.Wait(.4f, "resetheadsprite", () => player.head.GetComponent<SpriteRenderer>()?.SetTexture(AssetManager.Instance.textures["lilguy/head"]));
        WaitManager.Wait(.2f, "disableSlashGameObject", () => slash.SetActive(false));
    }

    private void SetupSlashColliders()
    {
        slash.SetActive(false);

        new CircleCollider(slash, "playersword", 210, new(300, 10));
        new CircleCollider(slash, "playersword", 250, new(0, 30));
        new CircleCollider(slash, "playersword", 210, new(-300, 10));

        slashColliders = slash.GetComponents<Collider>();
    }

    private Vector2f StringToDir(string dir)
    {
        return dir switch
        {
            "right" => new(1, 0),
            "down" => new(0, 1),
            "left" => new(-1, 0),
            "up" => new(0, -1),
            _ => new(0, 0)
        };
    }

    private string DirToString(Vector2f dir)
    {
        dir.Normalize();
        if (dir == new Vector2f(1, 0)) return "right";
        if (dir == new Vector2f(0, 1)) return "down";
        if (dir == new Vector2f(-1, 0)) return "left";
        if (dir == new Vector2f(0, -1)) return "up";
        return "";
    }

    public enum SwordStatus
    {
        Idle,
        slash0,
        slash1
    }
}