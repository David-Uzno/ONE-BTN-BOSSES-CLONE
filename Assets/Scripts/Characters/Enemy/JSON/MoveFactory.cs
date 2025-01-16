using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveFactory
{
    private static readonly Dictionary<string, IMoveAction> actions = new Dictionary<string, IMoveAction>();

    private static GameObject _trianglePrefab;

    public static IMoveAction GetAction(string moveType)
    {
        if (actions.ContainsKey(moveType))
        {
            return actions[moveType];
        }

        return null;
    }

    public static void SetMovement()
    {
        actions["TutorialMove"] = new TutorialMoveAction();
        actions["BossInitializer"] = new BossInitializerAction();
        actions["TutorialDodge"] = new TutorialDodgeAction();
    }

    public static void SetTriangle(GameObject trianglePrefab)
    {
        _trianglePrefab = trianglePrefab;

        actions["Triangle"] = new TriangleAction(_trianglePrefab);
    }
}
