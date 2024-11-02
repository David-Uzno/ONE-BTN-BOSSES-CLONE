<<<<<<< HEAD:Assets/Scripts/Player/Player.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player recibio: {damage} da�o. Le queda: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject); 
    }
}

=======
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [Header ("Rotation")]
    [SerializeField] private float _initialSpeed = 5f;
    [SerializeField] private Vector2 _rotationPoint = Vector2.zero;
    [SerializeField] private float _openingFactor = 1f;

    private float _currentSpeed;
    private int _rotationDirection = 1;
    private float _angleRadians;
    private float _radiusInitial;

    [Header("Controls")]
    [SerializeField] private PlayerInput _playerInput;
    private bool _isShootingPressed;

    public event Action<int> OnLifeLost;
    
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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (_health > 0)
        {
            if (OnLifeLost != null) 
            {
                OnLifeLost.Invoke(_health);
            }
        }
    }

    private void RotatePlayer()
    {
        // Incremento del ángulo
        _angleRadians += _currentSpeed * _rotationDirection * Time.fixedDeltaTime;

        // Cálculo de la nueva posición
        float x = _rotationPoint.x + Mathf.Cos(_angleRadians) * _radiusInitial;
        float y = _rotationPoint.y + Mathf.Sin(_angleRadians) * _radiusInitial;

        transform.position = new Vector2(x, y);

        // Cálculo del ángulo de rotación para el personaje
        Vector2 directionToCenter = (_rotationPoint - (Vector2)transform.position).normalized;
        float rotationAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90f);
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
>>>>>>> 2021584bbaf69cf008608b0adc942a34898c1554:Assets/Scripts/Characters/Player/Player.cs
