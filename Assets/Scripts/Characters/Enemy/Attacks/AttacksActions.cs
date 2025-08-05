using System.Collections.Generic;
using UnityEngine;

#region Simple Attacks
public class TutorialMoveAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        Debug.Log("Ejecutando TutorialMove");
    }
}

public class SquareAction : IMoveAction
{
    private GameObject _squarePrefab;
    private Transform _spawnParent;
    private GameObject _lastSquareInstance;

    public SquareAction(GameObject prefab, Transform parent = null)
    {
        _squarePrefab = prefab;
        _spawnParent = parent;
    }

    public void Execute(LevelLoader.Move move)
    {
        if (_squarePrefab == null)
        {
            Debug.LogError("El Prefab del Cuadrado no está asignado.");
            return;
        }

        CircularPath circularPath = new CircularPath(Vector2.zero);

        for (int i = 0; i < move.Count; i++)
        {
            float startAngle = 0.0f;
            if (move.StartAngle != null)
            {
                startAngle = move.StartAngle.GetRandomValue();
            }

            float angleRadians = Mathf.Deg2Rad * (startAngle + i * 360.0f / move.Count);
            Vector2 position = circularPath.GetPosition(angleRadians);

            HandleLastSquareInstance();
            _lastSquareInstance = Object.Instantiate(_squarePrefab, position, Quaternion.Euler(0, 0, startAngle), _spawnParent);
            ConfigureSquare(_lastSquareInstance);

            //Debug.Log($"Cuadrado {i + 1} instanciado en posición {position} con rotación {startAngle}");
        }
    }

    private void HandleLastSquareInstance()
    {
        if (_lastSquareInstance != null)
        {
            Object.Destroy(_lastSquareInstance);
        }
    }

    private void ConfigureSquare(GameObject square)
    {
        if (square == null)
        {
            Debug.LogError("El objeto cuadrado es nulo.");
        }
    }
}

public class TriangleAction : IMoveAction
{
    private GameObject _trianglePrefab;
    private GameObject _lastTriangleInstance;

    public TriangleAction(GameObject prefab)
    {
        _trianglePrefab = prefab;
    }

    public void Execute(LevelLoader.Move move)
    {
        if (_trianglePrefab == null)
        {
            Debug.LogError("El Prefab del Triangulo no está asignado.");
            return;
        }

        HandleLastTriangleInstances();

        CircularPath circularPath = new CircularPath(Vector2.zero);
        float startAngle = 0.0f;
        if (move.StartAngle != null)
        {
            startAngle = move.StartAngle.GetRandomValue();
        }

        for (int i = 0; i < move.Count; i++)
        {
            float angle = startAngle + i * (360.0f / move.Count);
            float angleRadians = Mathf.Deg2Rad * angle;
            
            Vector2 position = circularPath.GetPosition(angleRadians);
            
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject newTriangleInstance = Object.Instantiate(_trianglePrefab, position, rotation);

            if (newTriangleInstance.transform.childCount == 0)
            {
                Debug.LogWarning("El prefab del Triangulo no tiene un hijo. Se usará el padre directamente.");
            }
        }
    }

    private void HandleLastTriangleInstances()
    {
        if (_lastTriangleInstance != null)
        {
            Object.Destroy(_lastTriangleInstance);
        }
    }
}

public class StraightProjectile : IMoveAction
{
    private GameObject _projectilePrefab;
    private Transform _spawnParent;

    public StraightProjectile(GameObject prefab, Transform parent = null)
    {
        _projectilePrefab = prefab;
        _spawnParent = parent;
    }

    public void Execute(LevelLoader.Move move)
    {
        if (_projectilePrefab == null)
        {
            Debug.LogError("El Prefab del Proyectil no está asignado.");
            return;
        }

        Vector3 startPosition = new Vector3(0, 0, 0);
        
        float angle = 0.0f;
        if (move.StartAngle != null)
        {
            angle = move.StartAngle.GetRandomValue();
        }

        CircularPath circularPath = new CircularPath(Vector2.zero);

        for (int i = 0; i < move.Count; i++)
        {
            GameObject projectileInstance = Object.Instantiate(_projectilePrefab, startPosition, Quaternion.Euler(0, 0, angle), _spawnParent);

            BulletEnemy bullet = projectileInstance.GetComponent<BulletEnemy>();
            if (bullet != null)
            {
                Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                bullet.SetDirection(direction);
                bullet.SetCircularPath(circularPath);
            }

            //Debug.Log($"Proyectil {i + 1} instanciado en posición {startPosition} con ángulo {angle}");
        }
    }
}

#endregion

#region Compound Attacks
public class TutorialDodgeAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        Debug.Log("Ejecutando TutorialDodge");
        foreach (var subMove in move.Moves)
        {
            var action = MoveFactory.GetAction(subMove.Type);

            if (action != null)
            {
                action.Execute(subMove);
            }
        }
    }
}
#endregion
