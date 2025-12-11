using UnityEngine;
using System.Collections;

public class TurnsPlayer : PlayerMovement
{
    [Header("Turn Slowdown")]
    [SerializeField] private float _slowdownDuration = 0.2f;
    [SerializeField] private float _slowdownFactor = 0.3f;

    private bool _isSlowingDown = false;
    private float _originalSpeed;

    protected override void Movement()
    {
        _movementController._rotationDirection *= -1;

        if (!_isSlowingDown)
        {
            StartCoroutine(SlowdownCoroutine());
        }
    }

    private IEnumerator SlowdownCoroutine()
    {
        _isSlowingDown = true;

        // Guarda la velocidad original y aplica el factor de ralentización
        _originalSpeed = _movementController._currentSpeed;
        _movementController._currentSpeed *= _slowdownFactor;

        yield return new WaitForSeconds(_slowdownDuration);

        // Restaura la velocidad original
        _movementController._currentSpeed = _originalSpeed;
        _isSlowingDown = false;
    }
}
