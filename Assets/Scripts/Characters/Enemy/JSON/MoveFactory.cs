using System.Collections.Generic;
using UnityEngine;

public static class MoveFactory
{
    private static readonly Dictionary<string, IMoveAction> actions = new Dictionary<string, IMoveAction>();

    private static GameObject _squarePrefab;
    private static GameObject _trianglePrefab;
    private static GameObject _projectilePrefab;

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
        actions["TutorialDodge"] = new TutorialDodgeAction();
    }

    public static void SetSquare(GameObject squarePrefab)
    {
        _squarePrefab = squarePrefab;

        actions["Square"] = new SquareAction(_squarePrefab);
    }

    public static void SetTriangle(GameObject trianglePrefab)
    {
        _trianglePrefab = trianglePrefab;

        actions["Triangle"] = new TriangleAction(_trianglePrefab);
    }

    public static void SetStraightProjectile(GameObject projectilePrefab)
    {
        _projectilePrefab = projectilePrefab;
        actions["StraightProjectile"] = new StraightProjectile(_projectilePrefab);
    }
}
