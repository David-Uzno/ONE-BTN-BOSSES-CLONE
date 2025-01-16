using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExecutor : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private TextAsset _levelJson;

    [Header("Attacks")]
    [SerializeField] private GameObject _trianglePrefab;

    void Start()
    {
        MoveFactory.SetMovement();
        MoveFactory.SetTriangle(_trianglePrefab);

        _levelLoader.LoadLevel(_levelJson);

        if (_levelLoader.CurrentLevel != null)
        {
            StartCoroutine(ExecuteLevel(_levelLoader.CurrentLevel));
        }
        else
        {
            Debug.LogError("No se pudo cargar el nivel. Revisa el archivo JSON.");
        }
    }

    IEnumerator ExecuteLevel(LevelLoader.Level level)
    {
        foreach (var move in level.Moves)
        {
            yield return new WaitForSeconds(move.Tick);
            ExecuteMove(move);
        }
    }

    void ExecuteMove(LevelLoader.Move move)
    {
        var action = MoveFactory.GetAction(move.Type);
        if (action != null)
        {
            action.Execute(move);
        }
        else
        {
            Debug.LogWarning($"No se encontró una acción para el tipo: {move.Type}");
        }
    }
}
