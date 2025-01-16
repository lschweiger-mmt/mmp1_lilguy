/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.Graphics;
using SFML.System;

public class UIElement : GameObject
{
    private View view;

    public Transformable? shape;
    public Anchor anchor;
    public Vector2f size;
    public Vector2f positionFromAnchor;

    public RectangleShape debug;

    public Vector2f originalSize, originalPositionFromAnchor;
    public Vector2f currentPositionOffset, desiredPositionOffset;
    public Vector2f currentScaleOffset = new(1, 1), desiredScaleOffset = new(1, 1);
    public float currentRotationOffset = 0, desiredRotationOffset = 0;
    private float offsetSpeed = 10;

    private bool anchorsSetup = false;
    public bool ownAnchor = false;
    public bool dontAnimate = false;

    public UIElement(UIElement parent, Distances distances = new()) : base(parent)
    {
        Vector2f itemSize = new(parent.size.X - distances.left - distances.right, parent.size.Y - distances.top - distances.bottom);
        Vector2f middle = new(distances.left + itemSize.X / 2, distances.top + itemSize.Y / 2);
        MakeElement(new Anchor(true, true, true, true), middle - parent.size / 2, itemSize);
    }

    public UIElement(GameObjectHolder parent, Anchor anchor, Vector2f positionFromAnchor = new(), Vector2f size = new()) : base(parent)
    {
        MakeElement(anchor, positionFromAnchor, size);
    }

    private void MakeElement(Anchor anchor, Vector2f positionFromAnchor = new(), Vector2f size = new())
    {
        this.anchor = anchor;
        view = game.view;
        originalSize = size;
        originalPositionFromAnchor = positionFromAnchor;

        UpdateScale();
    }

    public override void LateInitialize()
    {
        UpdateScale();
    }
    public virtual void SetupShape() { }

    public void UpdateScale()
    {
        this.size = originalSize * (float)Hud.uiScale;
        this.positionFromAnchor = originalPositionFromAnchor * (float)Hud.uiScale;

        debug = new RectangleShape(this.size);
        debug.FillColor = Color.Transparent;
        debug.OutlineColor = Color.Red;
        debug.OutlineThickness = -6;




        foreach (var uiElement in children.ToList())
            (uiElement as UIElement)!.UpdateScale();


        if (parent is not UIElement)
            ChangeAnchor();

        SetupShape();
    }

    public void ChangeSize(Vector2f size)
    {

        originalSize = size;
        UpdateScale();
    }

