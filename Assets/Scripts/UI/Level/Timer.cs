using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    #region Variables
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _currentTimeText;         
    [SerializeField] private TextMeshProUGUI _finalTimeDisplayText;

    [Header("Messages")]
    [SerializeField] private GameObject _recordMessage;

    [Header("Level Settings")]
    private float _elapsedTime;
    private float _bestTime;

    private bool _isGameOver = false;    

    [Header("References")]
    private SaveManager _saveManager;
    private GameManager _gameManager;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _saveManager = FindObjectOfType<SaveManager>();
    }

    private void Start()
    {
        LoadBestTime();
    }

    private void Update()
    {
        if (!_isGameOver)
        {
            AccumulateTime();
            UpdateTimeUI();
        }
    }
    #endregion

    #region Time Management
    private void AccumulateTime()
    {
        _elapsedTime += Time.deltaTime;
    }

    private void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((_elapsedTime - Mathf.Floor(_elapsedTime)) * 1000);

        _currentTimeText.text = string.Format("{0}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void StopGameTimer()
    {
        _isGameOver = true;

        if (_finalTimeDisplayText != null)
        {
            _finalTimeDisplayText.text = FormatTime(_elapsedTime);
        }
        else
        {
            Debug.LogError("_finalTimeDisplayText no está asignado.");
        }

        if (_elapsedTime < _bestTime)
        {
            _bestTime = _elapsedTime;
            if (_saveManager != null && _gameManager != null)
            {
                _saveManager.SavePoint(_bestTime, _gameManager._currentLevel);
            }
            else
            {
                Debug.LogError("_saveManager o _gameManager no está asignado.");
            }

            if (_recordMessage != null)
            {
                _recordMessage.SetActive(true);
            }
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 1000);
        
        return string.Format("{0}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
    #endregion

    #region Data Handling
    private void LoadBestTime()
    {
        if (_saveManager != null)
        {
            if (_gameManager._currentLevel < 0)
            {
                Debug.LogError("El nivel actual no puede ser negativo.");
                _bestTime = float.MaxValue;
                return;
            }

            var data = _saveManager.LoadData();
            if (data._pointsPerLevel.Count > _gameManager._currentLevel)
            {
                _bestTime = data._pointsPerLevel[_gameManager._currentLevel];
            }
            else
            {
                _bestTime = float.MaxValue;
            }
        }
        else
        {
            Debug.LogError("SaveManager no encontrado en la escena.");
            _bestTime = float.MaxValue;
        }
    }
    #endregion
}
