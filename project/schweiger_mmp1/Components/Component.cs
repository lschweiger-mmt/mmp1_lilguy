/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public abstract class Component : Drawable
{
    public GameObject parent;
    public bool canHaveMultiple;
    private bool active = true;
    public List<Type> requires = new();

    public Component(GameObject parent)
    {
        this.parent = parent;
        SetComponentProperties();
        SetRequiredComponents();

        foreach (var reqComponent in requires)
        {
            if (!HasComponentOfType(parent.components, reqComponent))
                throw new MissingMemberException($"{this} requires Component of type {reqComponent} in parent");
        }
        parent.AddComponent(this);
    }

    public virtual void SetComponentProperties()
    {
        canHaveMultiple = false;
        active = true;
    }

    public virtual void Update(float deltatime) { }
    public virtual void LateUpdate() => ChangePos(parent.Position, parent.Rotation, parent.Scale);
    public virtual void ChangePos(Vector2f pos, float rotation, Vector2f scale) { }

    public void SetActive(bool active)
    {
        this.active = active;
        OnActiveChanged(active);
    }
    public virtual void OnActiveChanged(bool to) { }

    public bool ActiveSelf()
    {
        if (!parent.ActiveSelf()) return false;
        return active;
    }

    protected virtual void SetRequiredComponents() => requires = new();

    private bool HasComponentOfType(List<Component> components, Type type)
    {
        foreach (var item in components)
            if (item.GetType() == type)
                return true;

        return false;
    }

    public virtual void Draw(RenderTarget target, RenderStates states) { }
    public virtual void DebugDraw(RenderTarget target, RenderStates states) { }
    public virtual void Destroy() { }
}