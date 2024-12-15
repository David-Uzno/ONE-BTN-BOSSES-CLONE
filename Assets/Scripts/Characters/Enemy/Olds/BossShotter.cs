using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : Shooter
{
    [Header("Boss Attack Settings")]
    [SerializeField] private Vector3[] _spawnPositions; 
    [SerializeField] private float _spawnDelay = 1f;    

    protected override void Fire()
    {
        if (CanShoot() && _spawnPositions.Length > 0)
        {
            StartCoroutine(SpawnWithDelay());
            _shotTime = Time.time + _shotCooldown;
        }
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(_spawnDelay);

        Vector3 spawnPosition = _spawnPositions[Random.Range(0, _spawnPositions.Length)];

        Instantiate(_bullet, spawnPosition, transform.rotation);
    }
}

