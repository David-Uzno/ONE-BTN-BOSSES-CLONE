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
    private float _pathParameter;
    private ILevelLayout _layout;
    private bool _isInitialized = false;

    private void Start()
    {
        _layout = LevelLayoutResolver.Resolve(_rotationPoint);

        _pathParameter = _layout.GetParameter(transform.position);

        transform.position = _layout.GetPoint(_pathParameter);

        _currentSpeed = _initialSpeed;

        _isInitialized = true;
    }

    public void UpdateMovement()
    {
        if (!_isInitialized)
        {
            Debug.LogWarning("RotationController no está inicializado.");
            return;
        }

        _pathParameter += _currentSpeed * _rotationDirection * Time.fixedDeltaTime;

        transform.position = _layout.GetPoint(_pathParameter);

        Vector2 directionToCenter = (_layout.GetCenter() - (Vector2)transform.position).normalized;
        float rotationAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90f);
    }

    public bool IsInitialized => _isInitialized;
}