    public void ChangeAnchor()
    {
        bool topLeft = anchor.tl;
        bool topRight = anchor.tr;
        bool bottomRight = anchor.br;
        bool bottomLeft = anchor.bl;
        bool top = topLeft && topRight && !bottomLeft && !bottomRight;
        bool bottom = !topLeft && !topRight && bottomLeft && bottomRight;
        bool right = !topLeft && topRight && !bottomLeft && bottomRight;
        bool left = topLeft && !topRight && bottomLeft && !bottomRight;
        bool center = (topLeft && topRight && bottomLeft && bottomRight) || (!topLeft && !topRight && !bottomLeft && !bottomRight);

        float width = size.X;
        float height = size.Y;

        UIElement? thisParent = null;
        if (parent != null) thisParent = parent as UIElement;


        if (center) localOrigin = new Vector2f(width, height) / 2;
        else if (top) localOrigin = new Vector2f(width / 2, 0);
        else if (right) localOrigin = new Vector2f(width, height / 2);
        else if (bottom) localOrigin = new Vector2f(width / 2, height);
        else if (left) localOrigin = new Vector2f(0, height / 2);
        else if (topLeft) localOrigin = new Vector2f(0, 0);
        else if (topRight) localOrigin = new Vector2f(width, 0);
        else if (bottomRight) localOrigin = new Vector2f(width, height);
        else if (bottomLeft) localOrigin = new Vector2f(0, height);

        Vector2f parentOffset = new();
        float parentWidth;
        float parentHeight;

        if (parent is not UIElement)
        {
            parentOffset = (view.Center - view.Size) / 2;
            parentWidth = view.Size.X;
            parentHeight = view.Size.Y;
        }
        else
        {
            parentOffset = thisParent!.localPosition - thisParent!.localOrigin;
            parentWidth = thisParent!.size.X;
            parentHeight = thisParent!.size.Y;
        }

        if (center || top || right || bottom || left)
        {
            Vector2f screenOrigin = new();
            if (center) screenOrigin = parent == null ? view.Size / 2 : new Vector2f(parentWidth, parentHeight) / 2;
            else if (top) screenOrigin = new(parentWidth / 2, 0);
            else if (right)
            {
                screenOrigin = new(parentWidth, parentHeight / 2);
                positionFromAnchor = Utilities.ComponentProduct(positionFromAnchor, new(-1, 1));
            }
            else if (bottom)
            {
                screenOrigin = new(parentWidth / 2, parentHeight);
                positionFromAnchor = Utilities.ComponentProduct(positionFromAnchor, new(1, -1));
            }
            else if (left) screenOrigin = new(0, parentHeight / 2);

            localPosition = screenOrigin + parentOffset + positionFromAnchor;
        }
        else
        {
            float x = positionFromAnchor.X;
            float y = positionFromAnchor.Y;
            localPosition = new Vector2f((topLeft || bottomLeft ? x : parentWidth - x) + parentOffset.X, (topLeft || topRight ? y : parentHeight - y) + parentOffset.Y);
        }

        anchorsSetup = true;

        foreach (UIElement item in children)
            item.ChangeAnchor();
    }

    public override void LateUpdate()
    {
        if (!ActiveSelf()) return;

        CustomLateUpdate();

        if (Settings.showDebug)
            ShapeDrawer.DrawCircle(this.origin, Position, .05f, 16, Color.Yellow);

        foreach (var component in components)
            component.LateUpdate();
    }
    public override void CustomLateUpdate()
    {
        if (!anchorsSetup) ChangeAnchor();

        float expFactor = 1f - MathF.Exp(-Game.deltatimeDebug * offsetSpeed);
        currentPositionOffset += (GetPositionOffsetFromAllParents() - currentPositionOffset) * expFactor;
        currentScaleOffset += (GetScaleOffsetFromAllParents() - currentScaleOffset) * expFactor;
        currentRotationOffset += (GetRotationOffsetFromAllParents() - currentRotationOffset) * expFactor;

        Position = localPosition + currentPositionOffset * (float)Hud.uiScale;
        // Position = localPosition;
        Rotation = localRotation + currentRotationOffset;
        Scale = Utilities.ComponentProduct(localScale, currentScaleOffset);
        // Scale = localScale;
        Origin = localOrigin;

        if (parent != null)
        {
            Vector2f relativePosition = Position - GetHighestParent().Position;
            Vector2f rotatedVector = Utilities.RotateVector(relativePosition, Utilities.ToRadians(Rotation));
            rotatedVector = Utilities.ComponentProduct(rotatedVector, Scale);
            Position = Position - (relativePosition - rotatedVector);
        }
    }

