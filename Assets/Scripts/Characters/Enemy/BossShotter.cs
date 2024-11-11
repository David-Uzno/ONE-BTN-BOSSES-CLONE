using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : Shooter
{
    [Header("Boss Attack Settings")]
    [SerializeField] private float _spawnRadius = 1.5f;

    protected override void Fire()
    {
        if (CanShoot())
        {
            // Genera una posici�n aleatoria cercana al jefe
            Vector3 spawnPosition = transform.position + (Vector3)Random.insideUnitCircle * _spawnRadius;

            // Instancia el proyectil en la posici�n calculada
            Instantiate(_bullet, spawnPosition, transform.rotation);

            _shotTime = Time.time + _shotCooldown;
        }
    }
}

