using UnityEngine;

public class RotationController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float _initialSpeed = 2.5f;

    [Header("Rotation")]
    [SerializeField] private Vector2 _rotationPoint = Vector2.zero;
    public sbyte _rotationDirection = 1;

    [Header("Internal Calculations")]
    public float _currentSpeed;
    private float _angleRadians;
    private CircularPath _circularPath = new CircularPath(Vector2.zero);

    private void Start()
    {
        // Inicializar CircularPath con el radio fijo
        _circularPath = new CircularPath(_rotationPoint);

        // Calcular el ángulo inicial
        _angleRadians = _circularPath.GetAngle(transform.position);

        // Ajustar la posición inicial
        transform.position = _circularPath.GetPosition(_angleRadians);

        _currentSpeed = _initialSpeed;
    }

    public void UpdateMovement()
    {
        if (_circularPath == null)
        {
            Debug.LogError("_circularPath no está inicializado.");
            return;
        }

        // Incrementar ángulo
        _angleRadians += _currentSpeed * _rotationDirection * Time.fixedDeltaTime;

        // Calcular nueva posición
        transform.position = _circularPath.GetPosition(_angleRadians);

        // Calcular rotación
        Vector2 directionToCenter = (_rotationPoint - (Vector2)transform.position).normalized;
        float rotationAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90f);
    }
}
