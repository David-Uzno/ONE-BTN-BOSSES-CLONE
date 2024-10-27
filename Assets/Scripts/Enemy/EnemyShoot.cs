using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Transform _bullet;
    [SerializeField] private Transform _shootingPosition;

    [Header("Time of Shoot")]
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
            Instantiate(_bullet, _shootingPosition.position, _shootingPosition.rotation);
            _shotTime = Time.time + _shotCooldown;
        }
    }

    private bool CanShoot()
    {
        return Time.time > _shotTime;
    }
}
