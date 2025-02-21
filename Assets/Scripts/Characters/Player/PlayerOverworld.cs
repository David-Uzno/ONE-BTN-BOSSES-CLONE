using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerOverworld : MonoBehaviour 
{
    public static PlayerOverworld Instance { get; private set; }

    [HideInInspector] public bool IsActionPressed = false;

    [Header("Player Dependencies")]
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private PlayerInput _playerInput;

    [Header("UI & Transition")]
    [SerializeField] private GameObject _powerUpsUI;
    [SerializeField] private TransitionMaganer _transitionManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_playerGameObject == null)
        {
            _playerGameObject = GameObject.Find("PlayerOverworld");
            if (_playerGameObject == null)
            {
                Debug.LogError("PlayerOverworld GameObject no encontrado.");
            }
        }
    }

    private void Start()
    {
        _playerInput.actions["Accept"].performed += OnActionPerfomed;
        _playerInput.actions["Cancel"].performed += OnCancelPerformed;
    }

    private void OnDestroy()
    {
        _playerInput.actions["Accept"].performed -= OnActionPerfomed;
        _playerInput.actions["Cancel"].performed -= OnCancelPerformed;
    }

    private void OnActionPerfomed(InputAction.CallbackContext context)
    {
        if (IsActionPressed == false)
        {
            _transitionManager.SaveCameraPosition();
        
            _powerUpsUI.SetActive(true);
            _transitionManager.StartTransition();

            IsActionPressed = true;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + LoadLevelIndex.Instance._currentLevelIndex);
        }

        
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        _powerUpsUI.SetActive(false);
        _transitionManager.RevertTransition();

        IsActionPressed = false;
    }

    public GameObject GetPlayerGameObject()
    {
        return _playerGameObject;
    }
}
