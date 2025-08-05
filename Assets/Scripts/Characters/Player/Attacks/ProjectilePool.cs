using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private int _poolSize = 5;

    private Queue<Transform> _pool = new Queue<Transform>();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            Transform bullet = Instantiate(_bulletPrefab);
            bullet.name = $"Bullet_{i}";
            bullet.gameObject.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }

    public Transform GetBulletFromPool()
    {
        if (_pool.Count > 0)
        {
            Transform bullet = _pool.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        Transform newBullet = Instantiate(_bulletPrefab);
        newBullet.gameObject.SetActive(true);
        return newBullet;
    }

    public void ReturnBulletToPool(Transform bullet)
    {
        bullet.gameObject.SetActive(false);
        _pool.Enqueue(bullet);
    }
}