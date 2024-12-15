using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashPlayer : PlayerMovement
{
    [Header("Dash")]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private byte _counter = 100;
    

    [Header("Dash Settings")]
    [SerializeField] private float _increasedSpeed = 2.0f;
    [SerializeField] private float _counterDelay = 0.025f;
    private float _initialSpeed;

    private Coroutine _increaseDashCoroutine;
    private Coroutine _decreaseDashCoroutine;

    void Start()
    {
        _initialSpeed = _movementController._currentSpeed;
    }

    protected override void Movement()
    {
        if (_counter >= 1)
        {
            _collider.enabled = false;
            _movementController._currentSpeed *= _increasedSpeed;

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

            if (_counter == 0)
            {
                StopMovement();
            }

            yield return new WaitForSeconds(_counterDelay);          
        }
    }

    protected override void StopMovement()
    {
        _collider.enabled = true;
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
    }

    private IEnumerator IncreaseDashCounter()
    {
        while (_counter < 100)
        {
            _counter++;
            yield return new WaitForSeconds(_counterDelay);
        }
        _increaseDashCoroutine = null;
    }
}
