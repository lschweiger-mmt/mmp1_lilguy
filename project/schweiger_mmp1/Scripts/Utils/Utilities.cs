/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

using SFML.System;
static class Utilities
{
    public static readonly Vector2f vZero = new(0, 0);
    public static readonly Vector2f vOne = new(1, 1);
    public static readonly Vector2f vRight = new(1, 0);
    public static readonly Vector2f vUp = new(0, 1);

    /// <summary>
    /// Length of the vector.
    /// </summary>
    public static float Magnitude(this Vector2f input, int multiplicator = 1)
    {
        return MathF.Sqrt(input.X * input.X + input.Y * input.Y) * multiplicator;
    }


    /// <summary>
    /// A method that returns the squared magnitude of a vector.
    /// </summary>
    public static float SqrMagnitude(this Vector2f input)
    {
        return input.Magnitude() * input.Magnitude();
    }

    /// <summary>
    /// A method for converting radians to degrees.
    /// </summary>
    public static float ToDegrees(this float angle)
    {
        return angle / (MathF.PI / 180);
    }

    /// <summary>
    /// A method for converting degrees to radians.
    /// </summary>
    public static float ToRadians(this float angle)
    {
        return angle * (MathF.PI / 180);
    }

    /// <summary>
    /// A method for normalizing (length=1) vectors (in-place).
    /// </summary>
    public static Vector2f Normalize(this Vector2f input)
    {
        if (input == new Vector2f(0, 0)) return new Vector2f(0, 0);
        return new Vector2f(input.X / input.Magnitude(), input.Y / input.Magnitude());
    }

    /// <summary>
    /// Rotates the given vector input by the radians passed with the angle parameter
    /// and returns the rotated vector.
    /// </summary>
    public static Vector2f RotateVector(Vector2f input, float angle)
    {
        return new Vector2f(
            MathF.Cos(angle) * input.X - MathF.Sin(angle) * input.Y,
            MathF.Sin(angle) * input.X + MathF.Cos(angle) * input.Y
        );
    }

    /// <summary>
    /// Returns the angle between two given vectors. The return value is in radians.
    /// </summary>
    public static float AngleBetween(Vector2f v1, Vector2f v2)
    {
        return MathF.Atan2(v1.Y - v2.Y, v1.X - v2.X);
    }

    /// <summary>
    /// Returns the distance between two given points.
    /// </summary>
    public static float Distance(Vector2f v1, Vector2f v2)
    {
        return new Vector2f(MathF.Abs(v2.X - v1.X), MathF.Abs(v2.Y - v1.Y)).Magnitude();
    }

    /// <summary>
    /// A method for linear interpolation between two points.
    /// </summary>
    public static Vector2f Lerp(Vector2f v1, Vector2f v2, float t)
    {
        return v1 * (1f - t) + v2 * t;
    }

    /// <summary>
    /// A method for linear interpolation between two floats.
    /// </summary>
    public static float Lerp(float v1, float v2, float t)
    {
        return v1 * (1f - t) + v2 * t;
    }

    /// <summary>
    /// A method for calculating the dot product of two vectors.
    /// </summary>
    public static float Dot(Vector2f lhs, Vector2f rhs)
    {
        return lhs.X * rhs.X + lhs.Y * lhs.Y;
    }

    public static Vector2f ComponentProduct(Vector2f v1, Vector2f v2)
    {
        return new Vector2f(v1.X * v2.X, v1.Y * v2.Y);
    }
    public static Vector2f ComponentProduct(Vector2f v1, Vector2f v2, Vector2f v3)
    {
        return new Vector2f(v1.X * v2.X * v3.X, v1.Y * v2.Y * v3.Y);
    }
    public static Vector2f ComponentDivision(Vector2f v1, Vector2f v2)
    {
        return new Vector2f(v1.X / v2.X, v1.Y / v2.Y);
    }

    /// <summary>
    /// Generates a random string
    /// </summary>
    public static string RandomString(int stringLength = 10)
    {
        Random rand = new Random();

        int randValue;
        string str = "";
        char letter;
        for (int i = 0; i < stringLength; i++)
        {
            randValue = rand.Next(0, 26);
            letter = Convert.ToChar(randValue + 65);
            str = str + letter;
        }
        return str;
    }

    public static Vector2f Clamp(this Vector2f input, Vector2f min, Vector2f max)
    {
        return new Vector2f(Math.Clamp(input.X, min.X, max.X), Math.Clamp(input.Y, min.Y, max.Y));
    }

    public static float ClosestToZero(float a, float b)
    {
        return MathF.Min(MathF.Abs(a), MathF.Abs(b));
    }

    /// <summary>
    /// Maps Value Source to Target
    /// </summary>
    public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }


    public static float EasingFunction(float t, EasingType type)
    {
        switch (type)
        {
            case EasingType.Linear:
                return t;
            case EasingType.EaseIn:
                return t * t;
            case EasingType.EaseOut:
                return 1 - (1 - t) * (1 - t);
            case EasingType.EaseInOut:
                return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
            case EasingType.Cos:
                return (-MathF.Cos(2 * MathF.PI * t)) / 2 + 0.5f;
            case EasingType.QuadraticBounce:
                return -4 * t * (t - 1);
            case EasingType.SteepOut:
                return t * t * t - 3 * t * t + 3 * t;
            default:
                return t;
        }
    }
}

public enum EasingType
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut,
    Cos,
    QuadraticBounce,
    SteepOut
}