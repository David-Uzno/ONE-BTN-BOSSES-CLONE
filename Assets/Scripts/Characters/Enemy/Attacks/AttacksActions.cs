using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMoveAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        Debug.Log("Ejecutando TutorialMove");
        // Lógica
    }
}

public class BossInitializerAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        Debug.Log($"Inicializando Boss con escala {move.EnemyScale} en {move.TimeToScale} segundos");
        // Lógica
    }
}

public class TriangleAction : IMoveAction
{
    private GameObject _trianglePrefab;
    private Transform _spawnParent;

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

        for (int i = 0; i < move.Count; i++)
        {
            var triangle = Object.Instantiate(_trianglePrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.Euler(0, 0, move.StartAngle), _spawnParent);

            Debug.Log($"Triángulo {i + 1} instanciado en posición {triangle.transform.position} con rotación {move.StartAngle}");
        }
    }
}

// Movimiento compuesto
public class TutorialDodgeAction : IMoveAction
{
    public void Execute(LevelLoader.Move move)
    {
        Debug.Log("Ejecutando TutorialDodge");
        foreach (var subMove in move.Moves)
        {
            MoveFactory.GetAction(subMove.Type)?.Execute(subMove);
        }
    }
}
