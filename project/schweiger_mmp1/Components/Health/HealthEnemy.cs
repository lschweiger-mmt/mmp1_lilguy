/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;

public class HealthEnemy : Health
{
    public new Enemy parent;


    public HealthEnemy(Enemy parent, int maxhp, float iFrameDuration, Texture? hitTexture = null, float? knockedOutDuration = null) : base(parent, maxhp, iFrameDuration, hitTexture, knockedOutDuration)
    {
        this.parent = parent;
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
        if (!iFrames)
            parent.children[0].GetComponent<Animator>()?.SetPlayingAnimation("default");
        else
            parent.children[0].GetComponent<Animator>()?.Stop(.1f);
    }

    public override void TakeDamage(GameObject by, int damage = 1)
    {
        if (invulnerable) return;

        base.TakeDamage(by, damage);
        parent.TakeDamage(by, damage);
    }

    public override void Die()
    {
        base.Die();
        parent.Die();
    }

    internal Texture GetTexture(int hp)
    {
        if (hp <= maxhp / 1.9f && hp > 0)
            return AssetManager.Instance.textures[$"enemy/damage{new Random().Next(1, 4)}"];
        return AssetManager.Instance.textures["enemy/enemy"];
    }
}