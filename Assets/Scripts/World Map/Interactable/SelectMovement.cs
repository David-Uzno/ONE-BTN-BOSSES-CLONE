using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class SelectMovement : MonoBehaviour
{
#region Variables
    [SerializeField] private List<MovementData> _movementsData;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _titleTextUI;
    [SerializeField] private TextMeshProUGUI _descriptionTextUI;
    [SerializeField] private VideoPlayer _videoPlayer;

    [Header("Selection")]
    private int _currentIndex = 0;
    private GameObject _currentActiveMovement;
    public static UnityEvent<int> OnMovementSelected = new UnityEvent<int>();
    public static int SelectedIndex {get; private set;} = 0;

    [Header("Dependencies")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private TransitionMaganer _transitionManager;
    private GameObject _powerUpsUI;
#endregion

#region Movements
    private void Start()
    {
        _playerInput.actions["Left"].performed += OnPreviousMovementPerformed;
        _playerInput.actions["Right"].performed += OnNextMovementPerformed;

        _powerUpsUI = this.gameObject;

        if (_movementsData.Count > 0)
        {
            ActivateMovement(0);
        }
    }

    private void OnDestroy()
    {
        if (_playerInput != null)
        {
            _playerInput.actions["Left"].performed -= OnPreviousMovementPerformed;
            _playerInput.actions["Right"].performed -= OnNextMovementPerformed;
        }
    }

    private void ActivateMovement(int index)
    {
        if (!gameObject.activeInHierarchy) return;

        if (_currentActiveMovement != null)
        {
            Destroy(_currentActiveMovement);
        }

        _currentIndex = index;
        SelectedIndex = index;
        MovementData movement = _movementsData[_currentIndex];

        if (movement.MovementPrefab != null)
        {
            _currentActiveMovement = Instantiate(movement.MovementPrefab, transform);
            _currentActiveMovement.transform.SetSiblingIndex(1);
        }

        UpdateInformation(movement);
        OnMovementSelected.Invoke(_currentIndex);
    }


    private void UpdateInformation(MovementData movement)
    {
        _titleTextUI.text = movement.Title;
        _descriptionTextUI.text = movement.Description;

        if (movement.VideoClip != null)
        {
            _videoPlayer.clip = movement.VideoClip;
            _videoPlayer.Play();
        }
    }
#endregion

#region Buttons
    public void Back()
    {
        _powerUpsUI.SetActive(false);
        _transitionManager.RevertTransition();

        PlayerOverworld.Instance.IsActionPressed = false;
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + LoadLevelIndex.Instance._currentLevelIndex);
    }

    public void OnPreviousMovement()
    {
        HandleMovementChange(-1);
    }

    public void OnNextMovement()
    {
        HandleMovementChange(1);
    }

    private void OnPreviousMovementPerformed(InputAction.CallbackContext context)
    {
        HandleMovementChange(-1);
    }

    private void OnNextMovementPerformed(InputAction.CallbackContext context)
    {
        HandleMovementChange(1);
    }

    private void HandleMovementChange(int direction)
    {
        if (_movementsData.Count == 0) return;

        int newIndex = (_currentIndex + direction + _movementsData.Count) % _movementsData.Count;
        ActivateMovement(newIndex);
    }
#endregion
}
