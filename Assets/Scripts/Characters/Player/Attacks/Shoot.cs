using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Components")]
    protected Transform _bullet;
    [SerializeField] protected Transform _shootingPosition;
    private ObjectPool _objectPool;

    [Header("Pool Settings")]
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private int _poolSize = 4;

    [Header("Time of Shoot")]
    [SerializeField] protected float _shotCooldown;
    protected float _shotTime = 0;

    private void Awake()
    {
        if (_objectPool == null)
        {
            _objectPool = GetComponent<ObjectPool>() ?? FindFirstObjectByType<ObjectPool>();
            if (_objectPool == null)
            {
                Debug.LogError("No se encontró un ObjectPool en la escena.");
                return;
            }
        }
        _objectPool.Initialize(_bulletPrefab, _poolSize, _objectPool.transform);
    }

    private void Update()
    {
        Fire();
    }

    protected virtual void Fire()
    {
        if (CanShoot())
        {
            Transform bullet = _objectPool.GetPooledObject(_shootingPosition);
            if (bullet == null) return;

            bullet.SetPositionAndRotation(_shootingPosition.position, _shootingPosition.rotation);
            bullet.gameObject.SetActive(true);

            _shotTime = Time.time + _shotCooldown;
        }
    }

    protected bool CanShoot()
    {
        return Time.time > _shotTime;
    }
}
