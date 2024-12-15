using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExecutor : MonoBehaviour
{
    [SerializeField]
    private string jsonFilePath = "Assets/Levels/";

    public LevelLoader LevelLoader;

    void Start()
    {
        LevelLoader.LoadLevel(jsonFilePath);
        StartCoroutine(ExecuteLevel(LevelLoader.CurrentLevel));
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
        switch (move.Type)
        {
            case "TutorialMove":
                Debug.Log("Executing TutorialMove");
                // Implementar lógica específica
                break;

            case "BossInitializer":
                Debug.Log($"Initializing Boss with scale {move.EnemyScale} over {move.TimeToScale}s");
                // Implementar lógica específica
                break;

            case "TutorialDodge":
                Debug.Log("Starting TutorialDodge");
                StartCoroutine(ExecuteSubMoves(move.Moves));
                break;

            case "Triangle":
                Debug.Log($"Triangle at angle {move.StartAngle}");
                // Implementar lógica específica
                break;

            default:
                Debug.LogWarning($"Unknown Move Type: {move.Type}");
                break;
        }
    }

    IEnumerator ExecuteSubMoves(List<LevelLoader.Move> moves)
    {
        foreach (var subMove in moves)
        {
            yield return new WaitForSeconds(subMove.Tick);
            ExecuteMove(subMove);
        }
    }
}
