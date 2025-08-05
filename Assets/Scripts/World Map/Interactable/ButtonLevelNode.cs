using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLevelNode : MonoBehaviour
{
    [Header("Interactable")]
    [SerializeField] private bool _isInteractable = true;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _alphaInactiveOpacity = 0.5f;

    [Header("Information")]
    [SerializeField] private ushort _buttonIndex;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateAlpha();
    }

    private void OnMouseDown()
    {
        if (_isInteractable)
        {
            OverworldUI.Instance.GetPlayerGameObject().transform.position = this.transform.position;
            LoadLevelIndex.Instance._currentLevelIndex = _buttonIndex;
        }
    }

    public void SetInteractable(bool interactable)
    {
        _isInteractable = interactable;
        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        if (_spriteRenderer != null)
        {
            Color color = _spriteRenderer.color;
            if (_isInteractable)
            {
                color.a = 1f;
            }
            else
            {
                color.a = _alphaInactiveOpacity;
            }
            _spriteRenderer.color = color;
        }
    }
}
