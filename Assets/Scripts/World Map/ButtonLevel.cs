using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private bool _isInteractable = true;
    public int buttonIndex;

    private void OnMouseDown()
    {
        if (_isInteractable)
        {
            PlayerOverworld.Instance.GetPlayerGameObject().transform.position = this.transform.position;
        }
    }

    public void SetInteractable(bool interactable)
    {
        _isInteractable = interactable;
    }
}
