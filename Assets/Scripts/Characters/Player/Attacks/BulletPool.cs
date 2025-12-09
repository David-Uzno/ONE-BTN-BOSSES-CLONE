using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    [Header("Bullet Pool Settings")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _initialPoolSize = 10;

    private Queue<GameObject> _bulletPool = new();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            GameObject bullet = CreateNewBullet();
            bullet.SetActive(false);
            _bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (_bulletPool.Count > 0)
        {
            GameObject bullet = _bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            return CreateNewBullet();
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }

    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab);
        bullet.SetActive(false);
        return bullet;
    }
}
