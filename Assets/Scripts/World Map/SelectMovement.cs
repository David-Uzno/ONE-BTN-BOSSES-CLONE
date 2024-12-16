using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using Unity.VisualScripting;

public class SelectMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> _movements;

    [Header("Titles")]
    [SerializeField] private string[] _textTitles;
    [SerializeField] private TextMeshProUGUI _titleTextUI;

    [Header("Descriptions")]
    [SerializeField] private string[] _textDescriptions;
    [SerializeField] private TextMeshProUGUI _descriptionTextUI;

    [Header("Videos")]
    [SerializeField] private VideoClip[] _videoClips;
    [SerializeField] private VideoPlayer _videoPlayer;

    private int _currentIndex = 0;

    void Start()
    {
        foreach (GameObject movement in _movements)
        {
            movement.SetActive(false);
        }

        if (_movements.Count > 0)
        {
            _movements[_currentIndex].SetActive(true);
        }
    }

    public void OnPreviousMovement()
    {
        if (_movements.Count == 0) return;

        _movements[_currentIndex].SetActive(false);

        _currentIndex = (_currentIndex - 1 + _movements.Count) % _movements.Count;

        _movements[_currentIndex].SetActive(true);

        UpdateInformation();
    }

    public void OnNextMovement()
    {
        if (_movements.Count == 0) return;

        _movements[_currentIndex].SetActive(false);

        _currentIndex = (_currentIndex + 1) % _movements.Count;

        _movements[_currentIndex].SetActive(true);

        UpdateInformation();
    }

    private void UpdateInformation()
    {
        if(_currentIndex >= 0 && _currentIndex < _videoClips.Length)
        {
            _titleTextUI.text = _textTitles[_currentIndex];
            _descriptionTextUI.text = _textDescriptions[_currentIndex];

            _videoPlayer.clip = _videoClips[_currentIndex];
            _videoPlayer.Play();
        }
    }
}
