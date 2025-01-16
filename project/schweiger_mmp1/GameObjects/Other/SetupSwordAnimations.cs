/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */


// This file contains all the logic to make the sword animations.

using SFML.System;

public static class SetupSwordAnimation
{
    private static float swordDistance = 50;

    public static void SetupSwordAnimations(Sword s)
    {
        SetupSlashDownAnimations(s);
        SetupSlashDownAnimations(s, -1);
        SetupSlashRightAnimations(s);
        SetupSlashRightAnimations(s, -1);
        SetupSlashLeftAnimations(s);
        SetupSlashLeftAnimations(s, -1);
        SetupSlashUpAnimations(s);
        SetupSlashUpAnimations(s, -1);
    }

    private static void SetupSlashDownAnimations(Sword s, int direction = 1)
    {
        string name = direction > 0 ? "slash0down" : "slash1down";
        float sword0pos = -510; float sword1pos = 20;
        float sword0rot = 289; float sword1rot = 71;
        Vector2f hand0pos = new(40, -182); Vector2f hand1pos = new(-50, -102);

        SetupSlashAnimations(
            animationName: name,
            targetObject: s.slash,
            startPosition: new(-30 * direction, 230 + swordDistance),
            endPosition: new(-30 * direction, 230 + swordDistance),
            startRotation: 0,
            endRotation: 0,
            duration: 1.15f / 3
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.player.hand,
            startPositions: [direction > 0 ? hand0pos : hand1pos],
            endPositions: [direction > 0 ? hand0pos + new Vector2f(5, -5) : hand1pos + new Vector2f(5, -5)],
            startRotations: [],
            endRotations: [],
            durations: [1.4f / 4]
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.player.head,
            startPosition: new(25 * direction, 0),
            endPosition: new(31 * direction, 0),
            startRotation: 8 * direction,
            endRotation: 10 * direction,
            duration: 1.2f / 4
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.sword,
            startPositions: [],
            endPositions: [new(direction > 0 ? sword0pos : sword1pos, 68 + swordDistance), new(direction > 0 ? sword0pos : sword1pos, 63 + swordDistance)],
            startRotations: [],
            endRotations: [direction > 0 ? sword0rot : sword1rot, direction > 0 ? sword0rot + 2 : sword1rot - 2],
            durations: [0f, 1f / 4]
        );
    }

    private static void SetupSlashRightAnimations(Sword s, int direction = 1)
    {
        string name = direction > 0 ? "slash0right" : "slash1right";
        float sword0pos = 218; float sword1pos = -468;
        float sword0rot = 199; float sword1rot = -19;
        Vector2f hand0pos = new(-40, -30); Vector2f hand1pos = new(0, 100);
        float slash0pos = -20; float slash1pos = -220;

        SetupSlashAnimations(
            animationName: name,
            targetObject: s.slash,
            startPosition: new(220 + swordDistance, direction > 0 ? slash0pos : slash1pos),
            endPosition: new(220 + swordDistance, direction > 0 ? slash0pos : slash1pos),
            startRotation: -90,
            endRotation: -90,
            duration: 1.15f / 3
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.player.hand,
            startPositions: [(direction > 0 ? hand0pos : hand1pos)],
            endPositions: [(direction > 0 ? hand0pos : hand1pos) + new Vector2f(-5, -5)],
            startRotations: [],
            endRotations: [],
            durations: [1.4f / 4]
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.player.head,
            startPosition: new(-25, 0),
            endPosition: new(-31, 0),
            startRotation: -8,
            endRotation: -10,
            duration: 1.2f / 4
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.sword,
            startPositions: [],
            endPositions: [new(-180 + swordDistance, direction > 0 ? sword0pos : sword1pos), new(-180 + swordDistance, direction > 0 ? sword0pos - 5 : sword1pos + 5)],
            startRotations: [],
            endRotations: [direction > 0 ? sword0rot : sword1rot, direction > 0 ? sword0rot + 2 : sword1rot - 2],
            durations: [0f, 1f / 4]
        );
    }

