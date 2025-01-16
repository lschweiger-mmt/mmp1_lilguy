/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;

public class Health : Component
{
    public int hp, maxhp;

    protected float iFrameDuration; protected float knockedOutDuration;
    public bool iFrames = false; public bool knockedOut = false;
    protected Texture? hitTexture;

    public event EventHandler<int> hit;
    public Action die;
    public bool dead = false, invulnerable = false;

    public Health(GameObject parent, int maxhp, float iFrameDuration, Texture? hitTexture = null, float? knockedOutDuration = null) : base(parent)
    {
        this.maxhp = maxhp;
        this.iFrameDuration = iFrameDuration;
        this.hitTexture = hitTexture;
        hp = maxhp;
        this.knockedOutDuration = knockedOutDuration == null ? iFrameDuration : (float)knockedOutDuration;
    }

    public void Load()
    {
        hp = maxhp;
        dead = false;
        iFrames = false;
        knockedOut = false;
    }

    public void Hit(GameObject by)
    {
        if (invulnerable) return;
        if (!iFrames && !dead)
        {
            iFrames = true;
            knockedOut = true;
            TakeDamage(by);
            (parent as Player)?.BloodEffect();

            WaitManager.Wait(iFrameDuration, () => iFrames = false);
            WaitManager.Wait(knockedOutDuration, () => knockedOut = false);
        }
    }

    public virtual void TakeDamage(GameObject by, int damage = 1)
    {
        hit?.Invoke(this, hp - damage);
        if (invulnerable) return;
        hp -= damage;

        IFrames();

        if (hp <= 0) Die();
    }

    public virtual void IFrames()
    {
        if (parent.children[0].GetComponent(out SpriteRenderer? sr) && hitTexture != null)
        {
            Texture changeToTexture = sr!.GetTexture();
            if (this is HealthEnemy) changeToTexture = (this as HealthEnemy)!.GetTexture(hp);
            sr!.SetTexture(hitTexture);
            WaitManager.Wait(iFrameDuration, () => sr!.SetTexture(changeToTexture));
        }
    }

    public virtual void Die()
    {
        if (dead) return;
        dead = true;
        die?.Invoke();
    }

    public virtual void SetHp(int? to = null)
    {
        if (to == null) to = maxhp;
        hp = (int)to;
        if (this is HealthPlayer) (Game.Instance.sceneGame.hud as HudGame)?.SetHp(null, hp);
    }
}