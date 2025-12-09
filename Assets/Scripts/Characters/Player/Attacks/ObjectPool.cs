using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Transform _pooledObjectPrefab;
    private int _initialPoolSize;
    private Transform _poolParent;
    private bool _isInitialized;

    private readonly Queue<Transform> _availableObjects = new();

    public void Initialize(Transform prefab, int poolSize, Transform poolParent)
    {
        if (_isInitialized && _pooledObjectPrefab == prefab && _initialPoolSize == poolSize && _poolParent == poolParent)
        {
            return;
        }

        ClearPool();
        _pooledObjectPrefab = prefab;
        _initialPoolSize = poolSize;
        _poolParent = poolParent;
        CreateInitialPool();
        _isInitialized = true;
    }

    private void ClearPool()
    {
        while (_availableObjects.Count > 0)
        {
            Transform obj = _availableObjects.Dequeue();
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            Transform pooledObject = Instantiate(_pooledObjectPrefab, _poolParent);
            pooledObject.name = $"{_pooledObjectPrefab.name}_{i}";
            pooledObject.gameObject.SetActive(false);
            EnsurePooledItemComponent(pooledObject);
            _availableObjects.Enqueue(pooledObject);
        }
    }

    private void EnsurePooledItemComponent(Transform pooledObject)
    {
        if (!pooledObject.TryGetComponent(out PooledItem pooledItem))
        {
            pooledItem = pooledObject.gameObject.AddComponent<PooledItem>();
        }
        pooledItem.SetPool(this);
    }

    public Transform GetPooledObject(Transform parent = null)
    {
        if (!_isInitialized)
        {
            Debug.LogError("ObjectPool no ha sido inicializado.");
            return null;
        }

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
            EnsurePooledItemComponent(pooledObject);
        }
        pooledObject.SetParent(_poolParent);
        ResetPooledObject(pooledObject);

        return pooledObject;
    }

    private void ResetPooledObject(Transform pooledObject)
    {
        if (pooledObject.TryGetComponent(out PooledItem pooledItem))
        {
            pooledItem.ResetRecyclingFlag();
        }

        pooledObject.localRotation = Quaternion.identity;
        if (pooledObject.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }
    }

    public void ReturnObjectToPool(Transform pooledObject)
    {
        if (pooledObject == null) return;

        if (pooledObject.TryGetComponent(out PooledItem pooledItem))
        {
            pooledItem.MarkAsRecycling();
        }

        pooledObject.gameObject.SetActive(false);
        pooledObject.SetParent(_poolParent);
        pooledObject.localRotation = Quaternion.identity;
        _availableObjects.Enqueue(pooledObject);
    }
}
