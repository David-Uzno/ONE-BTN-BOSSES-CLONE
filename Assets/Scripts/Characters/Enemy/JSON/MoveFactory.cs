using System.Collections.Generic;
using UnityEngine;

public static class MoveFactory
{
    private static readonly Dictionary<string, IMoveAction> actions = new Dictionary<string, IMoveAction>();

    private static GameObject _squarePrefab;

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

    public static void SetTriangle(GameObject squarePrefab)
    {
        _squarePrefab = squarePrefab;

        actions["Square"] = new SquareAction(_squarePrefab);
    }
}
