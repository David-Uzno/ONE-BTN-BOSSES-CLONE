using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashPlayer : PlayerMovement
{
    [Header("Dash")]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private byte _dashCounter = 100;

    protected override void Movement()
    {
        _collider.enabled = false;
    }

    protected override void StopMovement()
    {
        _collider.enabled = true;
    }
}
