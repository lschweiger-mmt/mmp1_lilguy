/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public abstract class Collider : Component, IComparable
{
    public Drawable? debug;
    public string tag;
    public delegate void CollisionEvent(Collider with);
    public event CollisionEvent collided;

    protected Collider(GameObject parent, string tag) : base(parent)
    {
        this.tag = tag;
        CollisionChecker.colliders.Add(this);
        CollisionChecker.collided += Collided;
    }
    private void Collided(object? sender, Collider e)
    {
        if (sender != this) return;
        collided?.Invoke(e);
    }

    public virtual int CompareTo(object? obj) => Left().CompareTo((obj as Collider)?.Left());
    public virtual float Left() => parent.Position.X;
    public virtual float Right() => parent.Position.X;
    public override void SetComponentProperties() => canHaveMultiple = true;

    public override void OnActiveChanged(bool to)
    {
        if (to) CollisionChecker.colliders.Add(this);
        if (!to) CollisionChecker.colliders.Remove(this);
    }

    public override void Destroy()
    {
        base.Destroy();
        CollisionChecker.collided -= Collided;
        CollisionChecker.colliders.Remove(this);
    }
};

public class BoxCollider : Collider
{
    public Vector2f center, offset, size;
    private Vector2f localsize;

    public BoxCollider(GameObject parent, string tag, Vector2f dim) : base(parent, tag)
    {
        Setup(dim, Utilities.vZero);
    }
    public BoxCollider(GameObject parent, string tag, Vector2f dim, Vector2f offset) : base(parent, tag)
    {
        Setup(dim, offset);
    }

    public void Setup(Vector2f dim, Vector2f offset)
    {
        debug = new RectangleShape();
        this.offset = offset;
        center = new();
        localsize = dim;
    }

    public Vector2f Min() => center - size / 2;
    public Vector2f Max() => center + size / 2;

    public override void ChangePos(Vector2f pos, float rotation, Vector2f scale)
    {
        if (!ActiveSelf()) return;

        size = Utilities.ComponentProduct(scale, localsize);
        center = Utilities.RotateVector(Utilities.ComponentProduct(scale, offset), Utilities.ToRadians(rotation)) + pos - parent.Origin - parent.localOrigin;
        if (Settings.showDebug && debug != null)
            ShapeDrawer.RectOutline((RectangleShape)debug, Min(), Max(), 0, Utilities.vZero, 6f, Color.Green);
    }

    public override void DebugDraw(RenderTarget target, RenderStates states)
    {
        if (!ActiveSelf()) return;
        debug?.Draw(target, states);
    }
    public override float Left() => Min().X;
    public override float Right() => Max().X;
}

public class CircleCollider : Collider
{
    public Vector2f center;
    public Vector2f offset = new(0, 0);
    private float localRadius;
    public float radius;

    public CircleCollider(GameObject parent, string tag, float radius) : base(parent, tag)
    {
        Setup(radius, Utilities.vZero);
    }
    public CircleCollider(GameObject parent, string tag, float radius, Vector2f offset) : base(parent, tag)
    {
        Setup(radius, offset);
    }

    public void Setup(float radius, Vector2f offset)
    {
        debug = new CircleShape();
        this.offset = offset;
        center = new();
        localRadius = radius;
    }

    public override void ChangePos(Vector2f pos, float rotation, Vector2f scale)
    {
        center = parent.Position + Utilities.RotateVector(Utilities.ComponentProduct(scale, offset), Utilities.ToRadians(rotation));
        radius = (MathF.Abs(scale.X) + MathF.Abs(scale.Y)) / 2 * localRadius;

        if (Settings.showDebug && debug != null)
            ShapeDrawer.DrawCircle((CircleShape)debug, center, radius, 6, Color.Green);
    }

    public override void DebugDraw(RenderTarget target, RenderStates states)
    {
        if (!ActiveSelf()) return;
        debug?.Draw(target, states);
    }

    public override float Left() => center.X - radius;
    public override float Right() => center.X + radius;
}

public static class CollisionChecker
{
    public static event EventHandler<Collider> collided;
    public static List<Collider> colliders = new();

    public static void CheckCollisions()
    {
        // Sweep and Prune
        colliders.Sort();
        for (int i = 0; i < colliders.Count; i++)
        {
            Collider coll1 = colliders[i];
            for (int j = i + 1; j < colliders.Count; j++)
            {
                Collider coll2 = colliders[j];

                if (coll2.Left() > coll1.Right()) break;
                if (Collide(coll1, coll2))
                {
                    collided.Invoke(coll1, coll2);
                    collided.Invoke(coll2, coll1);
                }
            }
        }
    }

    public static bool Collide(Collider a, Collider b)
    {
        if (!(a.ActiveSelf() && b.ActiveSelf())) return false;
        a.LateUpdate();
        b.LateUpdate();

        if (a is BoxCollider && b is BoxCollider)
            return CollideBoxBox((BoxCollider)a, (BoxCollider)b);

        if (a is CircleCollider && b is CircleCollider)
            return CollideCircleCircle((CircleCollider)a, (CircleCollider)b);

        if (a is BoxCollider && b is CircleCollider)
            return CollideCircleBox((CircleCollider)b, (BoxCollider)a);

        if (a is CircleCollider && b is BoxCollider)
            return CollideCircleBox((CircleCollider)a, (BoxCollider)b);

        throw new Exception($"No predefined collision combination: {a.GetType()} and {b.GetType()}");
    }

    private static bool CollideBoxBox(BoxCollider a, BoxCollider b)
    {
        bool conditionA = a.Max().X < b.Min().X;
        bool conditionB = a.Min().X > b.Max().X;
        bool conditionC = a.Max().Y < b.Min().Y;
        bool conditionD = a.Min().Y > b.Max().Y;

        return conditionA && conditionB && conditionC && conditionD;
    }

    private static bool CollideCircleCircle(CircleCollider a, CircleCollider b)
    {
        return Utilities.Distance(a.center, b.center) < (a.radius + b.radius);
    }

    private static bool CollideCircleBox(CircleCollider circle, BoxCollider rect)
    {
        // temporary variables to set edges for testing
        float testX = circle.center.X;
        float testY = circle.center.Y;

        // which edge is closest?
        if (circle.center.X < rect.center.X - rect.size.X / 2) testX = rect.center.X - rect.size.X / 2;      // test left edge
        else if (circle.center.X > rect.center.X - rect.size.X / 2 + rect.size.X) testX = rect.center.X - rect.size.X / 2 + rect.size.X;   // right edge
        if (circle.center.Y < rect.center.Y - rect.size.Y / 2) testY = rect.center.Y - rect.size.Y / 2;      // top edge
        else if (circle.center.Y > rect.center.Y - rect.size.Y / 2 + rect.size.Y) testY = rect.center.Y - rect.size.Y / 2 + rect.size.Y;   // bottom edge

        // get distance from closest edges
        float distX = circle.center.X - testX;
        float distY = circle.center.Y - testY;
        float distance = MathF.Sqrt((distX * distX) + (distY * distY));

        // if the distance is less than the radius, collision!
        if (distance <= circle.radius)
        {
            return true;
        }
        return false;
    }
}