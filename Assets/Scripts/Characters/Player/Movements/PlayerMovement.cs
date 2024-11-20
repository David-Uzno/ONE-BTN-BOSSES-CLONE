using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Character
{
    [Header("Dependencies")]
    [SerializeField] protected RotationController _movementController;

    protected bool _isShootingPressed;

    protected virtual void FixedUpdate()
    {
        CheckShootingInput();
        _movementController.UpdateMovement(Time.fixedDeltaTime, transform);
    }

    protected void CheckShootingInput()
    {
        bool isCurrentShootingPressed = _playerInput.actions["Shoot"].ReadValue<float>() > 0;

        if (isCurrentShootingPressed && !_isShootingPressed)
        {
            Movement();
        }
        else if (!isCurrentShootingPressed && _isShootingPressed)
        {
            StopMovement();
        }

        _isShootingPressed = isCurrentShootingPressed;
    }

    protected virtual void Movement() { }

    protected virtual void StopMovement() { }
}
