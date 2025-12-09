using UnityEngine;

#region Simple Attacks
public abstract class BaseAction : IMoveAction
{
    protected Transform _prefab;
    protected Transform _spawnParent;
    protected Transform _lastInstance;
    protected ObjectPool _pool;

    public BaseAction(Transform prefab, ObjectPool pool = null, Transform parent = null)
    {
        _prefab = prefab;
        _pool = pool;
        _spawnParent = parent;
    }

    protected ILevelLayout GetLayout(Vector2 fallbackCenter)
    {
        return LevelLayoutResolver.Resolve(fallbackCenter);
    }

    public abstract void Execute(LevelLoader.Move move);

    protected void HandleLastInstance()
    {
        if (_pool == null && _lastInstance != null)
        {
            Object.Destroy(_lastInstance.gameObject);
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
    public SquareAction(Transform prefab, ObjectPool pool = null, Transform parent = null) : base(prefab, pool, parent) { }

    public override void Execute(LevelLoader.Move move)
    {
        if (_prefab == null)
        {
            Debug.LogError("El Prefab del Cuadrado no está asignado.");
            return;
        }

        ILevelLayout layout = GetLayout(Vector2.zero);

        for (int i = 0; i < move.Count; i++)
        {
            float startAngle = 0.0f;
            if (move.StartAngle != null)
            {
                startAngle = move.StartAngle.GetRandomValue();
            }

            float angleRadians = Mathf.Deg2Rad * (startAngle + i * 360.0f / move.Count);
            Vector2 position = layout.GetPoint(angleRadians);

            HandleLastInstance();

            if (_pool != null)
            {
                Transform pooled = _pool.GetPooledObject(_spawnParent);
                pooled.SetPositionAndRotation(position, Quaternion.Euler(0, 0, startAngle));
                pooled.gameObject.SetActive(true);
                ConfigureInstance(pooled.gameObject);
            }
            else
            {
                _lastInstance = Object.Instantiate(_prefab, position, Quaternion.Euler(0, 0, startAngle), _spawnParent);
                ConfigureInstance(_lastInstance.gameObject);
            }
        }
    }
}

public class TriangleAction : BaseAction
{
    public TriangleAction(Transform prefab, ObjectPool pool = null, Transform parent = null) : base(prefab, pool, parent) { }

    public override void Execute(LevelLoader.Move move)
    {
        if (_prefab == null)
        {
            Debug.LogError("El Prefab del Triangulo no está asignado.");
            return;
        }

        HandleLastInstance();

        ILevelLayout layout = GetLayout(Vector2.zero);
        float startAngle = 0.0f;
        if (move.StartAngle != null)
        {
            startAngle = move.StartAngle.GetRandomValue();
        }

        for (int i = 0; i < move.Count; i++)
        {
            float angle = startAngle + i * (360.0f / move.Count);
            float angleRadians = Mathf.Deg2Rad * angle;

            Vector2 position = layout.GetPoint(angleRadians);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            if (_pool != null)
            {
                Transform pooled = _pool.GetPooledObject(_spawnParent);
                pooled.SetPositionAndRotation(position, rotation);
                pooled.gameObject.SetActive(true);
                ConfigureInstance(pooled.gameObject);
            }
            else
            {
                Transform newInstance = Object.Instantiate(_prefab, position, rotation, _spawnParent);
                ConfigureInstance(newInstance.gameObject);
            }
        }
    }
}

public class StraightProjectile : IMoveAction
{
    private Transform _projectilePrefab;
    private Transform _spawnParent;
    private ObjectPool _pool;

    public StraightProjectile(Transform prefab, ObjectPool pool = null, Transform parent = null)
    {
        _projectilePrefab = prefab;
        _pool = pool;
        _spawnParent = parent;
    }

    public void Execute(LevelLoader.Move move)
    {
        if (_projectilePrefab == null)
        {
            Debug.LogError("El Prefab del Proyectil no está asignado.");
            return;
        }

        Vector3 startPosition = new(0, 0, 0);

        float angle = 0.0f;
        if (move.StartAngle != null)
        {
            angle = move.StartAngle.GetRandomValue();
        }

        ILevelLayout layout = LevelLayoutResolver.Resolve(Vector2.zero);

        for (int i = 0; i < move.Count; i++)
        {
            if (_pool != null)
            {
                Transform pooled = _pool.GetPooledObject(_spawnParent);
                pooled.SetPositionAndRotation(startPosition, Quaternion.Euler(0, 0, angle));
                pooled.gameObject.SetActive(true);

                if (pooled.TryGetComponent(out BulletEnemy bullet))
                {
                    Vector2 direction = new(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                    bullet.SetMovementDirection(direction);
                    bullet.AssignLayout(layout);
                }
            }
            else
            {
                Transform projectileInstance = Object.Instantiate(_projectilePrefab, startPosition, Quaternion.Euler(0, 0, angle), _spawnParent);

                if (projectileInstance.TryGetComponent(out BulletEnemy bullet))
                {
                    Vector2 direction = new(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                    bullet.SetMovementDirection(direction);
                    bullet.AssignLayout(layout);
                }
            }
        }
    }
}
#endregion

#region Compound Attacks
public class TutorialMoveAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        //Debug.Log("Ejecutando TutorialMove");
    }
}

public class TutorialDodgeAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        //Debug.Log("Ejecutando TutorialDodge");
    }
}
#endregion