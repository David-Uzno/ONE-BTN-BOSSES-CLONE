using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Transform _bullet;
    [SerializeField] protected Transform _shootingPosition;

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
            Instantiate(_bullet, _shootingPosition.position, _shootingPosition.rotation);
            _shotTime = Time.time + _shotCooldown;
        }
    }

    protected bool CanShoot()
    {
        return Time.time > _shotTime;
    }
}
