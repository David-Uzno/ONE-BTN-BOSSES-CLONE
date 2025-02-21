using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
#endregion

#region Movements
    void Start()
    {
        if (_movementsData.Count > 0)
        {
            ActivateMovement(0);
        }
    }

    private void ActivateMovement(int index)
    {
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
        gameObject.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + LoadLevelIndex.Instance._currentLevelIndex);
    }

    public void OnPreviousMovement()
    {
        if (_movementsData.Count == 0) return;

        ActivateMovement((_currentIndex - 1 + _movementsData.Count) % _movementsData.Count);
    }

    public void OnNextMovement()
    {
        if (_movementsData.Count == 0) return;

        ActivateMovement((_currentIndex + 1) % _movementsData.Count);
    }
#endregion
}
