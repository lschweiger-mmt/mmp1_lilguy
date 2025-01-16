/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public abstract class GameObject : GameObjectHolder, Drawable, IComparable<GameObject>
{
    public Scene scene;
    public GameObject? parent;

    public List<Component> components = new();

    public Vector2f localPosition;
    public float localRotation;
    public Vector2f localScale = Utilities.vOne;
    public Vector2f localOrigin = Utilities.vZero;

    private bool active = true;
    private bool initialized = false;


    // Debug
    public CircleShape origin = new();

    public GameObject(GameObjectHolder parent, Transform? transform = null)
    {
        parent.AddChild(this);
        if (transform != null) TransformGamobject((Transform)transform);
    }

    public virtual void Update(float deltatime)
    {
        if (!active) return;

        if (!initialized)
        {
            LateInitialize();
            initialized = true;
        }

        foreach (var component in components)
            if (component.ActiveSelf()) component.Update(deltatime);

        foreach (var child in children.ToList())
            child.Update(deltatime);
    }

    public virtual void LateInitialize() { }

    public virtual void LateUpdate()
    {
        if (!active) return;

        // Transform with parent
        if (parent != null)
        {
            float relativeRotation = parent.Rotation + Utilities.ToDegrees(Utilities.AngleBetween(Utilities.vZero, localPosition));
            Vector2f rotationVector = new Vector2f(-MathF.Cos(Utilities.ToRadians(relativeRotation)), -MathF.Sin(Utilities.ToRadians(relativeRotation)));
            Origin = parent.Origin + parent.localOrigin;
            Position = Utilities.ComponentProduct(rotationVector * Utilities.Magnitude(localPosition, 1), parent.Scale) + parent.Position;
            Rotation = localRotation + parent.Rotation;
            Scale = Utilities.ComponentProduct(localScale, parent.Scale);
        }

        CustomLateUpdate();

        if (Settings.showDebug)
            ShapeDrawer.DrawCircle(this.origin, Position, .05f, 16, Color.Yellow);

        foreach (var component in components)
            component.LateUpdate();
    }

    public virtual void CustomLateUpdate() { }

    public virtual void Draw(RenderTarget target, RenderStates states)
    {
        if (!active) return;
        LateUpdate();

        CustomDrawCall(target, states);
        foreach (var component in components)
            component?.Draw(target, states);

        foreach (var child in children.ToList())
            child?.Draw(target, states);

        if (Settings.showDebug)
        {
            foreach (var component in components)
                component?.DebugDraw(target, states);

            origin.Draw(target, states);
        }
    }

    public virtual void CustomDrawCall(RenderTarget target, RenderStates states) { }

    public bool HasComponent(Type component)
    {
        foreach (var item in components)
            if (item.GetType() == component) return true;

        return false;
    }
    public bool HasComponent<T>() where T : Component
    {
        foreach (var item in components)
            if (item is T) return true;

        return false;
    }

    /// <exception cref="ArgumentException"></exception>
    public T AddComponent<T>(T component) where T : Component
    {
        if (!component.canHaveMultiple && HasComponent(component.GetType()))
            throw new ArgumentException($"Cant add component of type {component.GetType().Name} because the GameObject already has it");
        components.Add(component);
        return component;
    }

    public List<T> GetComponents<T>() where T : Component
    {
        GetComponents(out List<T> ret);
        return ret!;
    }
    public bool GetComponents<T>(out List<T> returnComponents) where T : Component
    {
        returnComponents = new();
        foreach (var item in components)
            if (item is T)
                returnComponents.Add((T)item);

        return returnComponents.Count > 0;
    }

    public T? GetComponent<T>() where T : Component
    {
        GetComponent(out T? ret);
        return ret;
    }
    public bool GetComponent<T>(out T? component) where T : Component
    {
        component = default;
        foreach (var item in components)
        {
            if (item is T)
            {
                component = (T)item;
                return true;
            }
        }
        return false;
    }

    public virtual void Destroy()
    {
        parent?.children.Remove(this);
        scene?.children.Remove(this);
        foreach (var component in components.ToList())
            component.Destroy();

        components = new();
        foreach (var child in children.ToList())
            child.Destroy();

        children = new();
        Dispose();
    }

    public void TransformGamobject(Transform transform)
    {
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
        localScale = transform.localScale;
    }

    public virtual void SetActive(bool active)
    {
        this.active = active;
        foreach (var item in children)
            item.SetActive(active);
    }

    public bool ActiveSelf()
    {
        if (parent?.ActiveSelf() == false || scene.ActiveSelf() == false) return false;
        return active;
    }

    public int CompareTo(GameObject? obj)
    {
        return (Position.Y + Origin.Y).CompareTo(obj?.Position.Y + obj?.Origin.Y);
    }

    public GameObject GetHighestParent()
    {
        GameObject current = this;
        while (current.parent != null)
            current = current.parent;
        return current;
    }
}

public struct Transform()
{
    public Vector2f localPosition;
    public float localRotation;
    public Vector2f localScale;

    public Transform(Vector2f? localPosition = null, float? localRotation = null, Vector2f? localScale = null) : this()
    {
        if (localPosition != null) this.localPosition = (Vector2f)localPosition;
        else this.localPosition = Utilities.vZero;

        if (localRotation != null) this.localRotation = (float)localRotation;
        else this.localRotation = 0;

        if (localScale != null) this.localScale = (Vector2f)localScale;
        else this.localScale = Utilities.vOne;
    }
}


public class GameObjectHolder : Transformable
{
    public Game game;

    public List<GameObject> children = new();

    public GameObjectHolder() => game = Game.Instance;

    public void AddChild(GameObject gameObject)
    {
        bool go = this is GameObject;
        bool sc = this is Scene;

        if (go) gameObject.parent = this as GameObject;
        if (sc) gameObject.scene = (this as Scene)!;
        else gameObject.scene = gameObject.GetHighestParent().scene;

        bool ui = gameObject is UIElement;

        if (ui && sc) (this as Scene)!.hud.uiElements.Add((gameObject as UIElement)!);
        else children.Add(gameObject);
    }
}