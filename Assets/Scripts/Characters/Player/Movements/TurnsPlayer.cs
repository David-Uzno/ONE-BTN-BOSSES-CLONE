using UnityEngine;

public class TurnsPlayer : PlayerMovement
{
    protected override void Movement()
    {
        _movementController._rotationDirection *= -1;
    }
}
