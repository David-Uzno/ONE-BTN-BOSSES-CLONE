using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] public float _initialSpeed = 2.5f;
    [SerializeField] private Vector2 _rotationPoint = Vector2.zero;
    [SerializeField] private float _openingFactor = 10f;

    private float _angleRadians;
    private float _radiusInitial;
    public float _currentSpeed;

    public int _rotationDirection = 1;

    private void Start()
    {
        // Calcular el radio inicial
        if (_rotationPoint == Vector2.zero)
        {
            float distance = Vector2.Distance(transform.position, Vector2.zero);

            if (distance > 0f)
            {
                _radiusInitial = distance * _openingFactor;
            }
            else
            {
                _radiusInitial = _openingFactor;
            }
        }
        else
        {
            _radiusInitial = Vector2.Distance(transform.position, _rotationPoint) * _openingFactor;
        }

        // Calcular el ángulo inicial
        _angleRadians = Mathf.Atan2(transform.position.y - _rotationPoint.y, transform.position.x - _rotationPoint.x);

        // Ajustar la posición inicial
        float x = _rotationPoint.x + Mathf.Cos(_angleRadians) * _radiusInitial;
        float y = _rotationPoint.y + Mathf.Sin(_angleRadians) * _radiusInitial;
        transform.position = new Vector2(x, y);

        _currentSpeed = _initialSpeed;
    }

    public void UpdateMovement(float deltaTime, Transform playerTransform)
    {
        // Incrementar ángulo
        _angleRadians += _currentSpeed * _rotationDirection * deltaTime;

        // Calcular nueva posición
        float x = _rotationPoint.x + Mathf.Cos(_angleRadians) * _radiusInitial;
        float y = _rotationPoint.y + Mathf.Sin(_angleRadians) * _radiusInitial;
        playerTransform.position = new Vector2(x, y);

        // Calcular rotación
        Vector2 directionToCenter = (_rotationPoint - (Vector2)playerTransform.position).normalized;
        float rotationAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        playerTransform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90f);
    }
}
