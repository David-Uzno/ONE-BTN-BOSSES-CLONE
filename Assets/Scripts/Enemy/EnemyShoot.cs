using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Transform _shootingPosition;
    [SerializeField] private Transform _bullet;

    [Header("Hora de disparar")]
    [SerializeField] private float _shotCooldown = 1.5f; 
    private float _shotTime = 0;

    private void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (CanShoot())
        {
            FireWeapon();
            _shotTime = Time.time + _shotCooldown;
        }
    }

    public void FireWeapon()
    {
        Instantiate(_bullet, _shootingPosition.position, _shootingPosition.rotation);
    }

    private bool CanShoot()
    {
        return Time.time > _shotTime;
    }
}

