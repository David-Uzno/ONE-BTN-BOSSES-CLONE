using UnityEngine;

public static class LevelLayoutResolver
{
    private static ILevelLayout _cachedLayout;

    public static ILevelLayout Resolve(Vector2 fallbackCenter)
    {
        if (LevelLayoutProvider.Instance != null)
        {
            _cachedLayout = LevelLayoutProvider.Instance.GetLayout();
            if (_cachedLayout != null)
            {
                return _cachedLayout;
            }
        }

        if (_cachedLayout != null)
        {
            return _cachedLayout;
        }

        float fallbackRadius;
        if (GameManager.Instance != null)
        {
            fallbackRadius = GameManager.Instance._radius;
        }
        else
        {
            fallbackRadius = 7.5f;
        }
        _cachedLayout = CreateDefaultLayout(fallbackCenter, fallbackRadius);
        return _cachedLayout;
    }

    public static ILevelLayout Resolve()
    {
        return Resolve(Vector2.zero);
    }

    public static void ResetCache()
    {
        _cachedLayout = null;
    }

    private static ILevelLayout CreateDefaultLayout(Vector2 center, float radius)
    {
        Debug.LogWarning("No se encontró un LevelLayoutProvider en escena. Se está cargando el layout predeterminado.");
        return new CircularPath(center, radius);
    }
}
