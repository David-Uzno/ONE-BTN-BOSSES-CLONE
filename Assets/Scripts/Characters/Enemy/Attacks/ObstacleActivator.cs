using System.Collections;
using UnityEngine;

public class ObstacleActivator
{
    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _polygonCollider;

    public ObstacleActivator(SpriteRenderer spriteRenderer, PolygonCollider2D polygonCollider)
    {
        _spriteRenderer = spriteRenderer;
        _polygonCollider = polygonCollider;
    }

    public IEnumerator ActivateObstacleWithDelay(float activationDelay)
    {
        yield return new WaitForSeconds(activationDelay);
        SetSpriteAlpha(1f);
        EnableCollider(true);
    }

    public void InitializeObstacle(float initialAlpha)
    {
        SetSpriteAlpha(initialAlpha);
        EnableCollider(false);
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (_spriteRenderer != null)
        {
            Color color = _spriteRenderer.color;
            color.a = alpha;
            _spriteRenderer.color = color;
        }
    }

    private void EnableCollider(bool isEnabled)
    {
        if (_polygonCollider != null)
        {
            _polygonCollider.enabled = isEnabled;
        }
    }
}
