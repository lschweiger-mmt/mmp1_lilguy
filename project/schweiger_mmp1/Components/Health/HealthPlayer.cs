/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */


using SFML.Graphics;

public class HealthPlayer : Health
{
    public new Player parent;
    public HealthPlayer(GameObject parent, int maxhp, float iFrameDuration, float? knockedOutDuration = null) : base(parent, maxhp, iFrameDuration, null, knockedOutDuration)
    {
        this.parent = (Player)parent!;
    }

    public override void TakeDamage(GameObject who, int damage)
    {
        if (invulnerable) return;
        ScoreManager.Instance.CalculateScore(hp);
        base.TakeDamage(who, damage);

        SoundManager.Instance.Play("lilguy/ugh1", false, true);
        SoundManager.Instance.Play("tinitus");
        MusicManager.Instance.LowerMusicVolumeImpact();
        TimeManager.Instance.SetTimeScale(.1f);
        if (hp == 2 || hp == 1 || hp == 3) MusicManager.Instance.PlayMusic(GetMusicTrack());

    }

    public override void IFrames()
    {
        WaitManager.StopWait("resetheadsprite");
        IFrameBodyPart(parent.body, "body");
        IFrameBodyPart(parent.head, "head");
        IFrameBodyPart(parent.hand, "hand");
        Texture changeToTexture = AssetManager.Instance.textures[$"sword/sword"];
        parent.sword.sword.spriteRenderer.SetTexture(AssetManager.Instance.textures[$"lilguy/iframe/sword"]);
        WaitManager.Wait(iFrameDuration / 10f, () => parent.sword.sword.spriteRenderer.SetTexture(changeToTexture), true);

    }
    private void IFrameBodyPart(SpriteObject obj, string name)
    {
        Texture changeToTexture = AssetManager.Instance.textures[$"lilguy/{name}"];
        obj.spriteRenderer.SetTexture(AssetManager.Instance.textures[$"lilguy/iframe/{name}"]);
        WaitManager.Wait(iFrameDuration / 10f, () => obj.spriteRenderer.SetTexture(changeToTexture), true);
    }

    public override void Die()
    {
        base.Die();
        MusicManager.Instance.PlayMusic("menu");
        WaitManager.Wait(.5f, "circleIn1", () => parent.game.SetDesiredRadius(.2f));
        WaitManager.Wait(1.6f, "circleIn2", () => parent.game.SetDesiredRadius(-.2f));
        WaitManager.Wait(2.3f, "circleout", () => parent.game.SetMaxCircleRadius());
        WaitManager.Wait(2.3f, "loadDeathScene", () => Game.Instance.LoadScene(ref Game.Instance.sceneDeath));
    }

    public string GetMusicTrack()
    {
        if (hp == 3) return "dreileben";
        if (hp == 2) return "zweileben";
        if (hp == 1) return "einleben";
        else return "dreileben";
    }
}