/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;

public class RigidBodyCircle : Component
{
    public CircleCollider circleCollider;
    private bool staticRb;

    /// <summary>
    /// Handles Collisions of Rigidbodies (only one CircleCollider per GameObject works)
    /// </summary>
    public RigidBodyCircle(GameObject parent, bool staticRb = false) : base(parent)
    {
        circleCollider = parent.GetComponent<CircleCollider>()!;
        circleCollider.collided += CorrectCollision;
        this.staticRb = staticRb;
    }

    private void CorrectCollision(Collider coll)
    {
        if (!coll.parent.GetComponent(out RigidBodyCircle? b)) return;
        Vector2f currentPos = circleCollider.center;

        Vector2f currentRbPos = b!.circleCollider.center;
        float distance = Utilities.Distance(currentPos, currentRbPos);
        float minDistance = circleCollider.radius + b.circleCollider.radius;
        if (distance < minDistance)
        {
            float overlapAmount = MathF.Abs(distance - minDistance);
            Vector2f correction = (currentPos - currentRbPos).Normalize() * overlapAmount;
            if (staticRb) b.parent.Position -= correction;
            else if (b.staticRb) parent.Position += correction;
            else
            {
                parent.Position += correction / 2;
                b.parent.Position -= correction / 2;
            }
        }
    }

    protected override void SetRequiredComponents()
    {
        requires.Add(typeof(CircleCollider));
    }

    public override void Destroy()
    {
        base.Destroy();
        circleCollider.collided -= CorrectCollision;
    }
}