using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnsPlayer : PlayerMovement
{
    protected override void Movement()
    {
        _movementController._rotationDirection *= -1;
    }
}
