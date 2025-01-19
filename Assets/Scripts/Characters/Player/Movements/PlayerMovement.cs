using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Character
{
    [Header("Controls")]
    public PlayerInput _playerInput;

    [Header("Dependencies")]
    [SerializeField] protected RotationController _movementController;

    protected bool _isShootingPressed;

    protected virtual void FixedUpdate()
    {
        CheckShootingInput();
        _movementController.UpdateMovement();
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
