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

            Debug.Log($"Cuadrado {i + 1} instanciado en posición {position} con rotación {startAngle}");
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
