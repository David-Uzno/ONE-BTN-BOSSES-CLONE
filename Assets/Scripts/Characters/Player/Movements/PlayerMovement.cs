using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Controls")]
    public PlayerInput _playerInput;
    [SerializeField] protected RotationController _movementController;

    protected bool _isShootingPressed;

    protected virtual void FixedUpdate()
    {
        CheckShootingInput();
        if (_movementController != null && _movementController.IsInitialized)
        {
            _movementController.UpdateMovement();
        }
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
