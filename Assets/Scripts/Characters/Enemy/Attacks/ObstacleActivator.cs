using System.Collections;
using UnityEngine;

public class ObstacleActivator
{
    private readonly SpriteRenderer _spriteRenderer;
    private readonly PolygonCollider2D _polygonCollider;

    public ObstacleActivator(SpriteRenderer spriteRenderer, PolygonCollider2D polygonCollider)
    {
        _spriteRenderer = spriteRenderer;
        _polygonCollider = polygonCollider;
    }

    public IEnumerator Activate(float activationDelay)
    {
        yield return new WaitForSeconds(activationDelay);

        if (_spriteRenderer != null)
        {
            var color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;
        }

        if (_polygonCollider != null)
        {
            _polygonCollider.enabled = true;
        }
    }

    public void Initialize(float initialAlpha)
    {
        if (_spriteRenderer != null)
        {
            var color = _spriteRenderer.color;
            color.a = initialAlpha;
            _spriteRenderer.color = color;
        }

        if (_polygonCollider != null)
        {
            _polygonCollider.enabled = false;
        }
    }
}