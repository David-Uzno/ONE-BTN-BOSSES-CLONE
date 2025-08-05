using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OverworldUI : MonoBehaviour
{
    #region Variables
    public static OverworldUI Instance { get; private set; }

    [Header("UI & Transition")]
    [SerializeField] private GameObject _powerUpsUI;
    [SerializeField] private SelectMovement _selectCharacter;
    [SerializeField] private TransitionMaganer _transitionManager;

    [Header("Player Dependencies")]
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private PlayerInput _playerInput;

    [HideInInspector] public bool IsActionPressed = false;
    #endregion

    #region Unity Methods
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

        if (_selectCharacter == null)
        {
            _selectCharacter = Object.FindFirstObjectByType<SelectMovement>();
            if (_selectCharacter == null)
            {
                Debug.LogError("SelectCharacter no encontrado en la escena.");
            }
        }

        if (_transitionManager == null)
        {
            _transitionManager = Object.FindFirstObjectByType<TransitionMaganer>();
            if (_transitionManager == null)
            {
                Debug.LogError("TransitionMaganer no encontrado en la escena.");
            }
        }
    }

    private void Start()
    {
        _playerInput.actions["Accept"].performed += OnActionPerfomed;
        _playerInput.actions["Cancel"].performed += OnCancelPerformed;

        _playerInput.actions["Left"].performed -= OnLeftPerformed;
        _playerInput.actions["Right"].performed -= OnRightPerformed;
    }

    private void OnDestroy()
    {
        _playerInput.actions["Accept"].performed -= OnActionPerfomed;
        _playerInput.actions["Cancel"].performed -= OnCancelPerformed;

        _playerInput.actions["Left"].performed -= OnLeftPerformed;
        _playerInput.actions["Right"].performed -= OnRightPerformed;
    }
    #endregion

    #region Input Handlers
    private void OnActionPerfomed(InputAction.CallbackContext context)
    {
        if (IsActionPressed == false)
        {
            _transitionManager.SaveCameraPosition();

            _powerUpsUI.SetActive(true);
            _transitionManager.StartTransition();

            IsActionPressed = true;

            _playerInput.actions["Left"].performed += OnLeftPerformed;
            _playerInput.actions["Right"].performed += OnRightPerformed;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + LoadLevelIndex.Instance._currentLevelIndex);
        }
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        DeactivatePowerUpsUI();

        _playerInput.actions["Left"].performed -= OnLeftPerformed;
        _playerInput.actions["Right"].performed -= OnRightPerformed;
    }

    private void DeactivatePowerUpsUI()
    {
                _powerUpsUI.SetActive(false);
        _transitionManager.RevertTransition();

        IsActionPressed = false;
    }

    private void OnLeftPerformed(InputAction.CallbackContext context)
    {
        if (IsActionPressed)
        {
            if (_selectCharacter != null)
            {
                _selectCharacter.NextCharacter();
            }
            else
            {
                Debug.LogError("_selectCharacter es null. No se puede ejecutar OnLeftPerformed.");
            }
        }
        else
        {
            Debug.LogWarning("OnActionPerformed no ha sido activado. No se puede ejecutar OnLeftPerformed.");
        }
    }

    private void OnRightPerformed(InputAction.CallbackContext context)
    {
        if (IsActionPressed)
        {
            if (_selectCharacter != null)
            {
                _selectCharacter.PreviousCharacter();
            }
            else
            {
                Debug.LogError("_selectCharacter es null. No se puede ejecutar OnRightPerformed.");
            }
        }
        else
        {
            Debug.LogWarning("OnActionPerformed no ha sido activado. No se puede ejecutar OnRightPerformed.");
        }
    }
    #endregion

    #region Public Methods
    public GameObject GetPlayerGameObject()
    {
        return _playerGameObject;
    }
    #endregion
}
