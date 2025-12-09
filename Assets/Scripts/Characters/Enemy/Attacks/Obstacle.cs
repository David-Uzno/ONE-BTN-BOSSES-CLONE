using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
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
    }

    private void OnEnable()
    {
        SetInitialTransparency();
        DisableCollider();
        StartCoroutine(ActivateObstacle());
        CancelInvoke(nameof(DeactivateGameObject));
        Invoke(nameof(DeactivateGameObject), _disableDelay);
    }

    private void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _healthManager = FindFirstObjectByType<HealthManager>();
    }

    private void SetInitialTransparency()
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

    private IEnumerator ActivateObstacle()
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

    private void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_polygonCollider != null && _polygonCollider.enabled && other.CompareTag("Player") && _healthManager != null)
        {
            _healthManager.TakeDamage(1);
        }
    }
}
