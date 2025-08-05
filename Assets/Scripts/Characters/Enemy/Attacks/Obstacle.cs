using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float _initialAlpha = 0.5f;
    [SerializeField] private float _activationDelay = 1f;
    [SerializeField] private float _disableDelay = 3f;

    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _polygonCollider;
    private HealthManager _healthManager;

    private void Awake()
    {
        InitializeComponents();
        SetInitialAlpha();
        DisableCollider();

        StartCoroutine(ActivateTriangle());
        Invoke("DisableGameObject", _disableDelay);
    }

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _healthManager = Object.FindFirstObjectByType<HealthManager>();
    }

    private void SetInitialAlpha()
    {
        if (_spriteRenderer != null)
        {
            var color = _spriteRenderer.color;
            color.a = _initialAlpha;
            _spriteRenderer.color = color;
        }
    }

    private void DisableCollider()
    {
        if (_polygonCollider != null)
        {
            _polygonCollider.enabled = false;
        }
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

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _healthManager != null)
        {
            _healthManager.TakeDamage(1);
        }
    }
}