    public void SetDesiredPositionOffset(Vector2f offset, float speed = 15, Vector2f? jumpValue = null)
    {
        if ((GetHighestParent() as UIElement)!.dontAnimate) return;
        if (jumpValue != null)
        {
            currentPositionOffset = (Vector2f)jumpValue + (GetHighestParent() as UIElement)!.currentPositionOffset;
            currentPositionOffset = currentPositionOffset.Normalize() * Utilities.ClosestToZero(Utilities.Magnitude(currentPositionOffset), Utilities.Magnitude((Vector2f)jumpValue * 2.25f));
        };
        SetOffsetSpeed(speed);
        desiredPositionOffset = offset;
    }
    public void SetDesiredScaleOffset(Vector2f offset, float speed = 15, Vector2f? jumpValue = null)
    {
        if ((GetHighestParent() as UIElement)!.dontAnimate) return;
        if (jumpValue != null)
        {
            currentScaleOffset = Utilities.ComponentProduct((Vector2f)jumpValue, (GetHighestParent() as UIElement)!.currentScaleOffset);
            currentScaleOffset = currentScaleOffset.Normalize() * Utilities.ClosestToZero(Utilities.Magnitude(currentScaleOffset), Utilities.Magnitude((Vector2f)jumpValue * 1.5f));
        };
        SetOffsetSpeed(speed);
        desiredScaleOffset = offset;
    }
    public void SetDesiredRotationOffset(float offset, float speed = 15, float? jumpValue = null, bool ownAnchor = false)
    {
        if ((GetHighestParent() as UIElement)!.dontAnimate) return;
        if (jumpValue != null) currentRotationOffset = (float)jumpValue + (GetHighestParent() as UIElement)!.currentRotationOffset;
        this.ownAnchor = ownAnchor;
        SetOffsetSpeed(speed);
        desiredRotationOffset = offset;
    }
    public void SetOffsetSpeed(float speed)
    {
        offsetSpeed = speed;
        foreach (UIElement item in children)
            item.SetOffsetSpeed(speed);
    }

    public Vector2f GetCurrentPositionOffsetFromAllParents()
    {
        Vector2f ret = new();
        UIElement current = this;
        while (true)
        {
            ret += current.currentPositionOffset;
            if (current.parent == null) return ret;
            current = (UIElement)current.parent;
        }
    }

    public Vector2f GetPositionOffsetFromAllParents()
    {
        Vector2f ret = new();
        UIElement current = this;
        while (true)
        {
            ret += current.desiredPositionOffset;
            if (current.parent == null) return ret * (float)Hud.uiScale;
            current = (UIElement)current.parent;
        }
    }
    public Vector2f GetScaleOffsetFromAllParents()
    {
        Vector2f ret = new(1, 1);
        UIElement current = this;
        while (true)
        {
            ret = Utilities.ComponentProduct(ret, current.desiredScaleOffset);
            if (current.parent == null) return ret;
            current = (UIElement)current.parent;
        }
    }
    public float GetRotationOffsetFromAllParents()
    {
        float ret = 0;
        UIElement current = this;
        while (true)
        {
            ret += current.desiredRotationOffset;
            if (current.parent == null) return ret;
            current = (UIElement)current.parent;
        }
    }

    public override void CustomDrawCall(RenderTarget target, RenderStates states)
    {
        SetTransforms(debug);
        if (shape != null) SetTransforms(shape);

        (shape as Drawable)?.Draw(target, states);

        if (Settings.showDebug)
            debug.Draw(target, states);
    }



    public virtual void SetTransforms(Transformable obj)
    {
        obj.Position = Position;
        obj.Rotation = Rotation;
        obj.Scale = Scale;
        obj.Origin = Origin;
    }

    public Vector2f GetGlobalPosition()
    {
        if (parent is UIElement)
            return Position + Origin + (parent as UIElement)!.GetGlobalPosition();
        else
            return Position + Origin;
    }


    public struct Distances
    {
        public float top, right, bottom, left;

        public Distances(float top, float right, float bottom, float left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }
        public Distances(float spacing)
        {
            this.top = spacing;
            this.right = spacing;
            this.bottom = spacing;
            this.left = spacing;
        }
    }
}

public struct Anchor : IEquatable<Anchor>
{
    public bool tl, tr, br, bl;

    public Anchor(bool tl = false, bool tr = false, bool br = false, bool bl = false)
    {
        this.tl = tl;
        this.tr = tr;
        this.br = br;
        this.bl = bl;
        if (tl && tr && br && bl)
        {
            this.tl = false;
            this.tr = false;
            this.br = false;
            this.bl = false;
        }
    }

    public bool Equals(Anchor other)
    {
        return this.tl == other.tl && this.tr == other.tr && this.br == other.br && bl == other.bl;
    }
}
