using UnityEngine;

public enum LevelLayoutType
{
    Circular
}

public interface ILevelLayout
{
    Vector2 GetPoint(float parameter);
    float GetParameter(Vector2 position);
    Vector2 GetCenter();
}

public interface IRadialLayout
{
    float Radius { get; }
}
