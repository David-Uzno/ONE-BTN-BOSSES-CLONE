using UnityEngine;

#region Simple Attacks
public class TutorialMoveAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        Debug.Log("Ejecutando TutorialMove");
    }
}

public class BossInitializerAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        Debug.Log($"Inicializando Boss con escala {move.EnemyScale} en {move.TimeToScale} segundos");
    }
}

public class TriangleAction : IMoveAction
{
    private GameObject _trianglePrefab;
    private Transform _spawnParent;
    private GameObject _lastTriangleInstance;  

    public TriangleAction(GameObject prefab, Transform parent = null)
    {
        _trianglePrefab = prefab;
        _spawnParent = parent;
    }

    public void Execute(LevelLoader.Move move)
    {
        if (_trianglePrefab == null)
        {
            Debug.LogError("El Prefab del Triángulo no está asignado.");
            return;
        }

        CircularPath circularPath = new CircularPath(Vector2.zero);

        for (int i = 0; i < move.Count; i++)
        {
            float angleRadians = Mathf.Deg2Rad * (move.StartAngle + i * 360.0f / move.Count);
            Vector2 position = circularPath.GetPosition(angleRadians);

            HandleLastTriangleInstance();
            _lastTriangleInstance = Object.Instantiate(_trianglePrefab, position, Quaternion.Euler(0, 0, move.StartAngle), _spawnParent);
            ConfigureTriangle(_lastTriangleInstance);

            Debug.Log($"Triángulo {i + 1} instanciado en posición {position} con rotación {move.StartAngle}");
        }
    }

    private void HandleLastTriangleInstance()
    {
        if (_lastTriangleInstance != null)
        {
            Object.Destroy(_lastTriangleInstance);
        }
    }

    private void ConfigureTriangle(GameObject triangle)
    {
        if (triangle == null)
        {
            Debug.LogError("El objeto triángulo es nulo.");
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
