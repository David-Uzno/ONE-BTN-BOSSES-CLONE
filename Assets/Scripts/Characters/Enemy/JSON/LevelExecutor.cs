using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExecutor : MonoBehaviour
{
    [Header("Files for Loading")]
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private TextAsset _levelJSON;

    [System.Serializable]
    public struct PoolConfig
    {
        public string key;
        public GameObject prefab;
        public int poolSize;
        [HideInInspector] public ObjectPool pool;
    }

    [Header("Attack Pools")]
    [SerializeField] private List<PoolConfig> _attackPools = new();

    private void Start()
    {
        CreatePools();
        InitializeMoveFactory();
        LoadAndExecuteLevel();
    }

    private void CreatePools()
    {
        for (int poolIndex = 0; poolIndex < _attackPools.Count; poolIndex++)
        {
            PoolConfig poolConfig = _attackPools[poolIndex];

            // Crear GameObject contenedor para el pool
            GameObject poolContainer = new(poolConfig.key + "PoolContainer");
            poolContainer.transform.SetParent(transform);

            // Crear ObjectPool como hijo del contenedor
            ObjectPool createdPool = poolContainer.AddComponent<ObjectPool>();
            createdPool.Initialize(poolConfig.prefab.transform, poolConfig.poolSize, poolContainer.transform);

            poolConfig.pool = createdPool;
            _attackPools[poolIndex] = poolConfig;
        }
    }

    private void InitializeMoveFactory()
    {
        MoveFactory.SetMovement();
        foreach (PoolConfig poolConfig in _attackPools)
        {
            switch (poolConfig.key)
            {
                case "Square":
                    MoveFactory.SetSquare(poolConfig.prefab, poolConfig.pool);
                    break;
                case "Triangle":
                    MoveFactory.SetTriangle(poolConfig.prefab, poolConfig.pool);
                    break;
                case "StraightProjectile":
                    MoveFactory.SetStraightProjectile(poolConfig.prefab, poolConfig.pool);
                    break;
            }
        }
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
        IMoveAction moveAction = MoveFactory.GetAction(move.Type);
        if (moveAction != null)
        {
            moveAction.Execute(move);
        }
        else
        {
            Debug.LogWarning($"No se encontró una acción para el tipo: {move.Type}");
        }
    }
}
