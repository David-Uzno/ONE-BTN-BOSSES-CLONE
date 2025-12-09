using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Transform _pooledObjectPrefab;
    private int _initialPoolSize;
    private Transform _poolParent;

    private readonly Queue<Transform> _availableObjects = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Initialize(Transform prefab, int poolSize, Transform poolParent)
    {
        _pooledObjectPrefab = prefab;
        _initialPoolSize = poolSize;
        _poolParent = poolParent;
        CreateInitialPool();
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            Transform pooledObject = Instantiate(_pooledObjectPrefab, _poolParent);
            pooledObject.name = $"{_pooledObjectPrefab.name}_{i}";
            pooledObject.gameObject.SetActive(false);
            _availableObjects.Enqueue(pooledObject);
        }
    }

    public Transform GetPooledObject(Transform parent = null)
    {
        Transform pooledObject;
        if (_availableObjects.Count > 0)
        {
            pooledObject = _availableObjects.Dequeue();
        }
        else
        {
            pooledObject = Instantiate(_pooledObjectPrefab, _poolParent);
            pooledObject.name = $"{_pooledObjectPrefab.name}_{_initialPoolSize + _availableObjects.Count}";
            pooledObject.gameObject.SetActive(false);
        }
        pooledObject.SetParent(_poolParent);
        ResetPooledObject(pooledObject);

        return pooledObject;
    }

    private void ResetPooledObject(Transform pooledObject)
    {
        pooledObject.localRotation = Quaternion.identity;
        if (pooledObject.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }
    }

    public void ReturnObjectToPool(Transform pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.SetParent(_poolParent);
        pooledObject.localRotation = Quaternion.identity;
        _availableObjects.Enqueue(pooledObject);
    }
}
