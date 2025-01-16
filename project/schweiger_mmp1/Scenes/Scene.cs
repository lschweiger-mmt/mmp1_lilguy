/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;

public abstract class Scene : GameObjectHolder, Drawable
{
    private bool active;
    public Hud hud;

    public virtual void Initialize()
    {
        hud.Initialize();
    }
    public virtual void Load()
    {
        SetActive(true);
        hud.Load();
    }
    public void Update(float deltatime)
    {
        BeforeUpdate(deltatime);

        foreach (var gameobject in children.ToList())
            gameobject.Update(deltatime);
        hud.Update(deltatime);

        AfterUpdate(deltatime);
    }

    public virtual void AfterUpdate(float deltatime) { }
    public virtual void BeforeUpdate(float deltatime) { }

    public void Draw(RenderTarget target, RenderStates states)
    {
        BeforeDraw(target, states);
        if (hud.mainPanel != null)
        {
            game.shaderRectangleBackground.Draw(target, new RenderStates(game.backgroundShaderStates));
        }

        foreach (var gameobject in children.ToList())
            gameobject.Draw(target, states);

        foreach (var uiElement in hud.uiElements.ToList())
            uiElement.Draw(target, states);

        AfterDraw(target, states);
    }

    public virtual void BeforeDraw(RenderTarget target, RenderStates states) { }
    public virtual void AfterDraw(RenderTarget target, RenderStates states) { }

    public void SetActive(bool to)
    {
        active = to;
    }
    public bool ActiveSelf() => active;
}