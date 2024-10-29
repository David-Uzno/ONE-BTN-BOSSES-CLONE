using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootPlayer : Shooter
{
    [Header("Controls")]
    [SerializeField] private PlayerInput _playerInput;

    protected override void Fire()
    {
        if (_playerInput.actions["Shoot"].ReadValue<float>() > 0)
        {
            base.Fire();
        }
    }
}
