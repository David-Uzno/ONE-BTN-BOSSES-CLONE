using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private int _poolSize = 5;

    private readonly Queue<Transform> _pool = new();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            Transform bullet = Instantiate(_bulletPrefab, transform);
            bullet.name = $"Bullet_{i}";
            bullet.gameObject.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }

    public Transform GetBulletFromPool()
    {
        Transform bullet;
        if (_pool.Count > 0)
        {
            bullet = _pool.Dequeue();
            bullet.SetParent(null);
        }
        else
        {
            bullet = Instantiate(_bulletPrefab, transform);
            bullet.name = $"Bullet_{_poolSize + _pool.Count}";
            bullet.gameObject.SetActive(false);
            bullet.SetParent(null);
        }
        ResetBullet(bullet);

        return bullet;
    }

    private void ResetBullet(Transform bullet)
    {
        bullet.localRotation = Quaternion.identity;
        if (bullet.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }
    }

    public void ReturnBulletToPool(Transform bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.SetParent(transform);
        bullet.localRotation = Quaternion.identity;
        _pool.Enqueue(bullet);
    }
}
