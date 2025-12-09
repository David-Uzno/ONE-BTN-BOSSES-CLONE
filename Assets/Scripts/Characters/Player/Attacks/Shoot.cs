using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Components")]
    protected Transform _bullet;
    [SerializeField] protected Transform _shootingPosition;

    [Header("Pool Settings")]
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private int _poolSize = 5;

    [Header("Time of Shoot")]
    [SerializeField] protected float _shotCooldown;
    protected float _shotTime = 0;

    private void Awake()
    {
        if (ObjectPool.Instance == null)
        {
            Debug.LogError("No se encontró una instancia de ObjectPool en la escena.");
            return;
        }
        ObjectPool.Instance.Initialize(_bulletPrefab, _poolSize, transform);
    }

    private void Update()
    {
        Fire();
    }

    protected virtual void Fire()
    {
        if (CanShoot())
        {
            Transform bullet = ObjectPool.Instance.GetPooledObject(_shootingPosition);
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
