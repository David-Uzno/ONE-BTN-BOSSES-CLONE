using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExecutor : MonoBehaviour
{
    [Header("Files for Loading")]
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private TextAsset _levelJSON;

    [Header("Attack Prefabs")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _squarePrefab;
    [SerializeField] private GameObject _trianglePrefab;

    private void Start()
    {
        InitializeMoveFactory();
        LoadAndExecuteLevel();
    }

    private void InitializeMoveFactory()
    {
        MoveFactory.SetMovement();
        MoveFactory.SetSquare(_squarePrefab);
        MoveFactory.SetTriangle(_trianglePrefab);
        MoveFactory.SetStraightProjectile(_projectilePrefab);
    }

    private void LoadAndExecuteLevel()
    {
        _levelLoader.LoadLevel(_levelJSON);

        if (_levelLoader.CurrentLevel != null)
        {
            StartCoroutine(ExecuteLevel(_levelLoader.CurrentLevel));
        }
        else
        {
            Debug.LogError("No se pudo cargar el nivel. Revisa el archivo JSON.");
        }
    }

    private IEnumerator ExecuteLevel(LevelLoader.Level level)
    {
        foreach (LevelLoader.MoveGroup moveGroup in level.Moves)
        {
            yield return new WaitForSeconds(moveGroup.Tick);
            ExecuteMoveOrSubMoves(moveGroup);
        }
    }

    private void ExecuteMoveOrSubMoves(LevelLoader.MoveGroup moveGroup)
    {
        if (moveGroup.Moves != null && moveGroup.Moves.Count > 0)
        {
            StartCoroutine(ExecuteSubMoves(moveGroup.Moves));
        }
        else
        {
            ExecuteMove(moveGroup);
        }
    }

    private IEnumerator ExecuteSubMoves(List<LevelLoader.Move> subMoves)
    {
        foreach (LevelLoader.Move subMove in subMoves)
        {
            yield return new WaitForSeconds(subMove.Tick);
            ExecuteMove(subMove);
        }
    }

    private void ExecuteMove(LevelLoader.Move move)
    {
        IMoveAction action = MoveFactory.GetAction(move.Type);
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
