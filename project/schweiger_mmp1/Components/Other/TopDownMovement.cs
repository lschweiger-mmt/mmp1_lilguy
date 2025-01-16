/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;
using SFML.Window;

public class MovementPlayer : Movement
{
    private HealthPlayer? healthPlayer;
    CircleCollider coll;
    public MovementPlayer(GameObject parent, float maxAcceleration, float maxVelocity, float friction) : base(parent, maxAcceleration, maxVelocity, friction)
    {
        healthPlayer = parent.GetComponent<HealthPlayer>();
        coll = parent.GetComponent<CircleCollider>()!;
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
        MoveInputs(deltatime);
    }

    private void MoveInputs(float deltatime)
    {
        if (healthPlayer != null && healthPlayer.dead)
        {
            parent.children[0].GetComponent<Animator>()?.Stop(.1f);
            return;
        }

        Vector2f moveDir = Utilities.vZero;
        if (InputManager.Instance.GetKeyPressed(Keyboard.Key.W)) moveDir -= Utilities.vUp;
        if (InputManager.Instance.GetKeyPressed(Keyboard.Key.A)) moveDir -= Utilities.vRight;
        if (InputManager.Instance.GetKeyPressed(Keyboard.Key.S)) moveDir += Utilities.vUp;
        if (InputManager.Instance.GetKeyPressed(Keyboard.Key.D)) moveDir += Utilities.vRight;

        Move(moveDir, deltatime);
        CheckBounds();

        if (moveDir.Magnitude() > 0.01 && acceptsInput)
            parent.children[0].GetComponent<Animator>()?.SetPlayingAnimation("default");
        else
            parent.children[0].GetComponent<Animator>()?.Stop(.1f);
    }

    private void CheckBounds()
    {
        Vector2f center = coll.center;
        float radius = coll.radius;
        Vector2f view = parent.game.view.Size;

        float left = center.X - radius*1.5f + view.X / 2;
        float right = view.X / 2 - center.X - radius*1.5f;
        float top = center.Y - radius*3 + view.Y / 2;
        float bottom = view.Y / 2 - center.Y - radius;

        if (top < 0) parent.Position -= new Vector2f(0, top/2);
        if (left < 0) parent.Position -= new Vector2f(left/2, 0);
        if (right < 0) parent.Position += new Vector2f(right/2, 0);
        if (bottom < 0) parent.Position += new Vector2f(0, bottom/2);
    }

    public override void OnActiveChanged(bool active)
    {
        if (!active) parent.children[0].GetComponent<Animator>()?.Stop(.1f / 2);
    }

    public override void Move(Vector2f dir, float deltatime)
    {
        base.Move(dir, deltatime);
    }
}