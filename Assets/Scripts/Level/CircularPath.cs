using UnityEngine;

public class CircularPath : ILevelLayout, IRadialLayout
{
    private readonly Vector2 _center;
    private readonly float _radius;

    public CircularPath(Vector2 center, float radius)
    {
        _center = center;
        _radius = radius;
    }

    public float Radius => _radius;

    public Vector2 GetPoint(float angleRadians)
    {
        return _center + new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)) * _radius;
    }

    public float GetParameter(Vector2 position)
    {
        return Mathf.Atan2(position.y - _center.y, position.x - _center.x);
    }

    public Vector2 GetCenter()
    {
        return _center;
    }

    public float GetRadius()
    {
        return Radius;
    }

    public Vector2 GetPosition(float angleRadians)
    {
        return GetPoint(angleRadians);
    }

    public float GetAngle(Vector2 position)
    {
        return GetParameter(position);
    }
}
