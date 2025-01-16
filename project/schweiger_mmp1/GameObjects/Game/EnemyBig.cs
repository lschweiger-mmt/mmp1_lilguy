public class EnemyBig : Enemy
{
    public EnemyBig(GameObjectHolder parent) : base(parent) { }

    public override void Spawn()
    {
        base.Spawn();
        Scale = new(2, 2);
        health.maxhp *= 5;
        health.SetHp();
        dontRespawn = true;
    }
    public override void Die()
    {
        for (int i = 0; i < 15; i++)
            ScoreManager.Instance.ChangeCurrentMult(1);
            
        PointEffect(15, Game.col_pink, 400);
        SoundManager.Instance.Play("effects/base");

        base.Die();
    }
}