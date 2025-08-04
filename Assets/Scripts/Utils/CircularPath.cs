using UnityEngine;

public class CircularPath
{
    private Vector2 _center;
    private GameManager _gameManager;

    public CircularPath(Vector2 center)
    {
        _center = center;
        _gameManager = GameManager.Instance;
    }

    public float GetRadius()
    {
        return _gameManager._radius;
    }

    public Vector2 GetPosition(float angleRadians)
    {
        return _center + new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)) * _gameManager._radius;
    }

    public float GetAngle(Vector2 position)
    {
        return Mathf.Atan2(position.y - _center.y, position.x - _center.x);
    }
}
