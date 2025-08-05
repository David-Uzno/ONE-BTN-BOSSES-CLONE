using UnityEngine;

public class ButtonLevelNode : MonoBehaviour
{
    [Header("Interactable Settings")]
    [SerializeField] private bool _isInteractable = true;
    [SerializeField] private float _inactiveAlphaOpacity = 0.5f;
    private SpriteRenderer _spriteRenderer;

    [Header("Level Information")]
    [SerializeField] private ushort _levelIndex;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSpriteAlpha();
    }

    private void OnMouseDown()
    {
        if (_isInteractable)
        {
            MovePlayerToNode();
            SetCurrentLevelIndex();
        }
    }

    public void SetInteractable(bool isInteractable)
    {
        _isInteractable = isInteractable;
        UpdateSpriteAlpha();
    }

    private void MovePlayerToNode()
    {
        OverworldUI.Instance.GetPlayerGameObject().transform.position = transform.position;
    }

    private void SetCurrentLevelIndex()
    {
        LoadLevelIndex.Instance._currentLevelIndex = _levelIndex;
    }

    private void UpdateSpriteAlpha()
    {
        if (_spriteRenderer != null)
        {
            Color color = _spriteRenderer.color;
            color.a = _isInteractable ? 1f : _inactiveAlphaOpacity;
            _spriteRenderer.color = color;
        }
    }
}
