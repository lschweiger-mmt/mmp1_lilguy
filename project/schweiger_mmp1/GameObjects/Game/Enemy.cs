/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */



using SFML.Graphics;

public class Enemy : GameObject
{
    private const int pointValue = 1;
    private const int multValue = 1;
    public Collider collider;
    public Movement movement;
    public Player player;
    public SpriteObject sprite;
    public HealthEnemy health;

    public bool dontRespawn;

    public Enemy(GameObjectHolder parent, bool dontRespawn = false) : base(parent)
    {
        player = game.sceneGame.player;

        sprite = new SpriteObject(this, AssetManager.Instance.textures["enemy/enemy"]);
        const float spriteCenterOffset = 169.5f;
        sprite.localOrigin = new(0, spriteCenterOffset);

        const int jumpAnimationHeight = -38;
        const float jumpAnimationDuration = 0.4f;
        Tween jumpingTween = new Tween(new(0, 0), new(0, jumpAnimationHeight), 0, 0, jumpAnimationDuration, EasingType.QuadraticBounce, false);
        new Animator(sprite, jumpingTween);

        const int hitboxRadius = 140;
        collider = new CircleCollider(this, "enemy", hitboxRadius, new(0, -spriteCenterOffset));
        collider.collided += Collided;
        new RigidBodyCircle(this);

        const float maxAcceleration = 2f;
        const int maxSpeed = 600;
        const float friction = 1f;
        movement = new Movement(this, maxAcceleration, maxSpeed, friction);


        const int maxHealth = 2;
        const float iFrameDuration = .15f;
        const float knockedOutDuration = .3f;
        health = new HealthEnemy(this, maxHealth, iFrameDuration, AssetManager.Instance.textures["enemy/enemy_frame"], knockedOutDuration);

        new ParticleSystem(this);

        this.dontRespawn = dontRespawn;
        Spawn();
    }

    public virtual void Spawn()
    {
        Random rand = new Random();
        const float spawnDistanceFromCenter = 4800;
        const int spawnDistanceVarietyPercent = 130;
        Position = Utilities.RotateVector(Utilities.vRight * spawnDistanceFromCenter * rand.Next(100, spawnDistanceVarietyPercent) / 100, rand.Next(0, 360));

        const int scaleVariationPercentMin = 92;
        const int scaleVariationPercentMax = 115;
        Scale = Utilities.vOne * rand.Next(scaleVariationPercentMin, scaleVariationPercentMax) / 100f;
        health?.Load();
    }

    public virtual void TakeDamage(GameObject by, int damage)
    {
        const int knockbackAmount = 500;
        const float knockbackDuration = .02f;
        GetComponent<Movement>()?.ApplyForce(-(by.Position - Position), knockbackAmount, knockbackDuration);

        BloodEffect(3200, 30);

        SoundManager.Instance.PlayRandomAh();
    }
    public virtual void Die()
    {
        BloodEffect(300,360);

        ScoreManager.Instance.ChangeCurrentScore(pointValue);

        if (ScoreManager.Instance.lastKill + ScoreManager.Instance.timeToCountCombo > Game.gameTime)
        {
            PointEffect(pointValue, Game.col_pink, 220);
            ScoreManager.Instance.ChangeCurrentMult(multValue);
        }

        ScoreManager.Instance.lastKill = Game.gameTime;

        Spawn();
        if (dontRespawn) Destroy(true);
    }

    private void BloodEffect(float bloodSpeed, int bloodSpread)
    {
        const int bloodCount = 12;
        const int bloodScale = 100;
        const float bloodLifetime = .26f;

        GetComponent<ParticleSystem>()?.Emit(
            GetComponent<CircleCollider>()!.center,
            bloodCount,
            bloodScale * Scale.X,
            Game.col_pink,
            bloodLifetime,
            bloodSpeed,
            game.sceneGame.player.Position - GetComponent<CircleCollider>()!.center,
            bloodSpread);
    }

    public void PointEffect(int points, Color col, uint size = 140)
    {
        GetComponent<ParticleSystem>()?.EmitText(points.ToString(), GetComponent<CircleCollider>()!.center, col, size);
    }

    private void Collided(Collider e)
    {
        if (e.tag == "playersword")
            health?.Hit(Game.Instance.sceneGame.player);
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
        Move(deltatime);
    }

    public virtual void Move(float deltatime)
    {
        movement.Move(player.Position - Position, deltatime);
    }

    public new void Destroy(bool wait = false)
    {
        collider.collided -= Collided;
        (scene as SceneGame)?.enemies.Remove((parent as Enemy)!);
        if (wait)
        {
            const float waitTimeBeforeDestroy = 1.5f;
            WaitManager.Wait(waitTimeBeforeDestroy, base.Destroy);
        }
        else base.Destroy();
    }
}
