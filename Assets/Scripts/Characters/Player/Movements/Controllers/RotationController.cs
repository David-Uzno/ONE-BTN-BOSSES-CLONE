using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float _initialSpeed = 2.5f;
    [SerializeField] private float _openingFactor = 7.5f;

    [Header("Rotation")]
    [SerializeField] private Vector2 _rotationPoint = Vector2.zero;
    public sbyte _rotationDirection = 1;

    [Header("Internal Calculations")]
    public float _currentSpeed;
    private float _angleRadians;
    private float _radiusInitial;
    
    private void Start()
    {
        // Calcular el radio inicial
        float distance = Vector2.Distance(transform.position, _rotationPoint);
        _radiusInitial = Mathf.Max(distance, 1f) * _openingFactor;

        // Calcular el ángulo inicial
        _angleRadians = Mathf.Atan2(transform.position.y - _rotationPoint.y, transform.position.x - _rotationPoint.x);

        // Ajustar la posición inicial
        transform.position = _rotationPoint + new Vector2(Mathf.Cos(_angleRadians), Mathf.Sin(_angleRadians)) * _radiusInitial;

        _currentSpeed = _initialSpeed;
    }

    public void UpdateMovement()
    {
        // Incrementar ángulo
        _angleRadians += _currentSpeed * _rotationDirection * Time.fixedDeltaTime;

        // Calcular nueva posición
        transform.position = _rotationPoint + new Vector2(Mathf.Cos(_angleRadians), Mathf.Sin(_angleRadians)) * _radiusInitial;

        // Calcular rotación
        Vector2 directionToCenter = (_rotationPoint - (Vector2)transform.position).normalized;
        float rotationAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90f);
    }
}
