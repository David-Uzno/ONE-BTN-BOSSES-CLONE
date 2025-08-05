using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Components")]
    protected Transform _bullet;
    [SerializeField] protected Transform _shootingPosition;
    [SerializeField] private ProjectilePool _projectilePool;

    [Header("Time of Shoot")]
    [SerializeField] protected float _shotCooldown;
    protected float _shotTime = 0;

    private void Awake()
    {
        if (_projectilePool == null)
        {
            _projectilePool = Object.FindAnyObjectByType<ProjectilePool>();
            if (_projectilePool == null)
            {
                Debug.LogError("No se encontró un ProjectilePool en la escena.");
            }
        }
    }

    private void Update()
    {
        Fire();
    }

    protected virtual void Fire()
    {
        if (CanShoot())
        {
            Transform bullet = _projectilePool.GetBulletFromPool();
            bullet.position = _shootingPosition.position;
            bullet.rotation = _shootingPosition.rotation;
            bullet.gameObject.SetActive(true);

            _shotTime = Time.time + _shotCooldown;
        }
    }

    protected bool CanShoot()
    {
        return Time.time > _shotTime;
    }
}
