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
    public static UnityEvent<int> OnMovementSelected = new UnityEvent<int>();
    public static int SelectedIndex {get; private set;} = 0;

    [Header("Dependencies")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private TransitionMaganer _transitionManager;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerIndex"))
        {
            _currentIndex = PlayerPrefs.GetInt("PlayerIndex");
        }
        else
        {
            _currentIndex = 0;
        }

        UpdateInformation(_movementData.Characters[_currentIndex]);
    }

    private void SaveCurrentIndex()
    {
        PlayerPrefs.SetInt("PlayerIndex", _currentIndex);
    }

    private void UpdateInformation(MovementData.Character character)
    {
        _titleTextUI.text = character.Title;
        _descriptionTextUI.text = character.Description;

        if (character.VideoPlayer != null)
        {
            _videoPlayer.clip = character.VideoPlayer;
            _videoPlayer.Play();
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
