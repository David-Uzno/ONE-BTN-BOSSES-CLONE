using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SelectMovement : MonoBehaviour
{
    [SerializeField] private MovementData _movementData;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _titleTextUI;
    [SerializeField] private TextMeshProUGUI _descriptionTextUI;
    [SerializeField] private VideoPlayer _videoPlayer;
    private GameObject _currentIcon;

    [Header("Selection")]
    private int _currentIndex;
    public static UnityEvent<int> OnMovementSelected = new();
    public static int SelectedIndex {get; private set;} = 0;

    [Header("Dependencies")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private TransitionMaganer _transitionManager;
    
    private void Start()
    {
        if (_movementData == null || _movementData.Characters == null || _movementData.Characters.Count == 0)
        {
            Debug.LogError("SelectMovement requiere MovementData con al menos un Character.");
            return;
        }

        if (PlayerPrefs.HasKey("PlayerIndex"))
        {
            _currentIndex = PlayerPrefs.GetInt("PlayerIndex");
        }
        else
        {
            _currentIndex = 0;
        }

        int characterCount = _movementData.Characters.Count;
        _currentIndex = Mathf.Clamp(_currentIndex, 0, characterCount - 1);
        var character = _movementData.Characters[_currentIndex];
        if (character == null)
        {
            Debug.LogError("SelectMovement requiere un Character válido en MovementData.");
            return;
        }

        UpdateInformation(character);
    }

    private void SaveCurrentIndex()
    {
        PlayerPrefs.SetInt("PlayerIndex", _currentIndex);
    }

    private void UpdateInformation(MovementData.Character character)
    {
        if (character == null)
        {
            Debug.LogError("SelectMovement recibió un Character nulo.");
            return;
        }

        if (_titleTextUI != null)
        {
            _titleTextUI.text = character.Title;
        }
        else
        {
            Debug.LogWarning("SelectMovement: falta asignar _titleTextUI.");
        }

        if (_descriptionTextUI != null)
        {
            _descriptionTextUI.text = character.Description;
        }
        else
        {
            Debug.LogWarning("SelectMovement: falta asignar _descriptionTextUI.");
        }

        if (character.VideoPlayer != null && _videoPlayer != null)
        {
            _videoPlayer.clip = character.VideoPlayer;
            _videoPlayer.Play();
        }
        else if (character.VideoPlayer != null)
        {
            Debug.LogWarning("SelectMovement: se intentó reproducir un VideoPlayer pero falta el componente asignado.");
        }

        if (_currentIcon != null)
        {
            _currentIcon.SetActive(false);
        }

        if (character.Icon != null)
        {
            if (_currentIcon == null || _currentIcon.name != character.Icon.name)
            {
                if (_currentIcon != null)
                {
                    Destroy(_currentIcon);
                }
                _currentIcon = Instantiate(character.Icon, transform);
                _currentIcon.transform.SetSiblingIndex(1);
                _currentIcon.name = character.Icon.name;
            }
            _currentIcon.SetActive(true);
        }
    }

    public void NextCharacter()
    {
        // Circular al siguiente índice
        if (_currentIndex == _movementData.Characters.Count - 1)
        {
            _currentIndex = 0;
        }
        else
        {
            _currentIndex += 1;
        }

        SaveCurrentIndex();
        UpdateInformation(_movementData.Characters[_currentIndex]);
    }

    public void PreviousCharacter()
    {
        // Circular al anterior índice
        if (_currentIndex == 0)
        {
            _currentIndex = _movementData.Characters.Count - 1;
        }
        else
        {
            _currentIndex -= 1;
        }

        SaveCurrentIndex();
        UpdateInformation(_movementData.Characters[_currentIndex]);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
