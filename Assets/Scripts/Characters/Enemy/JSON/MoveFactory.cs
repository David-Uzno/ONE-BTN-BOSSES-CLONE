using System.Collections.Generic;
using UnityEngine;

public static class MoveFactory
{
    private static readonly Dictionary<string, IMoveAction> actions = new();

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

    public static void SetSquare(GameObject squarePrefab, ObjectPool pool = null)
    {
        _squarePrefab = squarePrefab;

        actions["Square"] = new SquareAction(_squarePrefab, pool);
    }

    public static void SetTriangle(GameObject trianglePrefab, ObjectPool pool = null)
    {
        _trianglePrefab = trianglePrefab;

        actions["Triangle"] = new TriangleAction(_trianglePrefab, pool);
    }

    public static void SetStraightProjectile(GameObject projectilePrefab, ObjectPool pool = null)
    {
        _projectilePrefab = projectilePrefab;
        actions["StraightProjectile"] = new StraightProjectile(_projectilePrefab, pool);
    }
}
