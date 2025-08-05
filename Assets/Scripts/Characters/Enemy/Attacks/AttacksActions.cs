using UnityEngine;

#region Simple Attacks
public class TutorialMoveAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        Debug.Log("Ejecutando TutorialMove");
    }
}

public abstract class BaseAction : IMoveAction
{
    protected GameObject _prefab;
    protected Transform _spawnParent;
    protected GameObject _lastInstance;

    public BaseAction(GameObject prefab, Transform parent = null)
    {
        _prefab = prefab;
        _spawnParent = parent;
    }

    public abstract void Execute(LevelLoader.Move move);

    protected void HandleLastInstance()
    {
        if (_lastInstance != null)
        {
            Object.Destroy(_lastInstance);
        }
    }

    protected void ConfigureInstance(GameObject instance)
    {
        if (instance == null)
        {
            Debug.LogError("El objeto instanciado es nulo.");
        }
    }
}

public class SquareAction : BaseAction
{
    public SquareAction(GameObject prefab, Transform parent = null) : base(prefab, parent) { }

    public override void Execute(LevelLoader.Move move)
    {
        if (_prefab == null)
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

            HandleLastInstance();
            _lastInstance = Object.Instantiate(_prefab, position, Quaternion.Euler(0, 0, startAngle), _spawnParent);
            ConfigureInstance(_lastInstance);
        }
    }
}

public class TriangleAction : BaseAction
{
    public TriangleAction(GameObject prefab, Transform parent = null) : base(prefab, parent) { }

    public override void Execute(LevelLoader.Move move)
    {
        if (_prefab == null)
        {
            Debug.LogError("El Prefab del Triangulo no está asignado.");
            return;
        }

        HandleLastInstance();

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

            GameObject newInstance = Object.Instantiate(_prefab, position, rotation, _spawnParent);
            ConfigureInstance(newInstance);
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
