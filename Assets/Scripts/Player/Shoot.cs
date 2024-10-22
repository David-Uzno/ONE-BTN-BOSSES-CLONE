using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Transform _shootingPosition;
    [SerializeField] private Transform _bullet;

    [Header("Time of Shoot")]
    [SerializeField] float _shotCooldown = 0.25f;
    float _shotTime = 0;

    private void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (_playerInput.actions["Shoot"].ReadValue<float>() > 0)
        {
            // Condición de Cooldown
            if (CanShoot())
            {
                // Instancia de la Bala
                FireWeapon();

                // Tiempo de Cooldown
                _shotTime = Time.time + _shotCooldown;
            }
        }
    }

    public void FireWeapon()
    {
        Instantiate(_bullet, _shootingPosition.position, _shootingPosition.rotation);
    }

    public bool CanShoot()
    {
        return Time.time > _shotTime;
    }
}
