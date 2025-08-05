using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Transform _shootingPosition;
    [SerializeField] private ProjectilePool _projectilePool;

    [Header("Time of Shoot")]
    [SerializeField] protected float _shotCooldown;
    protected float _shotTime = 0;

    private void Update()
    {
        Fire();
    }

    protected virtual void Fire()
    {
        if (CanShoot())
        {
            Transform bullet = _projectilePool.GetBullet();
            bullet.position = _shootingPosition.position;
            bullet.rotation = _shootingPosition.rotation;
            _shotTime = Time.time + _shotCooldown;
        }
    }

    protected bool CanShoot()
    {
        return Time.time > _shotTime;
    }
}
