using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float _initialAlpha = 0.5f;
    [SerializeField] private float _activationDelay = 1f;

    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _polygonCollider;
    private HealthManager _healthManager;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();

        _healthManager = Object.FindFirstObjectByType<HealthManager>();

        if (_spriteRenderer != null)
        {
            var color = _spriteRenderer.color;
            color.a = _initialAlpha;
            _spriteRenderer.color = color;
        }

        if (_polygonCollider != null)
        {
            _polygonCollider.enabled = false;
        }

        StartCoroutine(ActivateTriangle());
    }

    private IEnumerator ActivateTriangle()
    {
        yield return new WaitForSeconds(_activationDelay);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _healthManager != null)
        {
            _healthManager.TakeDamage(1);
        }
    }
}