    private static void SetupSlashUpAnimations(Sword s, int direction = 1)
    {
        string name = direction > 0 ? "slash0up" : "slash1up";
        float sword0pos = 30; float sword1pos = -510;
        float sword0rot = 109; float sword1rot = -109;
        Vector2f hand0pos = new(450, 0); Vector2f hand1pos = new(505, -50);

        SetupSlashAnimations(
            animationName: name,
            targetObject: s.slash,
            startPosition: new(30 * direction, -390 - swordDistance),
            endPosition: new(30 * direction, -390 - swordDistance),
            startRotation: 180,
            endRotation: 180,
            duration: 1.15f / 3
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.player.hand,
            startPositions: [direction > 0 ? hand0pos : hand1pos, new(270, -130)],
            endPositions: [direction > 0 ? hand0pos + new Vector2f(0, 5) : hand1pos + new Vector2f(0, 5), new(275, -135)],
            startRotations: [],
            endRotations: [],
            durations: [1.3f / 4, 0.1f / 4]
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.player.head,
            startPosition: new(-25 * direction, 0),
            endPosition: new(-31 * direction, 0),
            startRotation: -8 * direction,
            endRotation: -10 * direction,
            duration: 1.2f / 4
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.sword,
            startPositions: [],
            endPositions: [new(direction > 0 ? sword0pos : sword1pos, -232 - swordDistance), new(direction > 0 ? sword0pos : sword1pos, -237 - swordDistance)],
            startRotations: [],
            endRotations: [direction > 0 ? sword0rot : sword1rot, direction > 0 ? sword0rot + 2 : sword1rot - 2],
            durations: [0f, 1f / 4]
        );
    }

    private static void SetupSlashLeftAnimations(Sword s, int direction = 1)
    {
        string name = direction > 0 ? "slash0left" : "slash1left";
        float sword0pos = -412; float sword1pos = 282;
        float sword0rot = 379; float sword1rot = -379 + 180 + 360;
        Vector2f hand0pos = new(500, -50); Vector2f hand1pos = new(525, -150);
        float slash0pos = -170; float slash1pos = 50;

        SetupSlashAnimations(
            animationName: name,
            targetObject: s.slash,
            startPosition: new(-220 - swordDistance, direction > 0 ? slash0pos : slash1pos),
            endPosition: new(-220 - swordDistance, direction > 0 ? slash0pos : slash1pos),
            startRotation: 90,
            endRotation: 90,
            duration: 1.15f / 3
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.player.hand,
            startPositions: [direction > 0 ? hand0pos : hand1pos, new(270, -130)],
            endPositions: [(direction > 0 ? hand0pos : hand1pos) + new Vector2f(5, -5), new(275, -135)],
            startRotations: [],
            endRotations: [],
            durations: [1.3f / 4, 0.1f / 4]
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.player.head,
            startPosition: new(25, 0),
            endPosition: new(31, 0),
            startRotation: 8,
            endRotation: 10,
            duration: 1.2f / 4
        );
        SetupSlashAnimations(
            animationName: name,
            targetObject: s.sword,
            startPositions: [],
            endPositions: [
                new(-300 - swordDistance, direction > 0 ? sword0pos : sword1pos),
                new(-300 - swordDistance, direction > 0 ? sword0pos-5 : sword1pos+5),
                direction > 0 ? new(-460 - swordDistance, 137) : new(-300 - swordDistance, sword1pos+5)],
            startRotations: [],
            endRotations: [
                direction > 0 ? sword0rot : sword1rot,
                direction > 0 ? sword0rot+2 : sword1rot-2,
                direction > 0 ? 246 : sword1rot-2],
            durations: [0f, direction > 0 ? .9f / 4 : 1f / 4, direction > 0 ? .1f / 4 : 0]
        );
    }

    private static void SetupSlashAnimations(string animationName, GameObject targetObject, Vector2f[] startPositions, Vector2f[] endPositions, float[] startRotations, float[] endRotations, float[] durations)
    {
        Animation animation = new Animation(targetObject.GetComponent<Animator>()!, false);

        for (int i = 0; i < durations.Length; i++)
        {
            if (startPositions.Length <= i && startRotations.Length <= i)
                animation.AddTween(durations.Length <= i ? 0 : durations[i], endPositions.Length <= i ? new() : endPositions[i], endRotations.Length <= i ? 0 : endRotations[i]);
            else
                animation.AddTween(new Tween(startPositions.Length <= i ? null : startPositions[i], endPositions.Length <= i ? null : endPositions[i], startRotations.Length <= i ? 0 : startRotations[i], endRotations.Length <= i ? 0 : endRotations[i], durations.Length <= i ? 0 : durations[i]));
        }

        targetObject.GetComponent<Animator>()?.AddAnimation(animationName, animation);
    }

    private static void SetupSlashAnimations(string animationName, GameObject targetObject, Vector2f startPosition = new(), Vector2f endPosition = new(), float startRotation = 0, float endRotation = 0, float duration = 0)
    {
        Animation animation = new Animation(targetObject.GetComponent<Animator>()!, false);
        animation.AddTween(new Tween(startPosition, endPosition, startRotation, endRotation, duration));
        targetObject.GetComponent<Animator>()?.AddAnimation(animationName, animation);
    }
}