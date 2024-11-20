using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnsPlayer : Character
{
    [Header("Dependencies")]
    [SerializeField] private RotationController _movementController;

    private bool _isShootingPressed;

    private void FixedUpdate()
    {
        CheckChangeDirection();
        _movementController.UpdateMovement(Time.fixedDeltaTime, transform);
    }

    private void CheckChangeDirection()
    {
        bool isCurrentShootingPressed = _playerInput.actions["Shoot"].ReadValue<float>() > 0;

        if (isCurrentShootingPressed && !_isShootingPressed)
        {
            ChangeDirection();
        }

        _isShootingPressed = isCurrentShootingPressed;
    }

    private void ChangeDirection()
    {
        _movementController._rotationDirection *= -1;
    }
}
