/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;

public class Movement : Component
{
    public float accelaration, maxAcceleration, maxVelocity, friction;
    private Vector2f velocity, moveDirection;
    public bool acceptsInput = true;

    private RigidBodyCircle? rb;

    public Movement(GameObject parent, float maxAcceleration, float maxVelocity, float friction) : base(parent)
    {
        this.maxAcceleration = maxAcceleration;
        this.maxVelocity = maxVelocity;
        this.friction = friction;

        rb = parent.GetComponent<RigidBodyCircle>();
    }

    public void LoadAccelartionAndVelocity()
    {
        velocity = new();
        accelaration = 0;
    }

    public override void Update(float deltatime)
    {
        base.Update(deltatime);
        velocity += moveDirection * accelaration * deltatime;
        if (velocity.Magnitude() > maxVelocity && acceptsInput) velocity = velocity.Normalize() * maxVelocity;

        parent.Position += velocity * deltatime;

        if (accelaration <= 0)
        {
            accelaration = 0;
            velocity *= friction;
        }
        if (acceptsInput)
            accelaration = 0;
    }

    public virtual void Move(Vector2f dir, float deltatime)
    {
        if (!acceptsInput) return;

        dir.Normalize();
        if (dir.Magnitude() <= 0) return;

        moveDirection = dir;
        accelaration = maxAcceleration;
    }

    public virtual void ApplyForce(float amount, float duration)
    {
        ApplyForce(moveDirection, amount, duration);
    }
    public virtual void ApplyForce(Vector2f dir, float amount, float duration)
    {
        SetAcceptsInput(false);
        moveDirection = dir;
        accelaration = amount;
        WaitManager.Wait(duration, () => SetAcceptsInput(true));
    }
    public void SetAcceptsInput(bool to)
    {
        accelaration = 0;
        acceptsInput = to;
    }
}