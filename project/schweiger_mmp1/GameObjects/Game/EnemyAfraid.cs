public class EnemyAfraid : Enemy
{
    public EnemyAfraid(GameObjectHolder parent) : base(parent) { }

    public override void Spawn()
    {
        base.Spawn();
        health.maxhp = 4;
        health.SetHp();
    }

    public override void Move(float deltatime)
    {
        if (health.hp <= health.maxhp - 1)
        {
            movement.Move(-(player.Position - Position), deltatime);
            SoundManager.Instance.sounds["enemies/ahhh"].Volume = Utilities.Map(DistanceFromView() / 2000, 0, 1, Settings.soundVolumeVoice * .5f, 0);
            if (DistanceFromView() > 2000)
            {
                SoundManager.Instance.Stop("enemies/ahhh");
                Destroy();
            }
        }
        else
            base.Move(deltatime);
    }

    private float DistanceFromView()
    {
        var dx = MathF.Max(MathF.Max(-game.view.Size.X / 2 - Position.X, 0), Position.X - game.view.Size.X / 2);
        var dy = MathF.Max(MathF.Max(-game.view.Size.Y / 2 - Position.Y, 0), Position.Y - game.view.Size.Y / 2);
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    public override void TakeDamage(GameObject by, int damage)
    {
        base.TakeDamage(by, damage);
        if (health.hp == health.maxhp - damage)
        {
            movement.maxVelocity = 900;
            movement.maxAcceleration = 30;
            SoundManager.Instance.Play("enemies/ahhh");
        }
    }

    public override void Die()
    {
        player.MakeGiant();
        SoundManager.Instance.Stop("enemies/ahhh");
        base.Die();
        (scene as SceneGame)?.enemies.Remove((parent as Enemy)!);
        Destroy();
    }
}