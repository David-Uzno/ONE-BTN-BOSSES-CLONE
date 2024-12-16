using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonLevel : MonoBehaviour
{
    public delegate void ButtonClicked(ButtonLevel button);
    public static event ButtonClicked OnButtonClicked;

    [SerializeField] private bool _isInteractable = true;
    public int buttonIndex;

    [Header("Dependencies")]
    public PlayerInput _playerInput;

    private bool _isShootingPressed;

    private void OnMouseDown()
    {
        if (_isInteractable)
        {
            PlayerController.Instance.GetPlayerGameObject().transform.position = this.transform.position;

            StartCoroutine(WaitForShootInput());
        }
    }

    private IEnumerator WaitForShootInput()
    {
        while (true)
        {
            bool isCurrentShootingPressed = _playerInput.actions["Shoot"].ReadValue<float>() > 0;

            if (isCurrentShootingPressed && !_isShootingPressed)
            {
                OnButtonClicked?.Invoke(this);
                yield break;
            }

            _isShootingPressed = isCurrentShootingPressed;
            yield return null;
        }
    }

    public void SetInteractable(bool interactable)
    {
        _isInteractable = interactable;
    }
}