using System.Collections;
using TMPro;
using UnityEngine;

public class DashPlayer : PlayerMovement
{
    [Header("Dash Collider")]
    [SerializeField] private Collider2D _collider;

    [Header("Dash Counter")]
    [SerializeField] private byte _counter = 100;
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private float _counterDelay = 0.025f;

    [Header("Dash Speed")]
    [SerializeField] private float _counterIncreasedSpeed = 2.0f;
    [SerializeField] private float _counterDecreaseSpeed = 1.0f;
    [SerializeField] private float _invulnerabilityTime = 0.5f;

    private float _initialSpeed;

    private Coroutine _increaseDashCoroutine;
    private Coroutine _decreaseDashCoroutine;
    private Coroutine _invulnerabilityCoroutine;

    private void Start()
    {
        if (_movementController != null)
        {
            _initialSpeed = _movementController._initialSpeed;
        }
        else
        {
            _initialSpeed = 1.0f; // Valor por defecto para pruebas
        }
    }

    protected override void Movement()
    {
        if (_counter >= 1)
        {
            _collider.enabled = false;
            _movementController._currentSpeed *= _counterIncreasedSpeed;

            if (_decreaseDashCoroutine != null)
            {
                StopCoroutine(_decreaseDashCoroutine);
            }

            if (_increaseDashCoroutine != null)
            {
                StopCoroutine(_increaseDashCoroutine);
                _increaseDashCoroutine = null;
            }

            _decreaseDashCoroutine = StartCoroutine(DecreaseDashCounter());
        }
    }

    private IEnumerator DecreaseDashCounter()
    {
        while (_counter > 0)
        {
            _counter--;
            if (_counterText != null)
            {
                _counterText.text = _counter.ToString();
            }

            if (_counter == 0)
            {
                StopMovement();
            }

            yield return new WaitForSeconds(_counterDelay / _counterDecreaseSpeed);
        }
    }

    protected override void StopMovement()
    {
        _movementController._currentSpeed = _initialSpeed;

        if (_decreaseDashCoroutine != null)
        {
            StopCoroutine(_decreaseDashCoroutine);
            _decreaseDashCoroutine = null;
        }

        if (_increaseDashCoroutine == null)
        {
            _increaseDashCoroutine = StartCoroutine(IncreaseDashCounter());
        }

        if (_invulnerabilityCoroutine != null)
        {
            StopCoroutine(_invulnerabilityCoroutine);
        }
        _invulnerabilityCoroutine = StartCoroutine(InvulnerabilityCoroutine());
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_invulnerabilityTime);
        _collider.enabled = true;
        _invulnerabilityCoroutine = null;
    }

    private IEnumerator IncreaseDashCounter()
    {
        while (_counter < 100)
        {
            _counter++;
            if (_counterText != null)
            {
                _counterText.text = _counter.ToString();
            }
            yield return new WaitForSeconds(_counterDelay);
        }
        _increaseDashCoroutine = null;
    }
}
