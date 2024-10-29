using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header ("Rotation")]
    [SerializeField] private Vector2 _rotationPoint = Vector2.zero;
    [SerializeField] private float _openingFactor = 1f;
    [SerializeField] private float _initialSpeed = 5f;

    [Header("Controls")]
    [SerializeField] private PlayerInput _playerInput;
    private bool _isShootingPressed;

    private float _currentSpeed;
    private int _rotationDirection = 1;
    private float _angleRadians;
    private float _radiusInitial;

    void Start()
    {
        _currentSpeed = _initialSpeed;

        // Calcular el radio inicial
        _radiusInitial = Vector2.Distance(transform.position, _rotationPoint) * _openingFactor;
        _angleRadians = Mathf.Atan2(transform.position.y - _rotationPoint.y, transform.position.x - _rotationPoint.x);
    }

    void FixedUpdate()
    {
        CheckChangeDirection();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        // Incremento del ángulo
        _angleRadians += _currentSpeed * _rotationDirection * Time.fixedDeltaTime;

        // Cálculo de la nueva posición
        float x = _rotationPoint.x + Mathf.Cos(_angleRadians) * _radiusInitial;
        float y = _rotationPoint.y + Mathf.Sin(_angleRadians) * _radiusInitial;

        transform.position = new Vector2(x, y);
    }

    private void CheckChangeDirection()
    {
        bool isCurrentShootingPressed = _playerInput.actions["Shoot"].ReadValue<float>() > 0;

        if (isCurrentShootingPressed && !_isShootingPressed)
        {
            ChangeOrientation();
        }

        _isShootingPressed = isCurrentShootingPressed;
    }

    public void ChangeOrientation()
    {
        _rotationDirection *= -1;
        _currentSpeed = _initialSpeed; 
    }
}
